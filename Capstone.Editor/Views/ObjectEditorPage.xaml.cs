namespace Capstone.Editor.Views
{
    public sealed partial class ObjectEditorPage
    {
        public ObjectEditorPage()
        {
            InitializeComponent();
        }

        private void GoBack(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.CurrentApp.GoBack();
        }
    }
}
