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
    [AppID(0x00152600)]
    [AppDataInitializationTitleID("0004000000162F00")]
    [AppDataInitializationTitleID("0004000000152600")]
    [AppDataInitializationTitleID("0004000000163000")]
    public class ChibiRoboZipLash : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        private static readonly uint[] ScoreToLevel = new uint[] {
            0x03A980,
            0x0927C0,
            0x107AC0,
            0x1B7740,
            0x2A1D40
        };

        public uint Constant
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x00, true); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x00, value, true); }
        }

        public uint Score
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x04); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x04, value); }
        }

        public uint LastUploadedScore
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x08); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x08, value); }
        }

        public ArraySegment<byte> OwnerTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x0C, 0x04); }
            set { OwnerTag.CopyFrom(value); }
        }

        public ChibiRoboZipLash(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(ChibiRoboZipLash))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new ChibiRoboZipLash(tag.AppData);
                game.AppData.CopyFrom(new byte[0x0C]); // TODO: Use for-loop and create extension method
                game.Constant = 3;
                // TODO: how is OwnerTag calculated? Based on settings-CRC32?
                game.OwnerTag.CopyFrom(new byte[] {0x9e, 0x3b, 0x9d, 0xbf});
            }
        }

        #region General

        [Cheat(CheatAttribute.Type.NumberSpinner, "General", "Level", Description = "Changes the level of your Chibi", Min = 0, Max = 6)]
        public uint Level
        {
            get
            {
                var score = Score;
                byte level = 1;
                foreach (var scoreThreshold in ScoreToLevel)
                {
                    if (score < scoreThreshold)
                        break;
                    level++;
                }
                return level;
            }
            set
            {
                if (value <= 1)
                {
                    Score = 0;
                    return;
                }

                if (value > 6)
                    value = 6;

                Score = ScoreToLevel[value - 2];
            }
        }

        #endregion
    }
}
