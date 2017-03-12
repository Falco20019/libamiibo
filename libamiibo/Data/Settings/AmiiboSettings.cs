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
using LibAmiibo.Data.Settings.AppData;
using LibAmiibo.Data.Settings.UserData;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings
{
    public class AmiiboSettings
    {
        public ArraySegment<byte> CryptoBuffer { get; private set; }
        public AmiiboUserData AmiiboUserData { get; private set; }
        public AmiiboAppData AmiiboAppData { get; private set; }

        private IList<byte> CryptoBufferList
        {
            get { return CryptoBuffer as IList<byte>; }
        }

        public Status Status
        {
            get { return (Status) (CryptoBufferList[0] & 0x30); }
            set
            {
                var tmp = (int)CryptoBufferList[0];
                tmp &= ~0x30;
                tmp |= (int)value & 0x30;
                CryptoBufferList[0] = (byte)tmp;
            }
        }

        public ushort CrcUpdateCounter
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x02); }
            set { NtagHelpers.UInt16ToTag(CryptoBuffer, 0x02, value); }
        }

        public ushort AmiiboLastModifiedDateValue
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x06); }
            set { NtagHelpers.UInt16ToTag(CryptoBuffer, 0x06, value); }
        }

        public DateTime AmiiboLastModifiedDate
        {
            get { return NtagHelpers.DateTimeFromTag(AmiiboLastModifiedDateValue); }
            set { AmiiboLastModifiedDateValue = NtagHelpers.DateTimeToTag(value); }
        }

        // TODO: This is the unique console hash
        public uint CRC32
        {
            get { return NtagHelpers.UInt32FromTag(CryptoBuffer, 0x08); }
            set { NtagHelpers.UInt32ToTag(CryptoBuffer, 0x08, value); }
        }

        public ushort WriteCounter
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x88); }
            set { NtagHelpers.UInt16ToTag(CryptoBuffer, 0x88, value); }
        }

        public ArraySegment<byte> Unknown8EBytes
        {
            get { return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x8E, 0x02); }
            set { Unknown8EBytes.CopyFrom(value); }
        }

        public ArraySegment<byte> Signature
        {
            get { return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x90, 0x20); }
            set { Signature.CopyFrom(value); }
        }

        public AmiiboSettings(ArraySegment<byte> cryptoData, ArraySegment<byte> appData)
        {
            this.CryptoBuffer = cryptoData;
            this.AmiiboUserData = new AmiiboUserData(cryptoData);
            this.AmiiboAppData = new AmiiboAppData(cryptoData, appData);
        }
    }
}
