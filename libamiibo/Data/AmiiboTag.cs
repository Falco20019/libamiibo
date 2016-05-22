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
using LibAmiibo.Data.Figurine;
using LibAmiibo.Data.Settings;
using LibAmiibo.Encryption;
using LibAmiibo.Helper;

namespace LibAmiibo.Data
{
    public class AmiiboTag
    {
        /// <summary>
        /// This can be an encrypted tag converted with NtagHelpers.GetInternalTag() or an decrypted tag
        /// unpacked with Nfc3DAmiiboKeys.Unpack()
        /// </summary>
        public byte[] InternalTag { get; }
        public Amiibo Amiibo { get; }
        public AmiiboSettings AmiiboSettings { get; }
        /// <summary>
        /// Only valid if HasAppData returns true.
        /// </summary>
        public byte[] AppData
        {
            get
            {
                var data = new byte[0xD8];
                Array.Copy(InternalTag, 0x0B0, data, 0, data.Length);
                return data;
            }
        }
        public bool IsDecrypted { get; private set; }

        public bool HasAppData
        {
            get { return IsDecrypted && AmiiboSettings.Status.HasFlag(Status.AppDataInitialized); }
        }

        public byte[] DataSignature
        {
            get
            {
                var signature = new byte[0x20];
                Array.Copy(InternalTag, 0x008, signature, 0, signature.Length);
                return signature;
            }
        }
        /// <summary>
        /// Signs InternalTag from 0x1D4 to 0x208.
        /// </summary>
        public byte[] TagSignature
        {
            get
            {
                var signature = new byte[0x020];
                Array.Copy(InternalTag, 0x1B4, signature, 0, signature.Length);
                return signature;
            }
        }

        public byte[] CryptoInitSequence
        {
            get
            {
                var data = new byte[0x002];
                Array.Copy(InternalTag, 0x028, data, 0, data.Length);
                return data;
            }
        }

        /// <summary>
        /// Warning: Not sure if this is correct, maybe should be starting with 0x028 and is little-endian?
        /// </summary>
        public ushort WriteCounter
        {
            get { return NtagHelpers.UInt16FromTag(InternalTag, 0x029); }
        }

        public byte[] CryptoBuffer
        {
            get
            {
                var data = new byte[0x188];
                Array.Copy(InternalTag, 0x02C, data, 0, data.Length);
                return data;
            }
        }

        public byte[] NtagSerial
        {
            get
            {
                var data = new byte[0x004];
                Array.Copy(InternalTag, 0x1D4, data, 0, data.Length);
                return data;
            }
        }

        public byte[] PlaintextData
        {
            get
            {
                var data = new byte[0x02C];
                Array.Copy(InternalTag, 0x1DC, data, 0, data.Length);
                return data;
            }
        }

        private AmiiboTag(byte[] internalTag)
        {
            this.InternalTag = internalTag;
            this.Amiibo = Amiibo.FromInternalTag(internalTag);
            this.AmiiboSettings = AmiiboSettings.FromCryptoBuffer(CryptoBuffer);
        }

        public static AmiiboTag FromNtagData(byte[] data)
        {
            return new AmiiboTag(NtagHelpers.GetInternalTag(data)) { IsDecrypted = false };
        }

        public static AmiiboTag FromInternalTag(byte[] data)
        {
            return new AmiiboTag(data) { IsDecrypted = true };
        }

        public static AmiiboTag DecryptWithKeys(byte[] data)
        {
            byte[] decryptedData = new byte[NtagHelpers.NFC3D_AMIIBO_SIZE];
            var amiiboKeys = Files.AmiiboKeys;

            return amiiboKeys != null && amiiboKeys.Unpack(data, decryptedData) ? FromInternalTag(decryptedData) : FromNtagData(data);
        }
    }
}
