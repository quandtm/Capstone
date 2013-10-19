using Capstone.Core;
using Capstone.Graphics;
using SharpDX;

namespace Capstone.Screens
{
    public class GameScreen : IScreen
    {
        public void Initialise()
        {
        }

        public void Destroy()
        {
        }

        public void Update(double elapsedSeconds)
        {
        }

        public void Draw(double elapsedSeconds)
        {
            XamlGraphicsDevice.Instance.Clear();

            // Render here

            XamlGraphicsDevice.Instance.Present();
        }

        public void OnNavigatedTo()
        {
            XamlGraphicsDevice.Instance.ClearColour = Color.Magenta;
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
