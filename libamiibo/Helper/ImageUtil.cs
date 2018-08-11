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
 
using System.Drawing;
using System.IO;
using Image = StbSharp.Image;

namespace LibAmiibo.Helper
{
    //TODO: Add CsOpenGL: csgl.dll
    //Tutorial: http://www.developerfusion.com/article/3930/opengl-and-c-part-1/

    public static class ImageUtil
    {
        //Decode RGB5A4 Taken from the dolphin project
        private static readonly int[] Convert5To8 = { 0x00,0x08,0x10,0x18,0x20,0x29,0x31,0x39,
                                                0x41,0x4A,0x52,0x5A,0x62,0x6A,0x73,0x7B,
                                                0x83,0x8B,0x94,0x9C,0xA4,0xAC,0xB4,0xBD,
                                                0xC5,0xCD,0xD5,0xDE,0xE6,0xEE,0xF6,0xFF };
        //Convert4To8 is just multiplication by 0x11
        //private static readonly int[] Convert3To8 = { 0x00, 0x24, 0x48, 0x6D, 0x91, 0xB6, 0xDA, 0xFF };

        private static readonly byte[] TempBytes = new byte[4];
        public enum PixelFormat
        {                   //System.Drawing.Imaging.PixelFormat equivalent
            RGBA8 = 0,      //Format32bppArgb   (-should be Rgba)           rrrrrrrr gggggggg bbbbbbbb aaaaaaaa
            RGB8 = 1,       //Format24bppRgb                                rrrrrrrr gggggggg bbbbbbbb
            RGBA5551 = 2,   //Format16bppArgb1555 (-should be Rgba5551)     rrrrrggg ggbbbbba
            RGB565 = 3,     //Format16bppRgb565                             rrrrrggg gggbbbbb
            RGBA4 = 4,      //                                              rrrrgggg bbbbaaaa                   
            LA8 = 5,        //                                              llllllll aaaaaaaa
            HILO8 = 6,
            L8 = 7,         //                                              llllllll
            A8 = 8,         //                                              aaaaaaaa
            LA4 = 9,        //                                              llllaaaa
            L4 = 10,        //                                              llll
            ETC1 = 11,      //Ericsson Texture Compression //http://www.khronos.org/registry/gles/extensions/OES/OES_compressed_ETC1_RGB8_texture.txt
            ETC1A4 = 12     //Ericsson Texture Compression
        }

        private static byte GetLuminance(byte red, byte green, byte blue)
        {
            // Luma (Y’) = 0.299 R’ + 0.587 G’ + 0.114 B’ from wikipedia
            return (byte)(((0x4CB2*red + 0x9691*green + 0x1D3E*blue) >> 16) & 0xFF);
        }

        private static int PixelFormatBytes(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    return 4;
                case PixelFormat.RGB8:
                    return 3;
                case PixelFormat.RGBA5551:
                case PixelFormat.RGB565:
                case PixelFormat.LA4:
                case PixelFormat.LA8:
                case PixelFormat.ETC1:
                case PixelFormat.ETC1A4:
                    return 2;
                case PixelFormat.HILO8:
                case PixelFormat.L8:
                case PixelFormat.A8:
                case PixelFormat.L4:
                    return 1;
                default:
                    return 0;
            }
        }

