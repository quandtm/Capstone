namespace Capstone.Core
{
    public interface IComponent
    {
        Entity Owner { get; set; }

        void Initialise();
        void Destroy();
    }
}
