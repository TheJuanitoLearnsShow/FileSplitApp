using FileSplit.UI.Wpf2.Models;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSplit.UI.Wpf2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SplitForm _viewModel;

        public MainWindow()
        {

            _viewModel = new SplitForm() ;
            this.DataContext = _viewModel;
            InitializeComponent();
        }


        private (string,int) EnsureSafeValues()
        {
            var newFilename = _viewModel.NewFilename.Value;
            if (String.IsNullOrEmpty( newFilename))
            {
                _viewModel.NewFilename.Value = "NewSplitFile";
            }
            var validNumber = int.TryParse( _viewModel.MaxLines.Value, out int maxLinesValue);
            if (!validNumber)
            {
                _viewModel.MaxLines.Value = "1000";
            }
            return (newFilename, maxLinesValue);
        }
        private async void DoSplitBtn_Click(object sender, RoutedEventArgs e)
        {
            var (newFilename, maxLinesValue) = EnsureSafeValues();
            var folderPicked = new FolderPicked(_viewModel.OutputFolderInfo.SelectedFilePath);
            var inputStream = File.OpenRead(_viewModel.InputFileInfo.SelectedFilePath);
            await FileSplit.Core.Splitter.SplitFileAsTask(FuncConvert.ToFSharpFunc<int>( UpdateProgress), inputStream, folderPicked, newFilename, maxLinesValue);
            Progress.Text = $"File split done";
            await folderPicked.OpenInExplorer();
        }

        private void UpdateProgress(int currFileNumber)
        {
            Dispatcher.Invoke(() =>
            {
                Progress.Text = $"File #{currFileNumber} done";
            }
            );
        }
    }
}
