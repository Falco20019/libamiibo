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
    public class SubCharacter
    {
        private static Dictionary<long, string> dict = new Dictionary<long, string>
        {
            { 0x000001, "Dr." },
            { 0x000301, "Yarn" },
            { 0x000401, "& Luma" },
            { 0x0005ff, "Hammer Slam" },
            { 0x0008ff, "Turbo Charge" },
            { 0x010001, "Toon" },
            { 0x010101, "Shiek" },
            { 0x010201, "-dorf" },
            { 0x018101, "Winter" },
            { 0x018102, "Festival" },
            { 0x018103, "Sommer" },
            { 0x018201, "DJ" },
            //{ 0x018301, "???" },            // TODO: Some special Tom Nook
            //{ 0x018502, "???" },            // TODO: Some special Timmy
            //{ 0x018504, "???" },            // TODO: Some special Timmy
            //{ 0x018601, "???" },            // TODO: Some special Tommy
            //{ 0x018603, "???" },            // TODO: Some special Tommy
            //{ 0x018C01, "???" },            // TODO: Some special Digby
            //{ 0x018E01, "???" },            // TODO: Some special Resetti
            //{ 0x018F01, "???" },            // TODO: Some special Don Resetti
            //{ 0x01A801, "???" },            // TODO: Some special Redd
            //{ 0x01B101, "???" },            // TODO: Some special Shrunk
            //{ 0x01C101, "???" },            // TODO: Some special Lottie
            { 0x028f01, "Sanrio series" },
            { 0x02e001, "Sanrio series" },
            { 0x032e01, "Sanrio series" },
            { 0x350001, "Male rider" },
            { 0x350002, "Female rider" },
            { 0x037401, "Sanrio series" },
            { 0x04a801, "Sanrio series" },
            { 0x04d301, "Sanrio series" },
            { 0x05c001, "Zero Suit" },
            { 0x064001, "& Pikmin" },
            { 0x07c000, "Brawler" },
            { 0x07c001, "Swordfighter" },
            { 0x07c002, "Gunner" },
            { 0x080001, "Girl" },
            { 0x080002, "Boy" },
            { 0x080003, "Squid" },
            { 0x199601, "Shadow" },
            { 0xFFFFFF, "(empty)" },
        };

        internal static string GetName(long id)
        {
            string name;
            if (dict.TryGetValue(id, out name))
                return name;

            return null;
        }
    }
}