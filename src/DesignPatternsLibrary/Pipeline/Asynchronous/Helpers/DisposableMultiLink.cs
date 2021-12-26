using System;
using System.Collections.Generic;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public sealed class DisposableMultiLink : IDisposable
    {
        private readonly IEnumerable<IDisposable> _contents;
        public DisposableMultiLink(params IDisposable[] items)
        {
            _contents = items;
        }
        public void Dispose()
        {
            foreach (var item in _contents)
                item?.Dispose();
        }
    }
}
