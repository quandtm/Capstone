using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Capstone.Editor.Common
{
    public static class StringDataExtensions
    {
        public static void WriteStringEx(this DataWriter writer, string str)
        {
            var measured = writer.MeasureString(str);
            writer.WriteUInt32(measured);
            writer.WriteString(str);
        }

        public static async Task<string> ReadStringEx(this DataReader reader)
        {
            await reader.LoadAsync(sizeof(UInt32));
            var measured = reader.ReadUInt32();
            await reader.LoadAsync(measured);
            return reader.ReadString(measured);
        }
    }
}
