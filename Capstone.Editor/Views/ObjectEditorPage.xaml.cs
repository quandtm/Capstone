using Capstone.Editor.Data;
using Capstone.Editor.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Capstone.Editor.Views
{
    public sealed partial class ObjectEditorPage
    {
        public ObjectEditorViewModel VM
        {
            get { return (ObjectEditorViewModel)DataContext; }
        }

        public ObjectEditorPage()
        {
            InitializeComponent();
            DataContext = new ObjectEditorViewModel();
        }

        private void GoBack(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.CurrentApp.GoBack();
        }

        private void AddTemplate(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var component = btn.Tag as ComponentTemplate;
            if (component == null) return;
            VM.AddComponent(component);
        }

        private void DelTemplate(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;
            var component = btn.Tag as ComponentTemplate;
            if (component == null) return;
            VM.RemoveComponent(component);
        }

        private void CreateNewEntity(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            VM.CreateNewEntity();
        }
    }
}
