using Windows.UI.Xaml.Input;
using Capstone.Screens;

namespace Capstone.Pages
{
    public sealed partial class EditorPage : INavigatable
    {
        private EditorScreen _screen;

        public EditorPage()
        {
            InitializeComponent();
            _screen = App.ChangeScreen<EditorScreen>();
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
