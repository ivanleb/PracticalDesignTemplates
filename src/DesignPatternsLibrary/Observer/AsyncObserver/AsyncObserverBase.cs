﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer.AsyncObserver
{
    public abstract class AsyncObserverBase<T> : IAsyncObserver<T>
    {
        private const int Idle = 0;
        private const int Busy = 1;
        private const int Done = 2;

        private int _status = Idle;

        public ValueTask OnCompletedAsync()
        {
            TryEnter();

            try
            {
                return OnCompletedAsyncCore();
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract ValueTask OnCompletedAsyncCore();

        public ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            TryEnter();

            try
            {
                return OnErrorAsyncCore(error);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract ValueTask OnErrorAsyncCore(Exception error);

        public ValueTask OnNextAsync(T value)
        {
            TryEnter();

            try
            {
                return OnNextAsyncCore(value);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Idle);
            }
        }

        protected abstract ValueTask OnNextAsyncCore(T value);

        private void TryEnter()
        {
            var old = Interlocked.CompareExchange(ref _status, Busy, Idle);

            switch (old)
            {
                case Busy:
                    throw new InvalidOperationException("The observer is currently processing a notification.");
                case Done:
                    throw new InvalidOperationException("The observer has already terminated.");
            }
        }
    }
}
