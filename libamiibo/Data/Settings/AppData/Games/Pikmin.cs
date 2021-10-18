/*
 * Copyright (C) 2018 Benjamin Krämer
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
using LibAmiibo.Attributes;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x001A9200)]
    [AppDataInitializationTitleID("00040000001AF800")]
    [AppDataInitializationTitleID("00040000001A9200")]
    [AppDataInitializationTitleID("00040000001AFA00")]
    public class Pikmin : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        private static readonly ushort[] PikminToLevel = new ushort[] {
            0x032,
            0x096,
            0x12C,
            0x1F4,
            0x3E8
        };

        public uint Constant
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x00, true); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x00, value, true); }
        }

        public uint PikminsSaved
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x04, true); }
            set
            {
                NtagHelpers.UInt32ToTag(AppData, 0x04, value, true);
                Level = PikminBasedLevel;
            }
        }

        /// <summary>
        /// Use PikminBasedLevel to safely set the level.
        /// </summary>
        public uint Level
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x08, true) + 1; }
            private set { NtagHelpers.UInt32ToTag(AppData, 0x08, value - 1, true); }
        }
        
        public Pikmin(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(Pikmin))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new Pikmin(tag.AppData);
                game.AppData.CopyFrom(new byte[0x0C]); // TODO: Use for-loop and create extension method
                game.Constant = 1;
            }
        }

        #region General
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "General", "Level")]
        public uint PikminBasedLevel
        {
            get
            {
                var pikmins = PikminsSaved;
                byte level = 1;
                foreach (var pikminThreshold in PikminToLevel)
                {
                    if (pikmins < pikminThreshold)
                        break;
                    level++;
                }
                return level;
            }
            set
            {
                if (value <= 1)
                {
                    PikminsSaved = 0;
                    return;
                }

                if (value > 6)
                    value = 6;

                PikminsSaved = PikminToLevel[value - 2];
            }
        }

        #endregion
    }
}
