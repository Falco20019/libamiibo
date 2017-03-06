﻿using System;

namespace LibAmiibo.Helper
{
    public static class NtagHelpers
    {
        public const int NFC3D_AMIIBO_SIZE = 552;
        public const int NFC3D_NTAG_SIZE = 572;

        public static readonly byte[] CONFIG_BYTES =
        {
            0x01, 0x00, 0x0F, 0xBD,     // Dynamic lock bytes
            0x00, 0x00, 0x00, 0x04,     // CFG0
            0x5F, 0x00, 0x00, 0x00      // CFG1
        };
        public static readonly byte[] NTAG_PUB_KEY =
        {
            0x04, 0x49, 0x4E, 0x1A,
            0x38, 0x6D, 0x3D, 0x3C,
            0xFE, 0x3D, 0xC1, 0x0E,
            0x5D, 0xE6, 0x8A, 0x49,
            0x9B, 0x1C, 0x20, 0x2D,
            0xB5, 0xB1, 0x32, 0x39,
            0x3E, 0x89, 0xED, 0x19,
            0xFE, 0x5B, 0xE8, 0xBC,
            0x61
        };

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

            // ECDSA of tag:
            if (tag.Length == NFC3D_NTAG_SIZE)
            {
                Array.Copy(tag, 0x21C, intl, 0x208, 0x020);
            }
            else
            {
                for (int i = 0x208; i < 0x208 + 0x20; i++)
                    intl[i] = 0xFF;
            }
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
            Array.Copy(intl, 0x208, tag, 0x21C, 0x020);
            Array.Copy(CONFIG_BYTES, 0x000, tag, 0x208, 0x00C);
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
            var day = value & 0x1F;
            var month = (value >> 5) & 0x0F;
            var year = (value >> 9) & 0x7F;
            return new DateTime(2000 + year, month, day);
        }
    }
}
