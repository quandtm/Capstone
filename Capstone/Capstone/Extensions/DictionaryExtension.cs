namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        public static T GetOrDefault<T>(this Dictionary<string, object> d, string key, T defaultValue)
        {
            object o;
            if (d.TryGetValue(key, out o))
                return (T)o;
            else
                return defaultValue;
        }
    }
}
