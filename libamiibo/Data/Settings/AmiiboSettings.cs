using System;
using System.Text;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings
{
    public class AmiiboSettings
    {
        public byte[] CryptoBuffer { get; private set; }
        public Status Status
        {
            get { return (Status) (CryptoBuffer[0] & 0x18); }
        }

        public byte[] GetAmiiboSettingsBytes
        {
            get
            {
                return new[]
                {
                    (byte) (CryptoBuffer[0] & 0x0F),
                    (byte) (CryptoBuffer[1] & 0x0F) 
                };
            }
        }

        public ushort CrcUpdateCounter
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x02); }
        }

        public ushort AmiiboSetupDateValue
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x04); }
        }

        public DateTime AmiiboSetupDate
        {
            get
            {
                return NtagHelpers.DateTimeFromTag(AmiiboSetupDateValue);
            }
        }

        public ushort AmiiboLastModifiedDateValue
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x06); }
        }

        public DateTime AmiiboLastModifiedDate
        {
            get
            {
                return NtagHelpers.DateTimeFromTag(AmiiboLastModifiedDateValue);
            }
        }

        public uint CRC32
        {
            get { return NtagHelpers.UInt32FromTag(CryptoBuffer, 0x08); }
        }

        public string AmiiboNickname
        {
            get { return Encoding.BigEndianUnicode.GetString(CryptoBuffer, 0x0C, 0x14); }
        }

        public byte[] OwnerMii
        {
            get
            {
                var data = new byte[0x60];
                Array.Copy(CryptoBuffer, 0x20, data, 0, data.Length);
                return data;
            }
        }

        public ulong AppDataInitializationTitleID
        {
            get { return NtagHelpers.UInt64FromTag(CryptoBuffer, 0x80); }
        }

        public ushort WriteCounter
        {
            get { return NtagHelpers.UInt16FromTag(CryptoBuffer, 0x88); }
        }

        public uint AppID
        {
            get { return NtagHelpers.UInt32FromTag(CryptoBuffer, 0x8A); }
        }

        public byte[] Unknown8EBytes
        {
            get
            {
                var data = new byte[0x02];
                Array.Copy(CryptoBuffer, 0x8E, data, 0, data.Length);
                return data;
            }
        }
        public byte[] Signature
        {
            get
            {
                var signature = new byte[0x20];
                Array.Copy(CryptoBuffer, 0x90, signature, 0, signature.Length);
                return signature;
            }
        }

        private AmiiboSettings(byte[] data)
        {
            this.CryptoBuffer = data;
        }

        public static AmiiboSettings FromCryptoBuffer(byte[] data)
        {
            return new AmiiboSettings(data);
        }
    }
}
