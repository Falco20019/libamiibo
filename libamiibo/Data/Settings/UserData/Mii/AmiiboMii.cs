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

namespace LibAmiibo.Data.Settings.UserData.Mii
{
    public class AmiiboMii
    {
        public ArraySegment<byte> MiiBuffer { get; private set; }

        private IList<byte> MiiBufferList => MiiBuffer;

        public uint MiiID
        {
            get { return NtagHelpers.UInt32FromTag(MiiBuffer, 0x00); }
            set { NtagHelpers.UInt32ToTag(MiiBuffer, 0x00, value); }
        }

        public ulong SystemID
        {
            get { return NtagHelpers.UInt64FromTag(MiiBuffer, 0x04); }
            set { NtagHelpers.UInt64ToTag(MiiBuffer, 0x04, value); }
        }

        public uint SpecialnessAndDateOfCreation
        {
            get { return NtagHelpers.UInt32FromTag(MiiBuffer, 0x0C); }
            set { NtagHelpers.UInt32ToTag(MiiBuffer, 0x0C, value); }
        }

        private ArraySegment<byte> CreatorsMAC
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x10, 0x06); }
            set { CreatorsMAC.CopyFrom(value); }
        }

        public ushort BirthdaySexShirtFavorite
        {
            get { return NtagHelpers.UInt16FromTag(MiiBuffer, 0x18); }
            set { NtagHelpers.UInt16ToTag(MiiBuffer, 0x18, value); }
        }

        public string MiiNickname
        {
            get { return MarshalUtil.CleanInput(Encoding.Unicode.GetString(MiiBuffer.Array, MiiBuffer.Offset + 0x1A, 0x14)); }
            set { MiiNicknameBuffer.CopyFrom(Encoding.Unicode.GetBytes(MarshalUtil.CleanOutput(value))); }
        }

        private ArraySegment<byte> MiiNicknameBuffer
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x1A, 0x14); }
            set { MiiNicknameBuffer.CopyFrom(value); }
        }

        public ushort WidthAndHeight
        {
            get { return NtagHelpers.UInt16FromTag(MiiBuffer, 0x2E); }
            set { NtagHelpers.UInt16ToTag(MiiBuffer, 0x2E, value); }
        }

        public byte SharingFaceshapeSkincolor
        {
            get { return MiiBufferList[0x30]; }
            set { MiiBufferList[0x30] = value; }
        }

        public byte WrinklesMakeup
        {
            get { return MiiBufferList[0x31]; }
            set { MiiBufferList[0x31] = value; }
        }

        public byte Hairstyle
        {
            get { return MiiBufferList[0x32]; }
            set { MiiBufferList[0x32] = value; }
        }

        public byte HaircolorFliphair
        {
            get { return MiiBufferList[0x33]; }
            set { MiiBufferList[0x33] = value; }
        }

        public ArraySegment<byte> Unknown34Bytes
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x34, 0x04); }
            set { Unknown34Bytes.CopyFrom(value); }
        }

        public byte EyebrowStyleAndColor
        {
            get { return MiiBufferList[0x38]; }
            set { MiiBufferList[0x38] = value; }
        }

        public byte EyebrowScale
        {
            get { return MiiBufferList[0x39]; }
            set { MiiBufferList[0x39] = value; }
        }

        public byte EyebrowRotationAndXSpacing
        {
            get { return MiiBufferList[0x3A]; }
            set { MiiBufferList[0x3A] = value; }
        }

        public byte EyebrowYPosition
        {
            get { return MiiBufferList[0x3B]; }
            set { MiiBufferList[0x3B] = value; }
        }

        public ArraySegment<byte> Unknown3CBytes
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x3C, 0x04); }
            set { Unknown3CBytes.CopyFrom(value); }
        }

        public byte AllowCopying
        {
            get { return MiiBufferList[0x40]; }
            set { MiiBufferList[0x40] = value; }
        }

        public ArraySegment<byte> Unknown41Bytes
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x41, 0x07); }
            set { Unknown41Bytes.CopyFrom(value); }
        }

        public string AuthorNickname
        {
            get { return MarshalUtil.CleanInput(Encoding.Unicode.GetString(MiiBuffer.Array, MiiBuffer.Offset + 0x48, 0x14)); }
            set { AuthorNicknameBuffer.CopyFrom(Encoding.Unicode.GetBytes(MarshalUtil.CleanOutput(value))); }
        }

        private ArraySegment<byte> AuthorNicknameBuffer
        {
            get { return new ArraySegment<byte>(MiiBuffer.Array, MiiBuffer.Offset + 0x48, 0x14); }
            set { AuthorNicknameBuffer.CopyFrom(value); }
        }

        public AmiiboMii(ArraySegment<byte> miiData)
        {
            this.MiiBuffer = miiData;
        }
    }
}
