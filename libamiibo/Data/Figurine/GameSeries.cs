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

using System.Collections.Generic;

namespace LibAmiibo.Data.Figurine
{
    public static class GameSeries
    {
        private static Dictionary<int, string> dict = new Dictionary<int, string>
        {
            { 0x000,    "Super Mario" },
            { 0x010,    "Legend of Zelda" },
            { 0x058,    "Star Fox" },
            { 0x05C,    "Metroid" },
            { 0x060,    "F-Zero" },
            { 0x064,    "Pikmin" },
            { 0x06C,    "Punch-Out!!" },
            { 0x070,    "Wii Fit" },
            { 0x074,    "Kid Icarus" },
            { 0x078,    "Classic Nintendo" },
            { 0x07c,    "Mii" },
            { 0x080,    "Splatoon" },
            { 0x1D0,    "Pokken" },
            { 0x1F0,    "Kirby" },
            { 0x210,    "Fire Emblem" },
            { 0x224,    "Xenoblade Chronicles" },
            { 0x228,    "Earthbound" },
            { 0x22c,    "Chibi-Robo!" },
            { 0x320,    "Sonic the Hedgehog" },
            { 0x334,    "PAC-MAN" },
            { 0x348,    "Mega Man" },
            { 0x34C,    "Street Fighter" },
            { 0x35C,    "Shovel Knight" },
            //{ 0xXXX,    "Final Fantasy" },
            //{ 0xXXX,    "Bayonetta" },
            //{ 0xXXX,    "Monster Hunter" },
            { 0xFFF,    "(empty)" },
        };

        internal static string GetName(int id)
        {
            string name;
            if (dict.TryGetValue(id, out name))
                return name;
            if (id >= 0x018 && id <= 0x051)
                return "Animal Crossing";
            if (id >= 0x190 && id <= 0x1BD)
                return "Pokémon";

            return null;
        }
    }
}