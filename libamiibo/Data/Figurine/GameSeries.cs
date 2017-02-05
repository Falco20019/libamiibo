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
            { 0x008,    new GroupName("Yoshi's Woolly World", "YWW") }, // TODO: Maybe this is Yoshi's Story?
            { 0x010,    new GroupName("Legend of Zelda", "LOZ") },
            { 0x058,    new GroupName("Star Fox", "STF") },
            { 0x05C,    new GroupName("Metroid", "MET") },
            { 0x060,    new GroupName("F-Zero", "FZO") },
            { 0x064,    new GroupName("Pikmin", "PIK") },
            { 0x06C,    new GroupName("Punch-Out!!", "PUO") },
            { 0x070,    new GroupName("Wii Fit", "WIF") },
            { 0x074,    new GroupName("Kid Icarus", "KIC") },
            { 0x078,    new GroupName("Classic Nintendo", "CLN") },
            { 0x07C,    new GroupName("Mii", "MII") },
            { 0x080,    new GroupName("Splatoon", "SPL") },
            { 0x1D0,    new GroupName("Pokken", "POK") },
            { 0x1F0,    new GroupName("Kirby", "KIR") },
            { 0x210,    new GroupName("Fire Emblem", "FEM") },
            { 0x224,    new GroupName("Xenoblade Chronicles", "XCH") },
            { 0x228,    new GroupName("Earthbound", "EBO") },
            { 0x22C,    new GroupName("Chibi-Robo!", "CHI") },
            { 0x320,    new GroupName("Sonic the Hedgehog", "STH") },
            { 0x334,    new GroupName("PAC-MAN", "PAC") },
            { 0x348,    new GroupName("Mega Man", "MMA") },
            { 0x34C,    new GroupName("Street Fighter", "SFI") },
            { 0x350,    new GroupName("Monster Hunter", "MHU") },
            { 0x35C,    new GroupName("Shovel Knight", "SHK") },
            //{ 0xXXX,    new GroupName("Final Fantasy", "FFA") },
            //{ 0xXXX,    new GroupName("Bayonetta", "BAY") },
            { 0xFFF,    new GroupName("(empty)", "?") },
        };

        internal static GroupName GetName(int id)
        {
            GroupName name;
            if (dict.TryGetValue(id, out name))
                return name;
            if (id >= 0x000 && id <= 0x001)
                return new GroupName("Super Mario", "SMA");
            if (id >= 0x018 && id <= 0x051)
                return new GroupName("Animal Crossing", "ACR");
            if (id >= 0x190 && id <= 0x1BD)
                return new GroupName("Pokémon", "POK");

            return null;
        }
    }
}