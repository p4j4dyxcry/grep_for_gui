using Grepper.Models;

namespace Grepper.Extensions
{
    public static class FileSearcherStatusExtensions
    {
        public static string ToStr(this FileSearcherStatus e)
        {
            switch (e)
            {
                case FileSearcherStatus.Find: return "検索中";
                case FileSearcherStatus.Error: return "検索中にエラーが発生しました";
                case FileSearcherStatus.None: return "停止中";
                default: return "不明";
            }
        }
    }
}
