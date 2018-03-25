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

using LibAmiibo.Helper;
using System.Collections.Generic;

namespace LibAmiibo.Data.Figurine
{
    public static class AmiiboSet
    {
        private static Dictionary<byte, GroupName> dict = new Dictionary<byte, GroupName>
        {
            { 0x00, new GroupName("Super Smash Bros.", "SSB") },
            { 0x01, new GroupName("Super Mario Bros.", "SMB") },
            { 0x02, new GroupName("Chibi-Robo!", "CHI") },
            { 0x03, new GroupName("Yoshi's Woolly World", "YWW") },
            { 0x04, new GroupName("Splatoon", "SPL") },
            { 0x05, new GroupName("Animal Crossing", "ANC") },
            { 0x06, new GroupName("8-bit Mario", "8BT") },
            { 0x07, new GroupName("Skylanders", "SKY") },
            // Missing
            { 0x09, new GroupName("The Legend of Zelda", "LOZ") },
            { 0x0A, new GroupName("Shovel Knight", "SHK") },
            // Missing
            { 0x0C, new GroupName("Kirby", "KIR") },
            { 0x0D, new GroupName("Pokemon", "POK") },
            { 0x0E, new GroupName("Mario Sports Superstars", "MSS") },
            { 0x0F, new GroupName("Monster Hunter", "MHU") },
            { 0x10, new GroupName("BoxBoy!", "BXB") },
            { 0x11, new GroupName("Pikmin", "PIK") },
            { 0x12, new GroupName("Fire Emblem", "FEM") },
            { 0x13, new GroupName("Metroid", "MET") },
            { 0x14, new GroupName("Kellogg's", "KLG") },
            { 0xFF, new GroupName("(empty)", "?") },
        };

        internal static GroupName GetName(byte id)
        {
            GroupName name;
            if (dict.TryGetValue(id, out name))
                return name;

            return null;
        }
    }
}