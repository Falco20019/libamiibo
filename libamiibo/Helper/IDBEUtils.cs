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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LibAmiibo.Helper
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ICNLocalizedDescription
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] FirstTitle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] SecondTitle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] Publisher;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IDBEApplicationSettings
    {
        public ulong TitleID;
        public ulong Version;
        public Region Region;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] RegionSpecificGameRatings;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x0C)]
        public byte[] MatchMarkerIDs;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IDBEHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ShaBytes;
        public IDBEApplicationSettings ApplicationSettings;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public ICNLocalizedDescription[] Descriptions;
    }

    public enum Localization
    {
        Japanese = 0,
        English,
        French,
        German,
        Italian,
        Spanish,
        Chinese,
        Korean,
        Dutch,
        Portuguese,
        Russian
    }

    [Flags]
    public enum Region
    {
        Japan = 0x01,
        NorthAmerica = 0x02,
        Europe = 0x04,
        Australia = 0x08,
        China = 0x10,
        Korea = 0x20,
        Taiwan = 0x40,
        AllRegions = int.MaxValue
    }

    public abstract class IDBEContext
    {
        public IDBEHeader Header;
        public abstract string FirstTitle(Localization localization);
        public abstract string SecondTitle(Localization localization);
        public abstract string Publisher(Localization localization);
    }

    public class IDBE3DSContext : IDBEContext
    {
        public Bitmap SmallIcon, LargeIcon;

        public override string FirstTitle(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.Unicode.GetString(this.Header.Descriptions[(int)localization].FirstTitle));
        }
        public override string SecondTitle(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.Unicode.GetString(this.Header.Descriptions[(int)localization].SecondTitle));
        }
        public override string Publisher(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.Unicode.GetString(this.Header.Descriptions[(int)localization].Publisher));
        }

        public bool Open(Stream fs)
        {
            Header = MarshalUtil.ReadStruct<IDBEHeader>(fs);
            SmallIcon = ImageUtil.ReadImageFromStream(fs, 24, 24, ImageUtil.PixelFormat.RGB565);
            LargeIcon = ImageUtil.ReadImageFromStream(fs, 48, 48, ImageUtil.PixelFormat.RGB565);
            return true;
        }
    }

    public class IDBEWiiUContext : IDBEContext
    {
        public byte[] Image;

        public override string FirstTitle(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.BigEndianUnicode.GetString(this.Header.Descriptions[(int)localization].FirstTitle));
        }
        public override string SecondTitle(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.BigEndianUnicode.GetString(this.Header.Descriptions[(int)localization].SecondTitle));
        }
        public override string Publisher(Localization localization)
        {
            return MarshalUtil.CleanInput(Encoding.BigEndianUnicode.GetString(this.Header.Descriptions[(int)localization].Publisher));
        }

        public bool Open(Stream fs)
        {
            Header = MarshalUtil.ReadStructBE<IDBEHeader>(fs);
            Image = new byte[fs.Length - fs.Position];
            fs.Read(Image, 0, Image.Length);
            return true;
        }
    }
}
