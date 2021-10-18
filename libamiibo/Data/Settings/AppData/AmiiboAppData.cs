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
using System.Collections.Generic;
using LibAmiibo.Data.Settings.AppData.Games;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings.AppData
{
    public class AmiiboAppData
    {
        public ArraySegment<byte> CryptoBuffer { get; private set; }
        public ArraySegment<byte> AppData { get; private set; }

        private IList<byte> CryptoBufferList => CryptoBuffer;

        private ArraySegment<byte> AppDataInitializationTitleIDBuffer
        {
            get { return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x80, 0x08); }
            set { AppDataInitializationTitleIDBuffer.CopyFrom(value); }
        }

        public Title AppDataInitializationTitleID
        {
            get { return Title.FromTitleID(AppDataInitializationTitleIDBuffer); }
            set { AppDataInitializationTitleIDBuffer.CopyFrom(value.Data); }
        }

        public uint AppID
        {
            get { return NtagHelpers.UInt32FromTag(CryptoBuffer, 0x8A); }
            set { NtagHelpers.UInt32ToTag(CryptoBuffer, 0x8A, value); }
        }

        /// <summary>
        /// To set the game, use AmiiboTag.InitializeAppData()!
        /// </summary>
        public IGame Game => AppDataUtil.GetGameForAmiiboAppData(this);

        public AmiiboAppData(ArraySegment<byte> cryptoData, ArraySegment<byte> appData)
        {
            this.CryptoBuffer = cryptoData;
            this.AppData = appData;
        }
    }
}
