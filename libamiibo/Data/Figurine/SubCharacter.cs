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
            { 0x008001, "Yarn" },
            { 0x010001, "Toon" },
            { 0x010101, "Sheik" },
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
            { 0x080501, "Girl" },
            { 0x080502, "Boy" },
            { 0x080503, "Octopus" },
            { 0x09C001, "Soccer" },
            { 0x09C002, "Baseball" },
            { 0x09C003, "Tennis" },
            { 0x09C004, "Golf" },
            { 0x09C005, "Horse Racing" },
            { 0x09C101, "Soccer" },
            { 0x09C102, "Baseball" },
            { 0x09C103, "Tennis" },
            { 0x09C104, "Golf" },
            { 0x09C105, "Horse Racing" },
            { 0x09C201, "Soccer" },
            { 0x09C202, "Baseball" },
            { 0x09C203, "Tennis" },
            { 0x09C204, "Golf" },
            { 0x09C205, "Horse Racing" },
            { 0x09C301, "Soccer" },
            { 0x09C302, "Baseball" },
            { 0x09C303, "Tennis" },
            { 0x09C304, "Golf" },
            { 0x09C305, "Horse Racing" },
            { 0x09C401, "Soccer" },
            { 0x09C402, "Baseball" },
            { 0x09C403, "Tennis" },
            { 0x09C404, "Golf" },
            { 0x09C405, "Horse Racing" },
            { 0x09C501, "Soccer" },
            { 0x09C502, "Baseball" },
            { 0x09C503, "Tennis" },
            { 0x09C504, "Golf" },
            { 0x09C505, "Horse Racing" },
            { 0x09C601, "Soccer" },
            { 0x09C602, "Baseball" },
            { 0x09C603, "Tennis" },
            { 0x09C604, "Golf" },
            { 0x09C605, "Horse Racing" },
            { 0x09C701, "Soccer" },
            { 0x09C702, "Baseball" },
            { 0x09C703, "Tennis" },
            { 0x09C704, "Golf" },
            { 0x09C705, "Horse Racing" },
            { 0x09C801, "Soccer" },
            { 0x09C802, "Baseball" },
            { 0x09C803, "Tennis" },
            { 0x09C804, "Golf" },
            { 0x09C805, "Horse Racing" },
            { 0x09C901, "Soccer" },
            { 0x09C902, "Baseball" },
            { 0x09C903, "Tennis" },
            { 0x09C904, "Golf" },
            { 0x09C905, "Horse Racing" },
            { 0x09CA01, "Soccer" },
            { 0x09CA02, "Baseball" },
            { 0x09CA03, "Tennis" },
            { 0x09CA04, "Golf" },
            { 0x09CA05, "Horse Racing" },
            { 0x09CB01, "Soccer" },
            { 0x09CB02, "Baseball" },
            { 0x09CB03, "Tennis" },
            { 0x09CB04, "Golf" },
            { 0x09CB05, "Horse Racing" },
            { 0x09CC01, "Soccer" },
            { 0x09CC02, "Baseball" },
            { 0x09CC03, "Tennis" },
            { 0x09CC04, "Golf" },
            { 0x09CC05, "Horse Racing" },
            { 0x09CD01, "Soccer" },
            { 0x09CD02, "Baseball" },
            { 0x09CD03, "Tennis" },
            { 0x09CD04, "Golf" },
            { 0x09CD05, "Horse Racing" },
            { 0x09CE01, "Soccer" },
            { 0x09CE02, "Baseball" },
            { 0x09CE03, "Tennis" },
            { 0x09CE04, "Golf" },
            { 0x09CE05, "Horse Racing" },
            { 0x09CF01, "Soccer" },
            { 0x09CF02, "Baseball" },
            { 0x09CF03, "Tennis" },
            { 0x09CF04, "Golf" },
            { 0x09CF05, "Horse Racing" },
            { 0x09D001, "Soccer" },
            { 0x09D002, "Baseball" },
            { 0x09D003, "Tennis" },
            { 0x09D004, "Golf" },
            { 0x09D005, "Horse Racing" },
            { 0x09D101, "Soccer" },
            { 0x09D102, "Baseball" },
            { 0x09D103, "Tennis" },
            { 0x09D104, "Golf" },
            { 0x09D105, "Horse Racing" },
            { 0x199601, "Shadow" },
            { 0x210501, "Player 2" },
            { 0x324001, "Player 2" },
            { 0x350001, "Male rider" },
            { 0x350002, "Female rider" },
            { 0x350201, "& Cheval" },
            { 0x350301, "& Ayuria" },
            { 0x350401, "& Dan" },
            { 0x360001, "Player 2" },
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