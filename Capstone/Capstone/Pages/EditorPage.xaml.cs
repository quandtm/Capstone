using Capstone.Screens;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Capstone.Pages
{
    public sealed partial class EditorPage : INavigatable
    {
        private readonly EditorScreen _screen;
        private PointerPoint _prevPoint;

        public EditorPage()
        {
            InitializeComponent();
            _screen = App.ChangeScreen<EditorScreen>();
            _prevPoint = null;
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        private void HandlePointerDown(object sender, PointerRoutedEventArgs e)
        {
            _prevPoint = e.GetCurrentPoint((UIElement)sender);
        }

        private void HandlePointerUp(object sender, PointerRoutedEventArgs e)
        {
            _prevPoint = null;
        }

        private void HandlePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_prevPoint != null)
            {
                var curPt = e.GetCurrentPoint((UIElement)sender);
                _screen.Camera.Move((float)(_prevPoint.Position.X - curPt.Position.X), (float)(_prevPoint.Position.Y - curPt.Position.Y));
                _prevPoint = curPt;
            }
        }
    }
}
