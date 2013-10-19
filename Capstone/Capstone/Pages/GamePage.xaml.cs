using Capstone.Screens;

namespace Capstone.Pages
{
    public sealed partial class GamePage : INavigatable
    {
        private GameScreen _screen;

        public GamePage()
        {
            InitializeComponent();

            _screen = App.ChangeScreen<GameScreen>();
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
