using System;
using System.Collections.Generic;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public class RemoteCollectionEnumerator<T> : IEnumerator<T>
    {
        private readonly RemoteCollection<T> _remoteCollection;
        private int position = -1;
        public RemoteCollectionEnumerator(RemoteCollection<T> remoteCollection)
        {
            _remoteCollection = remoteCollection;
        }

        public object Current => IsPositionInCollectionRange() ? _remoteCollection[position] : throw new InvalidOperationException();
        T IEnumerator<T>.Current => IsPositionInCollectionRange() ? _remoteCollection[position] : throw new InvalidOperationException();

        public bool MoveNext() => ++position < _remoteCollection.Count - 1;
        public void Reset() => position = -1;
        public void Dispose()
        { }
        private bool IsPositionInCollectionRange() => !(position == -1 || position >= _remoteCollection.Count);
    }
}
