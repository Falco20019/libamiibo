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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace LibAmiibo.Helper
{
    static class MarshalUtil
    {
        public static string CleanInput(string input)
        {
            input = input.Replace('\n', ' ');
            var end = input.IndexOf('\0');
            if (end == -1)
                return input;
            return input.Remove(end);
        }

        public static string CleanOutput(string output)
        {
            return output.PadRight(10, '\0');
        }

        public static T ReadStruct<T>(Stream fs)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }

        public static T ReadStructBE<T>(Stream fs)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var typedObject = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            typedObject = (T)CorrectEndianness(typedObject);
            return typedObject;
        }

        private static object CorrectEndianness(object typedObject)
        {
            var type = typedObject.GetType();
            var fieldInfo = type.GetFields();

            foreach (var fi in fieldInfo)
            {
                var fieldType = fi.FieldType;
                if (fieldType.IsEnum)
                {
                    fieldType = Enum.GetUnderlyingType(fieldType);
                }

                if (fieldType.IsArray)
                    continue;

                if (!fieldType.IsPrimitive)
                {
                    var obj = fi.GetValue(typedObject);
                    obj = CorrectEndianness(obj);
                    fi.SetValue(typedObject, obj);
                }
                else if (fieldType == typeof (Int16))
                {
                    var i16 = (Int16)fi.GetValue(typedObject);
                    var b16 = BitConverter.GetBytes(i16);
                    var b16R = b16.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToInt16(b16R, 0));
                }
                else if (fieldType == typeof (Int32))
                {
                    var i32 = (Int32)fi.GetValue(typedObject);
                    var b32 = BitConverter.GetBytes(i32);
                    var b32R = b32.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToInt32(b32R, 0));
                }
                else if (fieldType == typeof (Int64))
                {
                    var i64 = (Int64)fi.GetValue(typedObject);
                    var b64 = BitConverter.GetBytes(i64);
                    var b64R = b64.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToInt64(b64R, 0));
                }
                else if (fieldType == typeof (UInt16))
                {
                    var i16 = (UInt16)fi.GetValue(typedObject);
                    var b16 = BitConverter.GetBytes(i16);
                    var b16R = b16.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToUInt16(b16R, 0));
                }
                else if (fieldType == typeof (UInt32))
                {
                    var i32 = (UInt32)fi.GetValue(typedObject);
                    var b32 = BitConverter.GetBytes(i32);
                    var b32R = b32.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToUInt32(b32R, 0));
                }
                else if (fieldType == typeof (UInt64))
                {
                    var i64 = (UInt64)fi.GetValue(typedObject);
                    var b64 = BitConverter.GetBytes(i64);
                    var b64R = b64.Reverse().ToArray();
                    fi.SetValue(typedObject, BitConverter.ToUInt64(b64R, 0));
                }
            }
            return typedObject;
        }
    }
}
