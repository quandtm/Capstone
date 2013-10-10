namespace Capstone.Resources
{
    public interface IResource
    {
        bool IsLoaded { get; }
        string ResourceKey { get; set; }
        void Load(ResourceCache cache);
        void Unload(ResourceCache cache);
    }
}
