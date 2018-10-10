using Grepper.Mvmm;
using System.IO;
using System.Windows.Input;
using Grepper.Models;
using FileInfo = Grepper.Models.FileInfo;

namespace Grepper
{
    public class FileVm : NotifyPropertyChanger
    {
        private readonly FileInfo _model;

        public string Name => Path.GetFileName(_model.FullPath);

        public string Directory => Path.GetDirectoryName(_model.FullPath);

        public string Text => _model.GetText();

        public FileSize FileSize => _model.Size;

        public ICommand OpenWithExplolerCommand { get; }

        public ICommand OpenCommand { get; }

        public FileVm(FileInfo model)
        {
            _model = model;

            OpenCommand = new ActionCommand(()=> _model.OpenWithAssociation());
            OpenWithExplolerCommand = new ActionCommand(() => _model.OpenWithExploler());
        }
    }
}
