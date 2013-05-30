using Capstone.Engine.Windows;
using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class MainPage
    {
        private Direct3DPanelProxy _proxy;

        public MainPage()
        {
            InitializeComponent();
            OpenMainMenu();
        }

        private void InitBGPanel()
        {
            if (_proxy == null)
            {
                _proxy = new Direct3DPanelProxy();
                _proxy.SetPanel(bgPanel, false);
            }
        }

        private void OpenMainMenu()
        {
            mainMenuPanel.Visibility = Visibility.Visible;
            editorPanel.Visibility = Visibility.Collapsed;
        }

        private void OpenEditor()
        {
            InitBGPanel();
            editorPanel.Visibility = Visibility.Visible;
            mainMenuPanel.Visibility = Visibility.Collapsed;
        }

        private void OpenEditorLoad(object sender, RoutedEventArgs e)
        {
            OpenEditor();
        }

        private void OpenEditorNew(object sender, RoutedEventArgs e)
        {
            OpenEditor();
        }

        private void ToggleEditorObjectives(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            EdObjectivePanel.Visibility = EdObjectivePanel.Visibility == Visibility.Visible
                                              ? Visibility.Collapsed
                                              : Visibility.Visible;
        }
    }
}
