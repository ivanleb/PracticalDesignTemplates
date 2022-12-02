using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public static class ChunkProviders 
    {
        public static IChunkProvider<Chunk<T>> GetBinaryFileChunkProvider<T>(string directoryPath) where T : ISerializable
        {
            BinaryFormatter formatter = new BinaryFormatter();
            DirectoryInfo directoryInfo = Directory.CreateDirectory(directoryPath);
            return new FileChunkProvider<T>(formatter, directoryInfo);
        }
    }
}
