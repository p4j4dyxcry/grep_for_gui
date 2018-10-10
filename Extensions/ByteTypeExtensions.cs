using System;
using Grepper.Models;

namespace Grepper.Extensions
{
    public static class ByteTypeExtension
    {
        private const long Long1024 = 1024;
        public static long ToBitDigits(this ByteType type)
        {
            switch (type)
            {
                case ByteType.B: return 1;
                case ByteType.Kb: return Long1024;
                case ByteType.Mb: return Long1024 * Long1024;
                case ByteType.Gb: return Long1024 * Long1024 * Long1024;
                default: return 1;
            }
        }

        public static long GetSize(this ByteType type, long size)
        {
            long bitDigits = 1;
            if (type == ByteType.Auto)
            {
                if (size >= Long1024 * Long1024 * Long1024)
                    bitDigits = Long1024 * Long1024 * Long1024;
                else if (size >= Long1024 * Long1024)
                    bitDigits = Long1024 * Long1024;
                else if (size >= Long1024)
                    bitDigits = Long1024;
                else
                    bitDigits = 1;
            }
            else
                bitDigits = type.ToBitDigits();

            var remainder = (size % bitDigits) > 0 ? 1 : 0;
            return size / bitDigits + remainder;
        }

        public static double GetSizeDouble(this ByteType type, long size)
        {
            var bitDigits = 1.0d;
            if (type == ByteType.Auto)
            {
                if (size >= Long1024 * Long1024 * Long1024)
                    bitDigits = Long1024 * Long1024 * Long1024;
                else if (size >= Long1024 * Long1024)
                    bitDigits = Long1024 * Long1024;
                else if (size >= Long1024)
                    bitDigits = Long1024;
                else
                    bitDigits = 1;
            }
            else
                bitDigits = type.ToBitDigits();

            return size / bitDigits;
        }

        public static string SizeToByteName(this ByteType type, long size)
        {
            if (type == ByteType.Auto)
            {
                if (size >= Long1024 * Long1024 * Long1024)
                    return "GB";
                else if (size >= Long1024 * Long1024)
                    return "MB";
                else if (size >= Long1024)
                    return "KB";
                else
                    return "B";
            }

            return type.ToString().ToUpper();
        }

        public static string GetSizeString(this ByteType type, long size)
        {
            var b = type.SizeToByteName(size);

            return $"{type.GetSize(size):#,0} {b}";
        }

        public static string GetSizeStringDouble(this ByteType type, long size, int digit)
        {
            var b = type.SizeToByteName(size);

            var roundSz = Math.Round(type.GetSizeDouble(size), digit);
            return $"{roundSz} {b}";
        }
    }
}