        private static Color DecodeColor(int val, PixelFormat pixelFormat)
        {
            int alpha = 0xFF, red, green, blue;
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    red = (val >> 24) & 0xFF;
                    green = (val >> 16) & 0xFF;
                    blue = (val >> 8) & 0xFF;
                    alpha = val & 0xFF;
                    return Color.FromArgb(alpha, red, green, blue);
                case PixelFormat.RGB8:
                    red = (val >> 16) & 0xFF;
                    green = (val >> 8) & 0xFF;
                    blue = val & 0xFF;
                    return Color.FromArgb(alpha, red, green, blue);
                case PixelFormat.RGBA5551:
                    red = Convert5To8[(val >> 11) & 0x1F];
                    green = Convert5To8[(val >> 6) & 0x1F];
                    blue = Convert5To8[(val >> 1) & 0x1F];
                    alpha = (val & 0x0001) == 1 ? 0xFF : 0x00;
                    return Color.FromArgb(alpha, red, green, blue);
                case PixelFormat.RGB565:
                    red = Convert5To8[(val >> 11) & 0x1F];
                    green = ((val >> 5) & 0x3F) * 4;
                    blue = Convert5To8[val & 0x1F];
                    return Color.FromArgb(alpha, red, green, blue);
                case PixelFormat.RGBA4:
                    alpha = 0x11 * (val & 0xf);
                    red = 0x11 * ((val >> 12) & 0xf);
                    green = 0x11 * ((val >> 8) & 0xf);
                    blue = 0x11 * ((val >> 4) & 0xf);
                    return Color.FromArgb(alpha, red, green, blue);
                case PixelFormat.LA8:
                    red = val >> 8;
                    alpha = val & 0xFF;
                    return Color.FromArgb(alpha, red, red, red);
                case PixelFormat.HILO8: //use only the HI
                    red = val >> 8;
                    return Color.FromArgb(alpha, red, red, red);
                case PixelFormat.L8:
                    return Color.FromArgb(alpha, val, val, val);
                case PixelFormat.A8:
                    return Color.FromArgb(val, alpha, alpha, alpha);
                case PixelFormat.LA4:
                    red = val >> 4;
                    alpha = val & 0x0F;
                    return Color.FromArgb(alpha, red, red, red);
                default:
                    return Color.White;
            }
        }

        private static void EncodeColor(Color color, PixelFormat pixelFormat, byte[] bytes)
        {
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    bytes[0] = color.A;
                    bytes[1] = color.B;
                    bytes[2] = color.G;
                    bytes[3] = color.R;
                    break;
                case PixelFormat.RGB8:
                    bytes[0] = color.B;
                    bytes[1] = color.G;
                    bytes[2] = color.R;
                    break;
                case PixelFormat.RGBA5551:
                    bytes[1] = (byte)((color.G & 0xE0) >> 5);
                    bytes[1] += (byte)(color.R & 0xF8);
                    bytes[0] = (byte)((color.B & 0xF8) >> 2);
                    bytes[0] = (byte)(color.A > 0x80 ? 1 : 0);
                    bytes[0] += (byte)((color.G & 0x18) << 3);
                    break;
                case PixelFormat.RGB565:
                    bytes[1] = (byte)((color.G & 0xE0) >> 5);
                    bytes[1] += (byte)(color.R & 0xF8);
                    bytes[0] = (byte)(color.B >> 3);
                    bytes[0] += (byte)((color.G & 0x1C) << 3);
                    break;
                case PixelFormat.RGBA4:
                    bytes[1] = (byte)((color.G & 0xF0) >> 4);
                    bytes[1] += (byte)(color.R & 0xF0);
                    bytes[0] = (byte)((color.A & 0xF0) >> 4);
                    bytes[0] += (byte)(color.B & 0xF0);
                    break;
                default:
                    bytes[0] = 0;
                    bytes[1] = 0;
                    break;
            }
        }

        private static void DecodeTile(int iconSize, int tileSize, int ax, int ay, Image bmp, Stream fs, PixelFormat pixelFormat)
        {
            if (tileSize == 0)
            {
                fs.Read(TempBytes, 0, 2);
                bmp.SetPixel(ax, ay, DecodeColor((TempBytes[1] << 8) + TempBytes[0], pixelFormat));
            }
            else
                for (var y = 0; y < iconSize; y += tileSize)
                    for (var x = 0; x < iconSize; x += tileSize)
                        DecodeTile(tileSize, tileSize / 2, x + ax, y + ay, bmp, fs, pixelFormat);
        }

        private static void EncodeTile(int iconSize, int tileSize, int ax, int ay, Image bmp, Stream fs, PixelFormat pixelFormat)
        {
            if (tileSize == 0)
            {
                EncodeColor(bmp.GetPixel(ax, ay), pixelFormat, TempBytes);
                fs.Write(TempBytes, 0, PixelFormatBytes(pixelFormat));
            }
            else
                for (var y = 0; y < iconSize; y += tileSize)
                    for (var x = 0; x < iconSize; x += tileSize)
                        EncodeTile(tileSize, tileSize / 2, x + ax, y + ay, bmp, fs, pixelFormat);
        }

        public static Image ReadImageFromStream(Stream fs, int width, int height, PixelFormat pixelFormat)
        {
            Image image = new Image
            {
                Width = width,
                Height = height,
                Comp = 4,
                Data = new byte[width * height * 4]
            };
            for (var y = 0; y < height; y += 8)
                for (var x = 0; x < width; x += 8)
                    DecodeTile(8, 8, x, y, image, fs, pixelFormat);
            return image;
        }

        public static void WriteImageToStream(Image source, Stream fs, PixelFormat pixelFormat)
        {
            for (var y = 0; y < source.Height; y += 8)
                for (var x = 0; x < source.Width; x += 8)
                    EncodeTile(8, 8, 0, 0, source, fs, pixelFormat);
        }
    }
}

static class Extensions
{
    public static void SetPixel(this Image img, int x, int y, Color color)
    {
        int offset = (y * img.Width + x) * img.Comp;
        img.Data[offset + 0] = color.R;
        img.Data[offset + 1] = color.G;
        img.Data[offset + 2] = color.B;
        img.Data[offset + 3] = color.A;
    }

    public static Color GetPixel(this Image img, int x, int y)
    {
        int offset = (y * img.Width + x) * img.Comp;
        var red = img.Data[offset + 0];
        var green = img.Data[offset + 1];
        var blue = img.Data[offset + 2];
        var alpha = img.Data[offset + 3];
        return Color.FromArgb(alpha, red, green, blue);
    }
}