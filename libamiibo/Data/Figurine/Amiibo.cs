/*
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Resources;

namespace LibAmiibo.Data.Figurine
{
    public class Amiibo
    {
        private ResourceManager resourceManager = Images.Resources.ResourceManager;
        private Image amiiboImage = null;
        private static string ImagePrefix
        {
            get { return "icon_"; }
        }

        public string StatueId { get; private set; }
        public int GameSeriesId
        {
            get
            {
                return int.Parse(StatueId.Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string GameSeriesName
        {
            get
            {
                return GameSeries.GetName(GameSeriesId) ?? "Unknown " + GameSeriesId;
            }
        }
        public byte CharacterNumberInGameSeries
        {
            get
            {
                return byte.Parse(StatueId.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public int CharacterId
        {
            get
            {
                return int.Parse(StatueId.Substring(0, 4), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string CharacterName
        {
            get
            {
                return Character.GetName(CharacterId) ?? "Unknown " + CharacterId;
            }
        }
        public byte CharacterVariant
        {
            get
            {
                return byte.Parse(StatueId.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public long SubCharacterId
        {
            get
            {
                return long.Parse(StatueId.Substring(0, 6), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string SubCharacterName
        {
            get
            {
                if (CharacterVariant == 0x00) return "Regular";
                return SubCharacter.GetName(SubCharacterId) ?? "Unknown " + SubCharacterId;
            }
        }
        public byte ToyTypeId
        {
            get
            {
                return byte.Parse(StatueId.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string ToyTypeName
        {
            get
            {
                return ToyType.GetName(ToyTypeId) ?? "Unknown " + ToyTypeId;
            }
        }
        public int AmiiboNo
        {
            get
            {
                return int.Parse(StatueId.Substring(8, 4), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string RetailName
        {
            get
            {
                // If known, use retail name:
                string retailName = AmiiboName.GetName(AmiiboNo);
                if (retailName != null && (AmiiboNo != 0 || CharacterId == 0))
                    return retailName;

                // Always use the characters name if known, or it's number in the series:
                string character = Character.GetName(CharacterId);
                retailName = character ?? "Char#" + CharacterNumberInGameSeries;

                // Add the game series name (or id if unknown):
                string gameSeries = GameSeries.GetName(GameSeriesId);
                retailName += " (" + gameSeries ?? "series " + GameSeriesId;

                // Only add the variant if not the standard one:
                if (CharacterVariant > 0)
                {
                    // Try to get the subcharacter name or use it's number instead
                    string subCharacter = SubCharacter.GetName(SubCharacterId);
                    retailName += ", " + subCharacter ?? "variant " + CharacterVariant;
                }
                retailName += ")";

                return retailName;
            }
        }
        public byte AmiiboSetId
        {
            get
            {
                return byte.Parse(StatueId.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
            }
        }
        public string AmiiboSetName
        {
            get
            {
                return AmiiboSet.GetName(AmiiboSetId) ?? "Unknown " + AmiiboSetId;
            }
        }

        public Image AmiiboImage
        {
            get
            {
                if (amiiboImage != null)
                    return amiiboImage;

                amiiboImage = Images.Resources.empty;
                if (AmiiboNo == 0xFFFF)
                    return amiiboImage;

                try
                {
                    var correctImage = (Image)resourceManager.GetObject(ImagePrefix + StatueId.ToLower());
                    if (correctImage != null)
                    {
                        amiiboImage = correctImage;
                        return amiiboImage;
                    }
                    
                    foreach (DictionaryEntry entry in resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true))
                    {
                        string fieldName = (string)entry.Key;
                        if (!fieldName.StartsWith(ImagePrefix))
                            continue;

                        Image image = (Image)entry.Value;
                        Amiibo icon = Amiibo.FromStatueId(fieldName.Substring(ImagePrefix.Length));
                        if (icon.AmiiboNo == AmiiboNo)
                        {
                            amiiboImage = image;
                            break;
                        }
                        if (amiiboImage == Images.Resources.empty && icon.CharacterId == CharacterId)
                            amiiboImage = image;
                    }
                }
                catch
                {
                }
                return amiiboImage;
            }
        }

        private Amiibo(byte[] internalTag)
        {
            this.StatueId = String.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}",
                    internalTag[0x1DC], internalTag[0x1DD], internalTag[0x1DE], internalTag[0x1DF], internalTag[0x1E0], internalTag[0x1E1], internalTag[0x1E2], internalTag[0x1E3]);
        }

        private Amiibo(string charId)
        {
            this.StatueId = charId;
        }

        public static Amiibo FromInternalTag(byte[] data)
        {
            return new Amiibo(data);
        }

        public static Amiibo FromNtagData(byte[] data)
        {
            return new Amiibo(String.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}",
                data[84], data[85], data[86], data[87], data[88], data[89], data[90], data[91]));
        }

        public static Amiibo FromStatueId(byte[] statueId)
        {
            return new Amiibo(String.Format("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}{5:X2}{6:X2}{7:X2}",
                    statueId[0], statueId[1], statueId[2], statueId[3], statueId[4], statueId[5], statueId[6], statueId[7]));
        }

        public static Amiibo FromStatueId(string statueId)
        {
            return new Amiibo(statueId);
        }

        public override string ToString()
        {
            return this.RetailName;
        }
    }
}
