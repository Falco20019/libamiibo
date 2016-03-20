using System;

namespace LibAmiibo.Helper
{
    public static class NtagHelpers
    {
        public const int NFC3D_AMIIBO_SIZE = 520;

        public static byte[] GetInternalTag(byte[] tag)
        {
            byte[] internalBytes = new byte[NFC3D_AMIIBO_SIZE];

            // Convert format
            TagToInternal(tag, internalBytes);

            return internalBytes;
        }

        public static void TagToInternal(byte[] tag, byte[] intl)
        {
            // 0x02C - 0x1B3 Crypto buffer
            Array.Copy(tag, 0x008, intl, 0x000, 0x008);     // LockBytes + CC
            Array.Copy(tag, 0x080, intl, 0x008, 0x020);     // Data Signature (signs 0x029 - 0x208)
            Array.Copy(tag, 0x010, intl, 0x028, 0x024);     // 0x010 - 0x013 unencrypted, 0x014 begin of encrypted section
            Array.Copy(tag, 0x0A0, intl, 0x04C, 0x168);     // Encrypted data buffer
            Array.Copy(tag, 0x034, intl, 0x1B4, 0x020);     // Tag Signature (signs 0x1D4 - 0x208)
            Array.Copy(tag, 0x000, intl, 0x1D4, 0x008);     // NTAG Serial
            Array.Copy(tag, 0x054, intl, 0x1DC, 0x02C);     // Plaintext data
        }

        public static void InternalToTag(byte[] intl, byte[] tag)
        {
            Array.Copy(intl, 0x000, tag, 0x008, 0x008);
            Array.Copy(intl, 0x008, tag, 0x080, 0x020);
            Array.Copy(intl, 0x028, tag, 0x010, 0x024);
            Array.Copy(intl, 0x04C, tag, 0x0A0, 0x168);
            Array.Copy(intl, 0x1B4, tag, 0x034, 0x020);
            Array.Copy(intl, 0x1D4, tag, 0x000, 0x008);
            Array.Copy(intl, 0x1DC, tag, 0x054, 0x02C);
        }

        public static ushort UInt16FromTag(byte[] buffer, int offset)
        {
            var data = new byte[0x02];
            Array.Copy(buffer, offset, data, 0, data.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public static uint UInt32FromTag(byte[] buffer, int offset)
        {
            var data = new byte[0x04];
            Array.Copy(buffer, offset, data, 0, data.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public static ulong UInt64FromTag(byte[] buffer, int offset)
        {
            var data = new byte[0x08];
            Array.Copy(buffer, offset, data, 0, data.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }

        public static DateTime DateTimeFromTag(ushort value)
        {
            return new DateTime(2000 + value & 0x7F00, value & 0x00F0, value & 0x000F);
        }
    }
}
