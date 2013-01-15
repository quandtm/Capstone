using Capstone.Editor.ViewModels;
using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class EditorPage
    {
        private EditorViewModel VM
        {
            get { return (EditorViewModel) DataContext; }
        }

        public EditorPage()
        {
            InitializeComponent();
            DataContext = new EditorViewModel();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel);
        }

        private void OpenBuildMode(object sender, RoutedEventArgs e)
        {
        }

        private void OpenLandscapeMode(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolSelect(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolPan(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadSprite(object sender, RoutedEventArgs e)
        {
            var fd = new FileOpenPicker();
            fd.FileTypeFilter.Add(".dds");
            fd.CommitButtonText = "Load";
            var files = await fd.PickMultipleFilesAsync();
            foreach (var f in files)
                VM.OpenSprite(f);
        }
    }
}
