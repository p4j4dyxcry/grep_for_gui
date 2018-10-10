using System.Windows;
using Grepper.Models;
using ICSharpCode.AvalonEdit;
namespace Grepper
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var fileSearcher = new FileSearcher();
            DataContext = new CoreVm(fileSearcher);
        }
    }
}