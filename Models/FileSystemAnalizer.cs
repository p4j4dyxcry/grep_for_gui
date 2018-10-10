using System;
using System.IO;

namespace Grepper.Models
{
    public class FileSystemAnalyzer
    {
        private static readonly string Unknown = "Unknown";
        private static readonly string Folder = "フォルダー";

        //! ファイル / フォルダーの種類を取得する
        public string GetKind(string fullpath)
        {
            var kind = Unknown;
            try
            {
                kind = GetKindFromFilePath(fullpath);
            }
            catch (Exception)
            {
                // ignored
            }

            return kind;
        }

        public static string GetKindFromFilePath(string src)
        {
            var path = src.TrimEnd('\\', '/');
            var kind = $"{Path.GetExtension(path).ToUpper()}";
            if (string.IsNullOrEmpty(kind))
            {
                if (Directory.Exists(path))
                    kind = Folder;
            }
            else
            {
                kind = kind.Replace(".", "");
            }

            return kind;
        }

        public string GetTimeStamp(string fullpath)
        {
            try
            {
                return File.GetLastWriteTime(fullpath).ToString("yyyy/MM/dd HH:mm");
            }
            catch (Exception)
            {
                // ignored
            }


            return Unknown;
        }

        public string GetName(string fullpath)
        {
            try
            {
                //! 最後が区切り文字で終わっているデータが空文字になるケースをケアする
                return Path.GetFileName(fullpath.TrimEnd('\\', '/'));
            }
            catch (Exception)
            {
                // ignored
            }

            return Unknown;
        }

        public FileSize GetFileSize(string fullpath, ByteType type = ByteType.Auto)
        {
            long sz = 1;

            try
            {
                if (Directory.Exists(fullpath))
                    sz = 0;
                else if (File.Exists(fullpath))
                    sz = new System.IO.FileInfo(fullpath).Length;

                return new FileSize(type, sz);
            }
            catch (Exception)
            {
                // ignored
            }

            return new FileSize(type, 0);
        }
    }
}
