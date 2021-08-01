using System;

namespace Binder
{
    public class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Singleton = new EmptyDisposable();

        public void Dispose()
        {

        }
    }
}
