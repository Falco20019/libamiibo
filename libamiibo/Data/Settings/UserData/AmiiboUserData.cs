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
using System.Text;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings.UserData
{
    public class AmiiboUserData
    {
        public ArraySegment<byte> CryptoBuffer { get; private set; }

        private IList<byte> CryptoBufferList => CryptoBuffer;

        public byte[] GetAmiiboSettingsBytes
        {
            get
            {
                return new[]
                {
                    (byte) (CryptoBufferList[0] & 0x0F),
                    CryptoBufferList[1]
                };
            }
            set
            {
                var tmp = (int)CryptoBufferList[0];
                tmp &= ~0x0F;
                tmp |= value[0] & 0x0F;
                CryptoBufferList[0] = (byte)tmp;
                CryptoBufferList[1] = value[1];
            }
        }

        // TODO: Add Country Code from 0x01

        public ushort AmiiboSetupDateValue
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x04); }
            set { NtagHelpers.UInt16ToTag(CryptoBuffer, 0x04, value); }
        }

        public DateTime AmiiboSetupDate
        {
            get { return NtagHelpers.DateTimeFromTag(AmiiboSetupDateValue); }
            set { AmiiboSetupDateValue = NtagHelpers.DateTimeToTag(value); }
        }

        public string AmiiboNickname
        {
            get { return MarshalUtil.CleanInput(Encoding.BigEndianUnicode.GetString(CryptoBuffer.Array, CryptoBuffer.Offset + 0x0C, 0x14)); }
            set { AmiiboNicknameBuffer.CopyFrom(Encoding.BigEndianUnicode.GetBytes(MarshalUtil.CleanOutput(value))); }
        }

        private ArraySegment<byte> AmiiboNicknameBuffer
        {
            get { return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x0C, 0x14); }
            set { AmiiboNicknameBuffer.CopyFrom(value); }
        }

        public ArraySegment<byte> OwnerMii
        {
            get { return new ArraySegment<byte>(CryptoBuffer.Array, CryptoBuffer.Offset + 0x20, 0x60); }
            set { OwnerMii.CopyFrom(value); }
        }

        public AmiiboUserData(ArraySegment<byte> cryptoData)
        {
            this.CryptoBuffer = cryptoData;
        }
    }
}
