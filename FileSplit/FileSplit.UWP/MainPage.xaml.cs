using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace FileSplit.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new FileSplit.App());
        }
    }
}
