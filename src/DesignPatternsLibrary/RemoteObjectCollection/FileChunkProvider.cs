using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    internal class FileChunkProvider<T> : IChunkProvider<Chunk<T>>
        where T : ISerializable
    {
        private readonly HashSet<FileChunkId> _files = new HashSet<FileChunkId>();
        private readonly IFormatter _formatter;
        private readonly DirectoryInfo _filesDirectory;

        public FileChunkProvider(IFormatter formatter, DirectoryInfo filesDirectory)
        {
            _formatter = formatter;
            _filesDirectory = filesDirectory;
        }

        public IChunkId Create(Chunk<T> chunk)
        {
            Guid id = Guid.NewGuid();
            FileInfo newFile = new FileInfo($"{_filesDirectory}\\{id}");
            FileChunkId newChunkId = new FileChunkId(id, newFile);

            using (var fs = newFile.OpenWrite())
                _formatter.Serialize(fs, chunk.GetValues());

            _files.Add(newChunkId);
            return newChunkId;
        }

        public Chunk<T> GetChunk(IChunkId chunkId)
        {
            if (chunkId is not FileChunkId fileChunkId)
                throw new ArgumentException(nameof(chunkId));

            if (_files.TryGetValue(fileChunkId, out FileChunkId savedFileChunkId))
                return ReadChunk(savedFileChunkId.File);

            throw new Exception($"Cannot find {nameof(chunkId)} in chunk list");
        }

        public bool Remove(IChunkId chunkId)
        {
            if (chunkId is not FileChunkId fileChunkId)
                throw new ArgumentException(nameof(chunkId));

            RemoveFileFromStorage(fileChunkId);

            return _files.RemoveWhere(ch => ch.Id == chunkId.Id) > 0;
        }

        public bool ChangeChunk(Chunk<T> chunk, IChunkId chunkId)
        {
            if (chunkId is not FileChunkId fileChunkId)
                throw new ArgumentException(nameof(chunkId));

            if (!_files.TryGetValue(fileChunkId, out _))
            { 
                Create(chunk); 
                return true;
            }

            RemoveFileFromStorage(fileChunkId);

            using (FileStream fs = fileChunkId.File.Open(FileMode.OpenOrCreate))
            {
                _formatter.Serialize(fs, chunk.GetValues());
                return true;
            }
        }

        public void Dispose()
        {
            foreach (FileChunkId chunkId in _files)
            {
                Remove(chunkId);
            }
        }

        private Chunk<T> ReadChunk(FileInfo chunkFile)
        {
            using var fs = chunkFile.OpenRead();
            T[] deserializedObjects = (T[])_formatter.Deserialize(fs);
            return new Chunk<T>(deserializedObjects);
        }

        private static void RemoveFileFromStorage(FileChunkId fileChunkId)
        {
            if (fileChunkId.File.Exists)
                fileChunkId.File.Delete();
        }
    }

    class FileChunkId : IChunkId
    {
        private FileInfo _fileInfo;

        public FileChunkId(Guid id, FileInfo fileInfo)
        {
            Id = id;
            _fileInfo = fileInfo;
        }

        public Guid Id { get; }

        public object Path => _fileInfo.FullName;

        public FileInfo File => _fileInfo;

        public int Count { get; }
    }
}
