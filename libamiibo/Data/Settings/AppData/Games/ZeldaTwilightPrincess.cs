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
    [AppID(0x1019C800)]
    [AppDataInitializationTitleID("000500001019E600")]
    [AppDataInitializationTitleID("000500001019C800")]
    [AppDataInitializationTitleID("000500001019E500")]
    public class ZeldaTwilightPrincess : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        public ArraySegment<byte> OwnerTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x00, 0x05); }
            set { OwnerTag.CopyFrom(value); }
        }

        public ArraySegment<byte> ConsoleTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x05, 0x0A); }
            set { ConsoleTag.CopyFrom(value); }
        }
        
        public ZeldaTwilightPrincess(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(ZeldaTwilightPrincess))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new ZeldaTwilightPrincess(tag.AppData);
                game.AppData.CopyFrom(new byte[0x34]); // TODO: Use for-loop and create extension method
                // TODO: how is OwnerTag calculated? Based on settings-CRC32?
                // TODO: how is ConsoleTag calculated? Based on settings-CRC32?
                game.QuickStartSlotNr = 0xFF;
            }
        }

        #region General
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "General", "Quickstart Slot")]
        public byte QuickStartSlotNr
        {
            get { return AppData.Array[AppData.Offset + 0x10]; }
            set { AppData.Array[AppData.Offset + 0x10] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "General", "Cave level reached")]
        public byte CaveLevelReached
        {
            get { return AppData.Array[AppData.Offset + 0x11]; }
            set { AppData.Array[AppData.Offset + 0x11] = value; }
        }

        #endregion

        #region Hearts

        /// <summary>
        /// 4 per heart
        /// </summary>
        [Cheat(CheatAttribute.Type.NumberSpinner, "Hearts", "Saved heart quarters", Description = "4 per heart")]
        public byte SavedHeartQuarters
        {
            get { return AppData.Array[AppData.Offset + 0x12]; }
            set { AppData.Array[AppData.Offset + 0x12] = value; }
        }

        /// <summary>
        /// 5 per heart
        /// </summary>
        [Cheat(CheatAttribute.Type.NumberSpinner, "Hearts", "Total heart pieces", Description = "5 per heart")]
        public byte TotalHeartPieces
        {
            get { return AppData.Array[AppData.Offset + 0x13]; }
            set { AppData.Array[AppData.Offset + 0x13] = value; }
        }

        #endregion
    }
}
