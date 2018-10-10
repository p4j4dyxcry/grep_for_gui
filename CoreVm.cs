using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Grepper.Models;
using Grepper.Mvmm;
using Grepper.Extensions;

namespace Grepper
{
    public class CoreVm : NotifyPropertyChanger
    {
        private FileSearcher _model;

        public ObservableCollection<FileVm> Items { get; } = new ObservableCollection<FileVm>();

        public FileVm _selectedItem;

        public FileVm SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string PreviewText => SelectedItem?.Text ?? string.Empty;

        public CoreVm(FileSearcher fileSearcher)
        {
            _model = fileSearcher;
            _model.PropertyChanged += (s, e) => Application.Current.Dispatcher.InvokeAsync(()=>OnPropertyChanged(e.PropertyName));
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedItem))
                    OnPropertyChanged(nameof(PreviewText));
            };

            SearchCommand = new ActionCommand(() =>
            {
                async void RunSearch()
                {
                    await _model.SearchAsync(x =>
                    {
                        Application.Current.Dispatcher.InvokeAsync(() => { Items.Add(new FileVm(x)); });
                    });
                }

                StopCommand?.Execute(null);
                Items.Clear();
                RunSearch();
            });

            StopCommand = new ActionCommand(() => _model.StopSearch()); 

            OpenCommand = new ActionCommand(() =>
            {
                _selectedItem?.OpenCommand?.Execute(null);
            });

            OpenWithExplolerCommand = new ActionCommand(() =>
            {
                _selectedItem?.OpenWithExplolerCommand?.Execute(null);
            });

        }

        public ICommand SearchCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand OpenCommand { get; }

        public ICommand OpenWithExplolerCommand { get; }

        public string FullPath
        {
            get => _model.FullPath;
            set => _model.FullPath = value;
        }

        public string SearchWord
        {
            get => _model.SearchWord;
            set => _model.SearchWord = value;
        }

        public string Filter
        {
            get => _model.Filter;
            set => _model.Filter = value;
        }

        public bool IsSubDirectorySearch
        {
            get => _model.IsSubDirectorySearch;
            set => _model.IsSubDirectorySearch = value;
        }

        public bool IsFileOnly
        {
            get => _model.IsFileOnly;
            set => _model.IsFileOnly = value;
        }

        public bool UseRegex
        {
            get => _model.UseRegex;
            set => _model.UseRegex = value;
        }

        public MultiSearchType MultiSearch
        {
            get => _model.MultiSearch;
            set => _model.MultiSearch = value;
        }

        public string Status => _model.Status.ToStr();
    }
}
