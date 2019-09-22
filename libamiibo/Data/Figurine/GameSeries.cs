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
    public static class GameSeries
    {
        private static Dictionary<int, GroupName> dict = new Dictionary<int, GroupName>
        {
            { 0x000,    new GroupName("Super Mario", "SMA") },
            // 0x001: ? Maybe extension of Super Mario
            { 0x002,    new GroupName("Yoshi's Woolly World", "YWW") }, // TODO: Maybe this is Yoshi's Story?
            { 0x003,    new GroupName("Donkey Kong", "DKK") },
            { 0x004,    new GroupName("Legend of Zelda", "LOZ") },
            { 0x005,    new GroupName("Breath of the Wild", "BOW") },
            // 0x006 - 0x014: Animal Crossing
            // 0x015: ? Maybe extension of Animal Crossing
            { 0x016,    new GroupName("Star Fox", "STF") },
            { 0x017,    new GroupName("Metroid", "MET") },
            { 0x018,    new GroupName("F-Zero", "FZO") },
            { 0x019,    new GroupName("Pikmin", "PIK") },
            // 0x01A: ???
            { 0x01B,    new GroupName("Punch-Out!!", "PUO") },
            { 0x01C,    new GroupName("Wii Fit", "WIF") },
            { 0x01D,    new GroupName("Kid Icarus", "KIC") },
            { 0x01E,    new GroupName("Classic Nintendo", "CLN") },
            { 0x01F,    new GroupName("Mii", "MII") },
            { 0x020,    new GroupName("Splatoon", "SPL") },
            // 0x021 - 0x026: ???
            { 0x027,    new GroupName("Mario Sports Superstars", "MSS") },
            // 0x028 - 0x063: ???
            // 0x064 - 0x075: Pokémon
            // 0x076 - 0x07B: ? Maybe extension of Pokémon
            { 0x07C,    new GroupName("Kirby", "KIR") },
            { 0x07D,    new GroupName("BoxBoy!", "BXB") },
            // 0x07E - 0x083: ???
            { 0x084,    new GroupName("Fire Emblem", "FEM") },
            // 0x085 - 0x088: ???
            { 0x089,    new GroupName("Xenoblade Chronicles", "XCH") },
            { 0x08A,    new GroupName("Earthbound", "EBO") },
            { 0x08B,    new GroupName("Chibi-Robo!", "CHI") },
            // 0x08C - 0x0C7: ???
            { 0x0C8,    new GroupName("Sonic the Hedgehog", "STH") },
            { 0x0C9,    new GroupName("Bayonetta", "BAY") },
            // 0x0CA - 0x0CB: ???
            { 0x0CD,    new GroupName("PAC-MAN", "PAC") },
            { 0x0CE,    new GroupName("Dark Souls", "DKS") },
            // 0x0CF - 0x0D1: ???
            { 0x0D2,    new GroupName("Mega Man", "MMA") },
            { 0x0D3,    new GroupName("Street Fighter", "SFI") },
            { 0x0D4,    new GroupName("Monster Hunter", "MHU") },
            // 0x0D5 - 0x0D6: ???
            { 0x0D7,    new GroupName("Shovel Knight", "SHK") },
            { 0x0D8,    new GroupName("Final Fantasy", "FFA") },
            // 0x0D9 - 0x0DC: ???
            { 0x0DD,    new GroupName("Kellogs", "KLG") },
            { 0x0DE,    new GroupName("Metal Gear Solid", "MGS") },
            // 0x0DF - 0x3FE: ???
            { 0x0E3,    new GroupName("Diablo", "DBL") },
            // 0x0DE - 0x3FE: ???
            { 0x3FF,    new GroupName("(empty)", "?") },
        };

        internal static GroupName GetName(int id)
        {
            GroupName name;
            if (dict.TryGetValue(id, out name))
                return name;
            if (id >= 0x006 && id <= 0x014)
                return new GroupName("Animal Crossing", "ACR");
            if (id >= 0x064 && id <= 0x075)
                return new GroupName("Pokémon", "POK");

            return null;
        }
    }
}