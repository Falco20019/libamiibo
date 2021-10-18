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
using LibAmiibo.Attributes;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x0016E100)]
    [AppDataInitializationTitleID("00040000001BC600")]
    [AppDataInitializationTitleID("000400000016E100")]
    [AppDataInitializationTitleID("00040000001BC500")]
    public class MonsterHunterStories : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        public ArraySegment<byte> PublisherTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x00, 0x14); }
            set { PublisherTag.CopyFrom(value); }
        }
        
        public MonsterHunterStories(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(MonsterHunterStories))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new MonsterHunterStories(tag.AppData);
                game.PublisherTag.CopyFrom(Encoding.ASCII.GetBytes("155260401735857398289"));
            }
        }
    }
}
