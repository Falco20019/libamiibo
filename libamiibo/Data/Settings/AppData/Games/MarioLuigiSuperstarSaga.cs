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
using LibAmiibo.Helper;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using LibAmiibo.Attributes;
using Newtonsoft.Json;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x00194B00)]
    [AppDataInitializationTitleID("00040000001B9000")]
    [AppDataInitializationTitleID("0004000000194B00")]
    [AppDataInitializationTitleID("00040000001B8F00")]
    public class MarioLuigiSuperstarSaga : IGame
    {
        public enum CharacterCheckValue
        {
            Goomba = 'K',
            KoopaTroopa = 'N',
            Boo = 'T'
        }

        private ArraySegment<byte> AppData { get; set; }

        private static readonly uint[] XpToLevel = new uint[] {
            0x000001,
            0x000002,
            0x000004,
            0x000006,
            0x00000A,
            0x000014,
            0x000028,
            0x000046,
            0x00006E,
            0x0000A0,
            0x0000DC,
            0x000122,
            0x000172,
            0x0001CC,
            0x000230,
            0x0002F8,
            0x0003E8,
            0x00054B,
            0x0006DB,
            0x0008BB,
            0x000AEB,
            0x000D6B,
            0x00100E,
            0x0012FC,
            0x0018D8,
            0x0021A2,
            0x002D5A,
            0x003C00,
            0x004D94,
            0x006216,
            0x007986,
            0x009768,
            0x00B89C,
            0x00E074,
            0x010EF0,
            0x014410,
            0x017FD4,
            0x01C23C,
            0x0211EC,
            0x02871C,
            0x02FC4C,
            0x03717C,
            0x03E6AC,
            0x045BDC,
            0x04D10C,
            0x05463C,
            0x05BB6C,
            0x06309C,
            0x06A5CC,
            0x071AFC,
            0x07902C,
            0x08055C,
            0x087A8C,
            0x08EFBC,
            0x0964EC,
            0x09DA1C,
            0x0A4F4C,
            0x0AC47C,
            0x0B39AC,
            0x0BAEDC,
            0x0C240C,
            0x0C993C,
            0x0D0E6C,
            0x0D839C,
            0x0DF8CC,
            0x0E6DFC,
            0x0EE32C,
            0x0F585C,
            0x0FCD8C,
            0x1042BC,
            0x10B7EC,
            0x112D1C,
            0x11A24C,
            0x12177C,
            0x128CAC,
            0x1301DC,
            0x13770C,
            0x13EC3C,
            0x14616C,
            0x14D69C,
            0x154BCC,
            0x15C0FC,
            0x16362C,
            0x16AB5C,
            0x17208C,
            0x1795BC,
            0x180AEC,
            0x18801C,
            0x18F54C,
            0x196A7C,
            0x19DFAC,
            0x1A54DC,
            0x1ACA0C,
            0x1B3F3C,
            0x1BB46C,
            0x1C299C,
            0x1C9ECC,
            0x1D13FC,
            0x1D892C
        };

        [Flags]
        public enum StampsEarnedValue : ulong
        {
            [Description("Goomba Squad")]
            GoombaSquad = 1 << 23,
            [Description("Koopa Troopa Squad")]
            KoopaTroopaSquad = 1 << 22,
            [Description("Shy Guy Squad")]
            ShyGuySquad = 1 << 21,
            [Description("Boo Squad")]
            BooSquad = 1 << 20,
            [Description("The Trio")]
            TheTrio = 1 << 19,
            [Description("Goombas Galore")]
            GoombasGalore = 1 << 18,
            [Description("Koopa Paratroopa Squad")]
            KoopaParatroopaSquad = 1 << 17,
            [Description("So Many Shy Guys")]
            SoManyShyGuys = 1 << 16,
            [Description("Beaucoup Boos")]
            BeaucoupBoos = 1 << 15,
            [Description("The Four Captains")]
            TheFourCaptains = 1 << 14,
            [Description("Buzzy Beetle and Dry Bones")]
            BuzzyBeetleAndDryBones = 1 << 13,
            [Description("Hammer Bros.")]
            HammerBros = 1 << 12,
            Lakitu = 1 << 11,
            [Description("Chargin’ Chuck and Bob-omb")]
            CharginChuckAndBobomb = 1 << 10,
            [Description("Piranha Plant and Pokey")]
            PiranhaPlantAndPokey = 1 << 9,
            [Description("Chain Chomp and Broozer")]
            ChainChompAndBroozer = 1 << 8,
            Magikoopa = 1 << 7,
            [Description("Koopalings 1")]
            Koopalings1 = 1 << 6,
            [Description("Koopalings 2")]
            Koopalings2 = 1 << 5,
            [Description("Special Guest 1")]
            SpecialGuest1 = 1 << 4,
            [Description("Special Guest 2")]
            SpecialGuest2 = 1 << 3,
            [Description("Special Guest 3")]
            SpecialGuest3 = 1 << 2,
            [Description("Special Guest 4")]
            SpecialGuest4 = 1 << 1,
            [JsonIgnore]
            Undefined = 0

        }

        public ArraySegment<byte> PublisherTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x00, 0x07); }
            set { PublisherTag.CopyFrom(value); }
        }

        public CharacterCheckValue CharacterCheck
        {
            get { return (CharacterCheckValue)AppData.Array[AppData.Offset + 0x07]; }
            set { AppData.Array[AppData.Offset + 0x07] = (byte)value; }
        }

        public ArraySegment<byte> SaveTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x0C, 0x04); }
            set { SaveTag.CopyFrom(value); }
        }

        public uint Experience
        {
            get { return NtagHelpers.UInt24FromTag(AppData, 0x10, true); }
            set { NtagHelpers.UInt24ToTag(AppData, 0x10, value, true); }
        }
        
        public MarioLuigiSuperstarSaga(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(MarioLuigiSuperstarSaga))]
        public class Initializer : IAppDataInitializer
        {
            private static readonly Dictionary<int, CharacterCheckValue> CHARACTER_CHECK_MAP = new Dictionary<int, CharacterCheckValue>
            {
                { 0x0015,   CharacterCheckValue.Goomba },
                { 0x0017,   CharacterCheckValue.Boo },
                { 0x0023,   CharacterCheckValue.KoopaTroopa },
            };
            
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new MarioLuigiSuperstarSaga(tag.AppData);
                game.AppData.CopyFrom(new byte[0x20]); // TODO: Use for-loop and create extension method
                game.PublisherTag.CopyFrom(Encoding.ASCII.GetBytes("REMILLI"));
                // TODO: how is OwnerTag calculated? Based on settings-CRC32?
                game.SaveTag.CopyFrom(new byte[] { 0x3d, 0x0d, 0x2f, 0xc9 });

                if (CHARACTER_CHECK_MAP.TryGetValue(tag.Amiibo.CharacterId, out CharacterCheckValue tmpCharCheck))
                    game.CharacterCheck = tmpCharCheck;
            }
        }

        #region Friendly Bonuses

        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Times shared")]
        public byte FriendlyBonusTimesSharedCount
        {
            get { return AppData.Array[AppData.Offset + 0x09]; }
            set { AppData.Array[AppData.Offset + 0x09] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Brought home")]
        public byte FriendlyBonusBroughtHomeCount
        {
            get { return AppData.Array[AppData.Offset + 0x0A]; }
            set { AppData.Array[AppData.Offset + 0x0A] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Still stored")]
        public byte FriendlyBonusStillStoredCount
        {
            get { return AppData.Array[AppData.Offset + 0x0B]; }
            set { AppData.Array[AppData.Offset + 0x0B] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Super EXP Bean S")]
        public byte FriendlyBonusSuperExpBeanS
        {
            get { return AppData.Array[AppData.Offset + 0x14]; }
            set { AppData.Array[AppData.Offset + 0x14] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Super EXP Bean DX")]
        public byte FriendlyBonusSuperExpBeanDX
        {
            get { return AppData.Array[AppData.Offset + 0x15]; }
            set { AppData.Array[AppData.Offset + 0x15] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Super EXP Bean L")]
        public byte FriendlyBonusSuperExpBeanL
        {
            get { return AppData.Array[AppData.Offset + 0x16]; }
            set { AppData.Array[AppData.Offset + 0x16] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Friendly Bonuses", "Super EXP Bean M")]
        public byte FriendlyBonusSuperExpBeanM
        {
            get { return AppData.Array[AppData.Offset + 0x17]; }
            set { AppData.Array[AppData.Offset + 0x17] = value; }
        }

        #endregion

        #region Stamps Earned
        
        [Cheat(CheatAttribute.Type.CheckBox, "Stamps Earned", "Stamp Earned")]
        public StampsEarnedValue StampsEarned
        {
            get { return (StampsEarnedValue)NtagHelpers.UInt24FromTag(AppData, 0x18, true); }
            set { NtagHelpers.UInt24ToTag(AppData, 0x18, (uint)value, true); }
        }

        #endregion

        #region Misc
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Misc", "Level", Max = 99)]
        public byte Level
        {
            get
            {
                var experience = Experience;
                byte level = 0;
                foreach (var xpThreshold in XpToLevel)
                {
                    if (experience < xpThreshold)
                        break;
                    level++;
                }
                return level;
            }
            set
            {
                if (value < 1)
                {
                    Experience = 0;
                    return;
                }

                if (value > 99)
                    value = 99;

                Experience = XpToLevel[value - 1];
            }
        }

        #endregion
    }
}
