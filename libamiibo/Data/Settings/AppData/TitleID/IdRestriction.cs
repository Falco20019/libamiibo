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

namespace LibAmiibo.Data.Settings.AppData.TitleID
{
    public static class IdRestriction
    {
        [Flags]
        public enum TitleType
        {
            Unknown = 0xFF,
            System = 0x01,
            Application = 0x02,
            Evaluation = 0x04,
            Prototype = 0x08
        }

        internal static TitleType GetType(ulong id)
        {
            TitleType type = 0;
            if (id >= 0x00000 && id <= 0x002FF)
                type |= TitleType.System;
            if (id >= 0x00300 && id <= 0xF7FFF)
                type |= TitleType.Application;
            if (id >= 0xF8000 && id <= 0xFFFFF)
                type |= TitleType.Evaluation;
            if (id >= 0xFF000 && id <= 0xFF3FF)
                type |= TitleType.Prototype;

            if (type != 0)
                return type;

            return TitleType.Unknown;
        }
    }
}