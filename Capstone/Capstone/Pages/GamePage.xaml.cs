using System.ComponentModel;
using Windows.Devices.Input;
using Capstone.Screens;

namespace Capstone.Pages
{
    public sealed partial class GamePage : INavigatable, INotifyPropertyChanged
    {
        private GameScreen _screen;

        public bool ShowTouchUI { get; private set; }

        public GamePage()
        {
            InitializeComponent();

            _screen = App.ChangeScreen<GameScreen>();

            DataContext = this;
        }

        public void OnNavigatedTo()
        {
            ShowTouchUI = new TouchCapabilities().TouchPresent >= 1;
        }

        public void OnNavigatedFrom()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
