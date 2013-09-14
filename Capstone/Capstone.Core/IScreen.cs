namespace Capstone.Core
{
    public interface IScreen
    {
        void Initialise();
        void Destroy();

        void Update(double elapsedSeconds);
        void Draw(double elapsedSeconds);

        void OnNavigatedTo();
        void OnNavigatedFrom();
    }
}
