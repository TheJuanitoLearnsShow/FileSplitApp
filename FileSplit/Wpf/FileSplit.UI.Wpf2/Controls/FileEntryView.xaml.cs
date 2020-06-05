using FileSplit.UI.Wpf2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSplit.UI.Wpf2.Controls
{
    /// <summary>
    /// Interaction logic for FileEntryView.xaml
    /// </summary>
    public partial class FileEntryView : UserControl
    {
        private readonly FileEntry _viewModel;

        public FileEntryView()
        {
            InitializeComponent();
            _viewModel = this.DataContext;
        }
        private void PickFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (System.Windows.Forms.DialogResult.OK == result)
                {
                    _viewModel.SelectedFilePath = dialog.SelectedPath;
                }
            }
        }
        private void PickFile()
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _viewModel.SelectedFilePath = openFileDialog.FileName;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsFolder)
            {
                PickFolder();
            }
            else
            {
                PickFile();
            }
        }
    }
}
