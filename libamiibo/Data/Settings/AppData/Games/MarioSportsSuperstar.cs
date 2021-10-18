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
    [AppID(0x00188B00)]
    [AppDataInitializationTitleID("0004000000188D00")]
    [AppDataInitializationTitleID("0004000000188B00")]
    [AppDataInitializationTitleID("0004000000188C00")]
    public class MarioSportsSuperstar : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        public bool IsSuperstar
        {
            get { return AppData.Array[AppData.Offset] == 0x01; }
            set { AppData.Array[AppData.Offset] = (byte)(value ? 0x01 : 0x00); }
        }
        
        public MarioSportsSuperstar(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(MarioSportsSuperstar))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new MarioSportsSuperstar(tag.AppData);
                game.AppData.CopyFrom(new byte[0xD8]); // TODO: Use for-loop and create extension method
                game.IsSuperstar = true;
            }
        }
    }
}
