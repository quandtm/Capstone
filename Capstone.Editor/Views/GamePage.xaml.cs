using Capstone.Editor.ViewModels;
using System.Threading.Tasks;

namespace Capstone.Editor.Views
{
    public sealed partial class GamePage : IView
    {
        private bool _ready;
        private GameViewModel VM { get; set; }

        public GamePage()
        {
            _ready = true;
            InitializeComponent();
            VM = new GameViewModel();
            DataContext = VM;
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel, true);
            _ready = true;
        }

        public async Task HandleNavigationTo(object parameter)
        {
        }

        public async Task HandleNavigationFrom()
        {
        }

        public void Load(Windows.Storage.StorageFile file)
        {
            VM.Load(file);
        }
    }
}
