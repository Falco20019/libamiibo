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
using Newtonsoft.Json;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x10161F00)]
    [AppDataInitializationTitleID("0005000E10162E00")]
    [AppDataInitializationTitleID("0005000E10161F00")]
    [AppDataInitializationTitleID("0005000E10162D00")]
    public class MarioParty10 : IGame
    {
        private const int TICKS_PER_SECOND = 62156250;
        private static readonly DateTime DateTimeBase = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        private ArraySegment<byte> AppData { get; set; }

        [Flags]
        public enum DataValue
        {
            [JsonIgnore]
            Undefined   = 0b000,
            Available   = 0b001,
            New         = 0b010,
            Selected    = 0b100
        }

        public ArraySegment<byte> PublisherTag1
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x00, 0x02); }
            set { PublisherTag1.CopyFrom(value); }
        }

        public ArraySegment<byte> PublisherTag2
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x64, 0x04); }
            set { PublisherTag2.CopyFrom(value); }
        }

        public byte TimesPlayed
        {
            get { return (byte)(AppData.Array[AppData.Offset + 0x49] - 3); }
            set { AppData.Array[AppData.Offset + 0x49] = Math.Max((byte)(value + 3), byte.MaxValue); }
        }

        private ulong LastDailyBonusDateValue
        {
            get { return NtagHelpers.UInt64FromTag(AppData, 0x50); }
            set { NtagHelpers.UInt64ToTag(AppData, 0x50, value); }
        }

        public DateTime LastDailyBonusDate
        {
            get { return DateTimeBase.AddSeconds(LastDailyBonusDateValue / TICKS_PER_SECOND); }
            set { LastDailyBonusDateValue = (ulong)(value.Subtract(DateTimeBase).TotalSeconds * TICKS_PER_SECOND); }
        }

        public uint SaveTag
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x54); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x54, value); }
        }

        private DataValue GetDataState(int offset) => (DataValue)AppData.Array[AppData.Offset + offset];
        private void SetDataState(int offset, DataValue value) => AppData.Array[AppData.Offset + offset] = (byte)value;
        
        public MarioParty10(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(MarioParty10))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new MarioParty10(tag.AppData);
                game.AppData.CopyFrom(new byte[0xD8]); // TODO: Use for-loop and create extension method
                game.PublisherTag1.CopyFrom(new byte[] { 0x05, 0x68 });
                game.PublisherTag2.CopyFrom(new byte[] { 0x27, 0x7D, 0xFB, 0x70 });
                game.TimesPlayed = 3;
            }
        }

        #region Tokens
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Mario Jump")]
        public DataValue TokenMarioJump
        {
            get { return GetDataState(0x002); }
            set { SetDataState(0x002, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Luigi Jump")]
        public DataValue TokenLuigiJump
        {
            get { return GetDataState(0x003); }
            set { SetDataState(0x003, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "P Switch")]
        public DataValue TokenPSwitch
        {
            get { return GetDataState(0x004); }
            set { SetDataState(0x004, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Bowser Jr.")]
        public DataValue TokenBowserJr
        {
            get { return GetDataState(0x005); }
            set { SetDataState(0x005, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Plus 1")]
        public DataValue TokenPlus1
        {
            get { return GetDataState(0x006); }
            set { SetDataState(0x006, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "1-2-3 Dice Block")]
        public DataValue Token1To3DiceBlock
        {
            get { return GetDataState(0x007); }
            set { SetDataState(0x007, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "4-5-6 Dice Block")]
        public DataValue Token4To6DiceBlock
        {
            get { return GetDataState(0x008); }
            set { SetDataState(0x008, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Reverse Dice Block")]
        public DataValue TokenReverseDiceBlock
        {
            get { return GetDataState(0x009); }
            set { SetDataState(0x009, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Slow Dice Block")]
        public DataValue TokenSlowDiceBlock
        {
            get { return GetDataState(0x00A); }
            set { SetDataState(0x00A, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Double Dice Block")]
        public DataValue TokenDoubleDiceBlock
        {
            get { return GetDataState(0x00B); }
            set { SetDataState(0x00B, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Bronze")]
        public DataValue TokenBronze
        {
            get { return GetDataState(0x00C); }
            set { SetDataState(0x00C, value); }
        }

        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Pow Block")]
        public DataValue TokenPowBlock
        {
            get { return GetDataState(0x00D); }
            set { SetDataState(0x00D, value); }
        }

        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Pipe")]
        public DataValue TokenPipe
        {
            get { return GetDataState(0x00E); }
            set { SetDataState(0x00E, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "? Block")]
        public DataValue TokenQuestionBlock
        {
            get { return GetDataState(0x00F); }
            set { SetDataState(0x00F, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Bowser Space")]
        public DataValue TokenBowserSpace
        {
            get { return GetDataState(0x010); }
            set { SetDataState(0x010, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Plus 5")]
        public DataValue TokenPlus5
        {
            get { return GetDataState(0x011); }
            set { SetDataState(0x011, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Silver")]
        public DataValue TokenSilver
        {
            get { return GetDataState(0x012); }
            set { SetDataState(0x012, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Dash Special")]
        public DataValue TokenDashSpecial
        {
            get { return GetDataState(0x013); }
            set { SetDataState(0x013, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Reverse Special")]
        public DataValue TokenReverseSpecial
        {
            get { return GetDataState(0x014); }
            set { SetDataState(0x014, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Jump Special")]
        public DataValue TokenJumpSpecial
        {
            get { return GetDataState(0x015); }
            set { SetDataState(0x015, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Coin Special")]
        public DataValue TokenCoinSpecial
        {
            get { return GetDataState(0x016); }
            set { SetDataState(0x016, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Star Special")]
        public DataValue TokenStarSpecial
        {
            get { return GetDataState(0x017); }
            set { SetDataState(0x017, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Gold")]
        public DataValue TokenGold
        {
            get { return GetDataState(0x018); }
            set { SetDataState(0x018, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Normal Board")]
        public DataValue TokenNormalBoard
        {
            get { return GetDataState(0x019); }
            set { SetDataState(0x019, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Mario Board")]
        public DataValue TokenMarioBoard
        {
            get { return GetDataState(0x01A); }
            set { SetDataState(0x01A, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Luigi Board")]
        public DataValue TokenLuigiBoard
        {
            get { return GetDataState(0x01B); }
            set { SetDataState(0x01B, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Peach Board")]
        public DataValue TokenPeachBoard
        {
            get { return GetDataState(0x01C); }
            set { SetDataState(0x01C, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Toad Board")]
        public DataValue TokenToadBoard
        {
            get { return GetDataState(0x01D); }
            set { SetDataState(0x01D, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Yoshi Board")]
        public DataValue TokenYoshiBoard
        {
            get { return GetDataState(0x01E); }
            set { SetDataState(0x01E, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Wario Board")]
        public DataValue TokenWarioBoard
        {
            get { return GetDataState(0x01F); }
            set { SetDataState(0x01F, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Rosalina Board")]
        public DataValue TokenRosalinaBoard
        {
            get { return GetDataState(0x020); }
            set { SetDataState(0x020, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Donkey Kong Board")]
        public DataValue TokenDonkeyKongBoard
        {
            get { return GetDataState(0x021); }
            set { SetDataState(0x021, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Tokens", "Bowser Board")]
        public DataValue TokenBowserBoard
        {
            get { return GetDataState(0x022); }
            set { SetDataState(0x022, value); }
        }

        #endregion

        #region Bases
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Mushroom")]
        public DataValue BaseMushroom
        {
            get { return GetDataState(0x02D); }
            set { SetDataState(0x02D, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Coin")]
        public DataValue BaseCoin
        {
            get { return GetDataState(0x02E); }
            set { SetDataState(0x02E, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Star")]
        public DataValue BaseStar
        {
            get { return GetDataState(0x02F); }
            set { SetDataState(0x02F, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Sunflower")]
        public DataValue BaseSunflower
        {
            get { return GetDataState(0x030); }
            set { SetDataState(0x030, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Flower")]
        public DataValue BaseFlower
        {
            get { return GetDataState(0x031); }
            set { SetDataState(0x031, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Heart")]
        public DataValue BaseHeart
        {
            get { return GetDataState(0x032); }
            set { SetDataState(0x032, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Orange")]
        public DataValue BaseOrange
        {
            get { return GetDataState(0x033); }
            set { SetDataState(0x033, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Watermelon")]
        public DataValue BaseWatermelon
        {
            get { return GetDataState(0x034); }
            set { SetDataState(0x034, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Kiwi")]
        public DataValue BaseKiwi
        {
            get { return GetDataState(0x035); }
            set { SetDataState(0x035, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Cookie")]
        public DataValue BaseCookie
        {
            get { return GetDataState(0x036); }
            set { SetDataState(0x036, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Doughnut")]
        public DataValue BaseDoughnut
        {
            get { return GetDataState(0x037); }
            set { SetDataState(0x037, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Shortbread")]
        public DataValue BaseShortbread
        {
            get { return GetDataState(0x038); }
            set { SetDataState(0x038, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Football")]
        public DataValue BaseFootball
        {
            get { return GetDataState(0x039); }
            set { SetDataState(0x039, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Baseball")]
        public DataValue BaseBaseball
        {
            get { return GetDataState(0x03A); }
            set { SetDataState(0x03A, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Tyre")]
        public DataValue BaseTyre
        {
            get { return GetDataState(0x03B); }
            set { SetDataState(0x03B, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Bronze")]
        public DataValue BaseBronze
        {
            get { return GetDataState(0x03C); }
            set { SetDataState(0x03C, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Silver")]
        public DataValue BaseSilver
        {
            get { return GetDataState(0x03D); }
            set { SetDataState(0x03D, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Bases", "Gold")]
        public DataValue BaseGold
        {
            get { return GetDataState(0x03E); }
            set { SetDataState(0x03E, value); }
        }

        #endregion
    }
}
