namespace Capstone.Resources
{
    public interface IResource
    {
        string ResourceKey { get; set; }
        void Load(ResourceCache cache);
        void Unload(ResourceCache cache);
    }
}
