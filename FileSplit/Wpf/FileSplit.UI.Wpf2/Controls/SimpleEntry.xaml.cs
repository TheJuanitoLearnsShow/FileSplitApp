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
    public partial class SimpleEntryView : UserControl
    {
        private SimpleEntry _viewModel;

        public SimpleEntryView()
        {
            InitializeComponent();
        }
       
    }
}
