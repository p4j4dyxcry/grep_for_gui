using System;
using Grepper.Extensions;

namespace Grepper.Models
{
    public class FileSize : IComparable
    {
        public ByteType Type { get; }
        public long ByteSize { get; }

        //! 小数有効桁
        public int Digit { get; set; }

        public FileSize(ByteType type, long byteSize)
        {
            Type = type;
            ByteSize = byteSize;

            //! B , KB は整数で切り上げで表示するのでそれ以外は有効桁を設定している
            const int digit = 2;

            if (type == ByteType.Auto && ByteSize > 1024 * 1024)
                Digit = digit;

            if (type == ByteType.Mb)
                Digit = digit;

            if (type == ByteType.Gb)
                Digit = digit;

        }
        public override string ToString()
        {
            if (Digit == 0)
                return Type.GetSizeString(ByteSize);
            else
                return Type.GetSizeStringDouble(ByteSize, Digit);
        }

        //! ソート用
        public int CompareTo(object obj)
        {
            if (obj is FileSize fileSize)
            {
                if (fileSize.ByteSize > ByteSize)
                    return 1;
                if (fileSize.ByteSize < ByteSize)
                    return -1;
                return 0;
            }

            return 0;
        }
    }
}
