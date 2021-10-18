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
using LibAmiibo.Attributes;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x00132600)]
    [AppDataInitializationTitleID("0004000000132800")]
    [AppDataInitializationTitleID("0004000000132600")]
    [AppDataInitializationTitleID("0004000000132700")]
    public class MarioLuigiPaperJam : IGame
    {
        public enum CharacterCheckValue
        {
            Toad = 'K',
            Luigi = 'L',
            Mario = 'M',
            Peach = 'P',
            Bowser = 'T',
            Yoshi = 'Y'
        }
        
        public enum CardStateValue
        {
            Disabled  = 0b00,
            Enabled   = 0b01,
            Sparkling = 0b11
        }
        
        private ArraySegment<byte> AppData { get; set; }

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

        public ArraySegment<byte> OwnerTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x08, 0x08); }
            set { OwnerTag.CopyFrom(value); }
        }

        private CardStateValue GetCardState(int cardId)
        {
            var offset = AppData.Offset + 0x014 + (cardId / 4);
            var shift = (cardId % 4) * 2;
            var mask = (byte)(0b11 << shift);
            var value = AppData.Array[offset] & mask;
            return (CardStateValue)(value >> shift);
        }

        private void SetCardState(int cardId, CardStateValue value)
        {
            var offset = AppData.Offset + 0x014 + (cardId / 4);
            var shift = (cardId % 4) * 2;
            var mask = (byte)(0b11 << shift);
            var shiftValue = (byte)((byte)value << shift);
            AppData.Array[offset] &= (byte)~mask;
            AppData.Array[offset] |= shiftValue;
        }
        
        public MarioLuigiPaperJam(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(MarioLuigiPaperJam))]
        public class Initializer : IAppDataInitializer
        {
            private static readonly Dictionary<int, CharacterCheckValue> CHARACTER_CHECK_MAP = new Dictionary<int, CharacterCheckValue>
            {
                { 0x0000,   CharacterCheckValue.Mario },
                { 0x0001,   CharacterCheckValue.Luigi },
                { 0x0002,   CharacterCheckValue.Peach },
                { 0x0003,   CharacterCheckValue.Yoshi },
                { 0x0005,   CharacterCheckValue.Bowser },
                { 0x000A,   CharacterCheckValue.Toad },
            };
            
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new MarioLuigiPaperJam(tag.AppData);
                game.AppData.CopyFrom(new byte[0x20]); // TODO: Use for-loop and create extension method
                game.PublisherTag.CopyFrom(Encoding.ASCII.GetBytes("MILLION"));
                // TODO: how is OwnerTag calculated? Based on settings-CRC32?
                game.OwnerTag.CopyFrom(new byte[] { 0x3d, 0x0d, 0x2f, 0xc9, 0x34, 0x31, 0x67, 0xef });

                if (CHARACTER_CHECK_MAP.TryGetValue(tag.Amiibo.CharacterId, out CharacterCheckValue tmpCharCheck))
                    game.CharacterCheck = tmpCharCheck;
            }
        }

        #region Level 1

        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 1")]
        public CardStateValue Level1Card1State
        {
            get { return GetCardState(0); }
            set { SetCardState(0, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 2")]
        public CardStateValue Level1Card2State
        {
            get { return GetCardState(1); }
            set { SetCardState(1, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 3")]
        public CardStateValue Level1Card3State
        {
            get { return GetCardState(2); }
            set { SetCardState(2, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 4")]
        public CardStateValue Level1Card4State
        {
            get { return GetCardState(3); }
            set { SetCardState(3, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 5")]
        public CardStateValue Level1Card5State
        {
            get { return GetCardState(4); }
            set { SetCardState(4, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 6")]
        public CardStateValue Level1Card6State
        {
            get { return GetCardState(5); }
            set { SetCardState(5, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 7")]
        public CardStateValue Level1Card7State
        {
            get { return GetCardState(6); }
            set { SetCardState(6, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 8")]
        public CardStateValue Level1Card8State
        {
            get { return GetCardState(7); }
            set { SetCardState(7, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1", "Card 9")]
        public CardStateValue Level1Card9State
        {
            get { return GetCardState(8); }
            set { SetCardState(8, value); }
        }

        #endregion

        #region Level 1 - Combo
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1 - Combo", "Card 1")]
        public CardStateValue Level1Combo1State
        {
            get { return GetCardState(9); }
            set { SetCardState(9, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1 - Combo", "Card 2")]
        public CardStateValue Level1Combo2State
        {
            get { return GetCardState(10); }
            set { SetCardState(10, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1 - Combo", "Card 3")]
        public CardStateValue Level1Combo3State
        {
            get { return GetCardState(11); }
            set { SetCardState(11, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 1 - Combo", "Card 4")]
        public CardStateValue Level1Combo4State
        {
            get { return GetCardState(12); }
            set { SetCardState(12, value); }
        }

        #endregion

        #region Level 2
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 1")]
        public CardStateValue Level2Card1State
        {
            get { return GetCardState(13); }
            set { SetCardState(13, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 2")]
        public CardStateValue Level2Card2State
        {
            get { return GetCardState(14); }
            set { SetCardState(14, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 3")]
        public CardStateValue Level2Card3State
        {
            get { return GetCardState(15); }
            set { SetCardState(15, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 4")]
        public CardStateValue Level2Card4State
        {
            get { return GetCardState(16); }
            set { SetCardState(16, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 5")]
        public CardStateValue Level2Card5State
        {
            get { return GetCardState(17); }
            set { SetCardState(17, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 6")]
        public CardStateValue Level2Card6State
        {
            get { return GetCardState(18); }
            set { SetCardState(18, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 7")]
        public CardStateValue Level2Card7State
        {
            get { return GetCardState(19); }
            set { SetCardState(19, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 8")]
        public CardStateValue Level2Card8State
        {
            get { return GetCardState(20); }
            set { SetCardState(20, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2", "Card 9")]
        public CardStateValue Level2Card9State
        {
            get { return GetCardState(21); }
            set { SetCardState(21, value); }
        }

        #endregion 

        #region Level 2 - Combo
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2 - Combo", "Card 1")]
        public CardStateValue Level2Combo1State
        {
            get { return GetCardState(22); }
            set { SetCardState(22, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2 - Combo", "Card 2")]
        public CardStateValue Level2Combo2State
        {
            get { return GetCardState(23); }
            set { SetCardState(23, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2 - Combo", "Card 3")]
        public CardStateValue Level2Combo3State
        {
            get { return GetCardState(24); }
            set { SetCardState(24, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 2 - Combo", "Card 4")]
        public CardStateValue Level2Combo4State
        {
            get { return GetCardState(25); }
            set { SetCardState(25, value); }
        }

        #endregion

        #region Level 3
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Level 3", "Card")]
        public CardStateValue Level3CardState
        {
            get { return GetCardState(26); }
            set { SetCardState(26, value); }
        }

        #endregion
    }
}
