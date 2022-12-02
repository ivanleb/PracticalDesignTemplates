using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public static class UseCase
    {
        public static void Run() 
        {
            var chunkProvider = ChunkProviders.GetBinaryFileChunkProvider<Item>("\\");
            using RemotePaginationCollection<Item> collection = new RemotePaginationCollection<Item>(2000, chunkProvider);

            int length = 200_000;
            for (int i = 0; i < length; i++)
            {
                collection.Add(new Item($"Item{i}"));
            }
        }
    }

    public class Item : ISerializable
    {
        public string Name;

        public Item(string name)
        {
            Name = name;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("Name", Name);
        }
    }
}
