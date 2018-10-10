using System;
using System.Diagnostics;
using System.IO;

namespace Grepper.Models
{
    public class FileInfo
    {
        public bool IsDirectry { get; }

        public FileSize Size { get; }

        public string FullPath { get; }

        public string GetText()
        {
            if (IsDirectry)
                return null;

            // 4mb 以上は開かない
            if (Size.ByteSize > 1024 * 1024 )
                return "ファイルが大きすぎます";

            return File.ReadAllText(FullPath);
        }

        //! エクスプローラで開く
        public void OpenWithExploler()
        {
            var explorerExe = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\EXPLORER.EXE");
            Process.Start(explorerExe, IsDirectry ? FullPath : $"/select,\"{FullPath}\"");
        }

        //! 関連付けで開く
        public void OpenWithAssociation()
        {
            var explorerExe = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\EXPLORER.EXE");
            Process.Start(explorerExe,FullPath);
        }

        public FileInfo(string filePath)
        {
            IsDirectry = Directory.Exists(filePath);

            Size = new FileSystemAnalyzer().GetFileSize(filePath);

            FullPath = filePath;
        }

    }
}
