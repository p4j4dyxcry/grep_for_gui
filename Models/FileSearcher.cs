using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grepper.Mvmm;

namespace Grepper.Models
{
    public enum MultiSearchType
    {
        None, Or, And
    }

    public enum FileSearcherStatus
    {
        None,
        Find,
        Error,
    }
    
    public class FileSearcher : NotifyPropertyChanger
    {
        private FileSearcherStatus _status = FileSearcherStatus.None;

        public FileSearcherStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string _filter = "*.*";

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        private bool _isSubDirectorySearch = true;

        public bool IsSubDirectorySearch
        {
            get => _isSubDirectorySearch;
            set => SetProperty(ref _isSubDirectorySearch, value);
        }

        private bool _useRegex = true;

        public bool UseRegex
        {
            get => _useRegex;
            set => SetProperty(ref _useRegex, value);
        }

        private bool _isFileOnly = true;

        public bool IsFileOnly
        {
            get => _isFileOnly;
            set => SetProperty(ref _isFileOnly, value);
        }


        private string _fulllpath = "C:\\";
        public string FullPath
        {
            get => _fulllpath;
            set => SetProperty(ref _fulllpath, value);
        }

        private MultiSearchType _multiSearchType = MultiSearchType.None;

        public MultiSearchType MultiSearch
        {
            get => _multiSearchType;
            set => SetProperty(ref _multiSearchType, value);
        }

        private string _searchWord;
        public string SearchWord
        {
            get => _searchWord;
            set => SetProperty(ref _searchWord, value);
        }

        public bool IsSearching => Lock;

        public void StopSearch()
        {
            Lock = false;
            Status = FileSearcherStatus.None;
        }
        
        public async Task SearchAsync(Action<FileInfo> func)
        {
            if (Lock)
                return;

            await Task.Run(() =>
            {
                Lock = true;
                try
                {
                    Status = FileSearcherStatus.Find;
                    foreach (var fileSystem in find_filesystems())
                    {
                        if (Lock is false) break;

                        var words = MultiSearch == MultiSearchType.None ? new[] { SearchWord } : SearchWord.Split(' ');
                        words = words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                        if (words.Length == 0)
                            func?.Invoke(new FileInfo(fileSystem));

                        else if (search_data(fileSystem, words, func))
                            func?.Invoke(new FileInfo(fileSystem));

                        else if (search_data(File.ReadAllText(fileSystem), words, func))
                            func?.Invoke(new FileInfo(fileSystem));
                    }

                    Status = FileSearcherStatus.None;
                }
                catch
                {
                    Status = FileSearcherStatus.Error;
                }

            });
            Lock = false;
        }
        
        private readonly object _lockToken = new object();

        private bool _lockFlag;

        private bool Lock
        {
            get
            {
                lock (_lockToken)
                {
                    return _lockFlag;
                }
            }
            set
            {
                lock (_lockToken)
                {
                    _lockFlag = value;
                }
            }
        }

        private IEnumerable<string> find_filesystems()
        {
            if (IsFileOnly)
                return Directory.EnumerateFiles(FullPath, Filter,
                    IsSubDirectorySearch
                        ? SearchOption.AllDirectories
                        : SearchOption.TopDirectoryOnly);
            else
                return Directory.EnumerateFileSystemEntries(FullPath, Filter,
                    IsSubDirectorySearch
                        ? SearchOption.AllDirectories
                        : SearchOption.TopDirectoryOnly);
        }

        private bool search_data(string entry, string[] words, Action<FileInfo> func)
        {
            var mutch_count = 0;
            //! check file name
            foreach (var word in words)
            {
                if (MultiSearch != MultiSearchType.And)
                {
                    if (UseRegex)
                        if (Regex.IsMatch(entry, word))
                            return true;

                        else if (Regex.IsMatch(entry, word))
                            return true;
                }
                else
                {
                    if (UseRegex)
                        if (Regex.IsMatch(entry, word))
                            mutch_count++;
                        else if (Regex.IsMatch(entry, word))
                            mutch_count++;
                }
            }

            if (mutch_count == words.Length)
                return true;

            return false;
        }
    }
}
