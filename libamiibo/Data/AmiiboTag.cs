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

using LibAmiibo.Data.Figurine;
using LibAmiibo.Data.Settings;
using LibAmiibo.Data.Settings.AppData;
using LibAmiibo.Data.Settings.AppData.Games;
using LibAmiibo.Encryption;
using LibAmiibo.Helper;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LibAmiibo.Data
{
    public class AmiiboTag
    {
        /// <summary>
        /// The data placed at the start of a blank NTAG215.
        /// </summary>
        private static readonly byte[] tagHeader = new byte[] { 
            // Tag serial number
            0x04, 0x01, 0x02, 0x8F, 0x03, 0x04, 0x05, 0x06, 0x04, 

            // Internal value
            0x48, 

            // Lock bytes
            0x0F, 0xE0, 
            
            // Capability container
            0xF1, 0x10, 0xFF, 0xEE,

            // Amiibo magic
            0xA5, 

            // Write counter
            0x00, 0x00,

            // Figure version (always 0x00)
            0x00
        };

        /// <summary>
        /// The data placed at hex offset 0x208 of a blank NTAG215.
        /// </summary>
        private static readonly byte[] tagFooter = new byte[] { 
            // Dynamic lock bytes
            0x01, 0x00, 0x0F,
                
            // RFUI
            0xBD,
                
            // CFG0
            0x00, 0x00, 0x00, 0x04,
                
            // CFG1
            0x5F, 0x00, 0x00, 0x00
        };

        /// <summary>
        /// 
        /// This can be an encrypted tag converted with NtagHelpers.GetInternalTag() or an decrypted tag
        /// unpacked with Nfc3DAmiiboKeys.Unpack()
        /// </summary>
        public ArraySegment<byte> InternalTag { get; }

        private Amiibo amiibo;
        public Amiibo Amiibo
        {
            get { return amiibo; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                this.amiibo = value;
                var amiiboBuffer = new ArraySegment<byte>(InternalTag.Array, 0x1DC, 0x08);
                amiiboBuffer.CopyFrom(NtagHelpers.StringToByteArrayFastest(value.StatueId));
            }
        }

        public AmiiboSettings AmiiboSettings { get; }

        /// <summary>
        /// Only valid if HasAppData returns true. Use AmiiboSettings.AmiiboAppData for easier access.
        /// </summary>
        public ArraySegment<byte> AppData
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x0DC, 0xD8);
            }
            set { AppData.CopyFrom(value); }
        }
        public bool IsDecrypted { get; private set; }

        public bool HasAppData
        {
            get { return IsDecrypted && AmiiboSettings.Status.HasFlag(Status.AppDataInitialized); }
        }

        public bool HasUserData
        {
            get { return IsDecrypted && AmiiboSettings.Status.HasFlag(Status.UserDataInitialized); }
        }

        public ArraySegment<byte> LockBytesCC
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x000, 0x008);
            }
            set { LockBytesCC.CopyFrom(value); }
        }

        public ArraySegment<byte> DataSignature
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x008, 0x20);
            }
            set { DataSignature.CopyFrom(value); }
        }
        /// <summary>
        /// Signs InternalTag from 0x1D4 to 0x208.
        /// </summary>
        public ArraySegment<byte> TagSignature
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x1B4, 0x20);
            }
            set { TagSignature.CopyFrom(value); }
        }

        public ArraySegment<byte> CryptoInitSequence
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x028, 0x004);
            }
            set { CryptoInitSequence.CopyFrom(value); }
        }

        public ushort WriteCounter
        {
            get { return NtagHelpers.UInt16FromTag(InternalTag, 0x029); }
            set { NtagHelpers.UInt16ToTag(InternalTag, 0x029, value); }
        }

        public ArraySegment<byte> CryptoBuffer
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x02C, 0x188);
            }
            set { CryptoBuffer.CopyFrom(value); }
        }

        public ArraySegment<byte> NtagSerial
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x1D4, 0x008);
            }
            set { NtagSerial.CopyFrom(value); }
        }

        public byte[] UID
        {
            get
            {
                IList<byte> ntagSerial = NtagSerial;
                return new[] { ntagSerial[0], ntagSerial[1], ntagSerial[2], ntagSerial[4], ntagSerial[5], ntagSerial[6], ntagSerial[7] };
            }
            set
            {
                var bcc0 = (byte)(0x88 ^ value[0] ^ value[1] ^ value[2]);
                var bcc1 = (byte)(value[3] ^ value[4] ^ value[5] ^ value[6]);
                NtagSerial.CopyFrom(new [] {value[0], value[1], value[2], bcc0, value[3], value[4], value[5], value[6]});
                LockBytesCC.CopyFrom(new[] {bcc1});
            }
        }

        public ArraySegment<byte> PlaintextData
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x1DC, 0x02C);
            }
            set { PlaintextData.CopyFrom(value); }
        }

        public ArraySegment<byte> NtagECDSASignature
        {
            get
            {
                return new ArraySegment<byte>(InternalTag.Array, 0x208, 0x020);
            }
            set { NtagECDSASignature.CopyFrom(value); }
        }

        private AmiiboTag(ArraySegment<byte> internalTag)
        {
            this.InternalTag = internalTag;
            this.Amiibo = Amiibo.FromInternalTag(internalTag);
            this.AmiiboSettings = new AmiiboSettings(CryptoBuffer, AppData);
        }

        public static AmiiboTag FromNtagData(byte[] data)
        {
            return new AmiiboTag(new ArraySegment<byte>(NtagHelpers.GetInternalTag(data))) { IsDecrypted = false };
        }

        public static AmiiboTag FromInternalTag(ArraySegment<byte> data)
        {
            return new AmiiboTag(data) { IsDecrypted = true };
        }

        /// <summary>
        /// Creates an AmiiboTag instance from an identification block represented as a hexadecimal string.
        /// </summary>
        /// <param name="identificationBlock">The identification block as a hexadecimal string.</param>
        /// <returns>An AmiiboTag instance representing the identification block.</returns>
        public static AmiiboTag FromIdentificationBlock(string identificationBlock)
        {
            // Create a regular expression object to match a 16-character hexadecimal string..
            Regex regex = new Regex(@"^(?:0x)?([0-9A-Fa-f]{16})$");

            // Match the input identification block against the regular expression.
            Match match = regex.Match(identificationBlock);

            if (match.Success)
            {
                // The captured group at index 1 contains the hexadecimal value.
                string hexadecimalValue = match.Groups[1].Value;

                // Call the method to create an AmiiboTag from a byte array.
                return FromIdentificationBlock(ByteHelpers.StringToByteArray(hexadecimalValue));
            }
            else
            {
                // The input identification block does not match the expected format.
                throw new ArgumentException(nameof(identificationBlock));
            }
        }

        /// <summary>
        /// Creates an AmiiboTag instance from an identification block represented as a byte array.
        /// </summary>
        /// <param name="identificationBlock">The identification block as a byte array.</param>
        /// <returns>An AmiiboTag instance representing the identification block.</returns>
        public static AmiiboTag FromIdentificationBlock(byte[] identificationBlock)
        {
            // Check if the identification block byte array is null.
            if (identificationBlock == null)
            {
                throw new ArgumentNullException(nameof(identificationBlock));
            }

            // Check that the identification block byte array has a length of 8 bytes.
            if (identificationBlock.Length != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(identificationBlock));
            }

            // Check that the format version of the identification block is equal to 0x02.
            if (identificationBlock[7] != 0x02)
            {
                throw new ArgumentException(nameof(identificationBlock));
            }

            // Create a new byte array to hold decrypted tag data.
            var decryptedNtag = new byte[540];

            // Copy the tag header to the byte array.
            tagHeader.CopyTo(decryptedNtag, 0x00);

            // Copy the identification block to the tag byte array.
            identificationBlock.CopyTo(decryptedNtag, 0x54);

            // Initialize a new 32-byte array to store a random salt for the tag.
            var salt = new byte[32];

            // Initialize a random number generator.
            var rnd = new Random();

            // Fill the salt byte array with random values.
            rnd.NextBytes(salt);

            // Copy the salt to the tag byte array
            salt.CopyTo(decryptedNtag, 0x60);

            // Copy the tag footer to the tag byte array.
            tagFooter.CopyTo(decryptedNtag, 0x208);

            // Create an AmiiboTag from the decrypted data.
            var tag = AmiiboTag.FromNtagData(decryptedNtag);

            // Set the IsDecrypted property to true and randomize the UID.
            tag.IsDecrypted = true;
            tag.RandomizeUID();

            // Return the tag.
            return tag;
        }

        public static AmiiboTag DecryptWithKeys(byte[] data)
        {
            byte[] decryptedData = new byte[NtagHelpers.NFC3D_AMIIBO_SIZE];
            var amiiboKeys = Keys.AmiiboKeys;

            return amiiboKeys != null && amiiboKeys.Unpack(data, decryptedData)
                ? FromInternalTag(new ArraySegment<byte>(decryptedData))
                : FromNtagData(data);
        }

        public byte[] EncryptWithKeys()
        {
            byte[] encryptedData = new byte[NtagHelpers.NFC3D_NTAG_SIZE];
            var amiiboKeys = Keys.AmiiboKeys;

            if (amiiboKeys == null)
                return null;

            amiiboKeys.Pack(this.InternalTag.Array, encryptedData);
            return encryptedData;
        }

        public void RandomizeUID()
        {
            var rand = new Random();
            var bytes = new byte[7];
            rand.NextBytes(bytes);
            bytes[0] = 0x04; // NXP manufacturer code
            if (bytes[4] == 0x88)
                bytes[4]++; // This must not be 0x88. Just increasing one to avoid the case when another random would result in 0x88
            UID = bytes;
        }

        public bool IsUidValid()
        {
            var uid = UID;
            var bcc0In = NtagSerial.Skip(3).First();
            var bcc1In = LockBytesCC.First();
            var bcc0Out = (byte)(0x88 ^ uid[0] ^ uid[1] ^ uid[2]);
            var bcc1Out = (byte)(uid[3] ^ uid[4] ^ uid[5] ^ uid[6]);
            return bcc0In == bcc0Out && bcc1In == bcc1Out & uid[0] == 0x04 && uid[4] != 0x88;
        }

        public bool IsNtagECDSASignatureValid()
        {
            var uid = UID;
            if (NtagECDSASignature.Count != 0x20 || uid.Length != 7)
                return false;

            var r = new byte[NtagECDSASignature.Count / 2];
            var s = new byte[NtagECDSASignature.Count / 2];
            Array.Copy(NtagECDSASignature.Array, NtagECDSASignature.Offset, r, 0, r.Length);
            Array.Copy(NtagECDSASignature.Array, NtagECDSASignature.Offset + r.Length, s, 0, s.Length);

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

        public void InitializeAppData<T>() where T: IAppDataInitializer, new()
        {
            var factory = new T();
            var settings = this.AmiiboSettings;
            var appData = settings.AmiiboAppData;

            appData.AppDataInitializationTitleID = factory.GetInitializationTitleIDs().First();
            appData.AppID = factory.GetAppID() ?? throw new InvalidOperationException("The AppID was not found");
            settings.Status |= Status.AppDataInitialized;
            settings.AmiiboLastModifiedDate = DateTime.UtcNow.Date;
            settings.WriteCounter++;

            factory.InitializeAppData(this);
            this.WriteCounter++;
        }
    }
}
