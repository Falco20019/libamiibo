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
using System.IO;
using LibAmiibo.Data.Figurine;
using LibAmiibo.Data.Settings;
using LibAmiibo.Encryption;
using LibAmiibo.Helper;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;

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
                Array.Copy(InternalTag, 0x0DC, data, 0, data.Length);
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
                var data = new byte[0x008];
                Array.Copy(InternalTag, 0x1D4, data, 0, data.Length);
                return data;
            }
        }

        public string UID
        {
            get
            {
                byte[] ntagSerial = this.NtagSerial;
                return String.Format("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2}", ntagSerial[0], ntagSerial[1], ntagSerial[2], ntagSerial[4], ntagSerial[5], ntagSerial[6], ntagSerial[7]);
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

        public byte[] NtagECDSASignature
        {
            get
            {
                var data = new byte[0x020];
                Array.Copy(InternalTag, 0x208, data, 0, data.Length);
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

        public byte[] EncryptWithKeys()
        {
            byte[] encryptedData = new byte[NtagHelpers.NFC3D_NTAG_SIZE];
            var amiiboKeys = Files.AmiiboKeys;

            if (amiiboKeys == null)
                return null;

            amiiboKeys.Pack(this.InternalTag, encryptedData);
            return encryptedData;
        }

        public bool IsNtagECDSASignatureValid()
        {
            if (NtagECDSASignature.Length != 0x20 || NtagSerial.Length != 8)
                return false;

            var r = new byte[NtagECDSASignature.Length/2];
            var s = new byte[NtagECDSASignature.Length/2];
            Array.Copy(NtagECDSASignature, 0, r, 0, r.Length);
            Array.Copy(NtagECDSASignature, r.Length, s, 0, s.Length);

            var uid = new byte[7];
            Array.Copy(NtagSerial, 0, uid, 0, 3);
            Array.Copy(NtagSerial, 4, uid, 3, 4);

            var curve = SecNamedCurves.GetByName("secp128r1");
            var curveSpec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            var key = new ECPublicKeyParameters("ECDSA", curve.Curve.DecodePoint(NtagHelpers.NTAG_PUB_KEY), curveSpec);

            var signer = SignerUtilities.GetSigner("NONEwithECDSA");

            signer.Init(false, key);

            signer.BlockUpdate(uid, 0, uid.Length);

            using (var ms = new MemoryStream())
            using (var der = new Asn1OutputStream(ms))
            {
                var v = new Asn1EncodableVector
                {
                    new DerInteger(new BigInteger(1, r)),
                    new DerInteger(new BigInteger(1, s))
                };
                der.WriteObject(new DerSequence(v));

                return signer.VerifySignature(ms.ToArray());
            }
        }
    }
}
