using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{
    public interface IDataReceiver<T>
    {
        void OnNext(T value);
    }

    internal sealed class DataReceiveObserver<T> : IObserver<T>
    {
        private IDataReceiver<T> receiver;

        public DataReceiveObserver(IDataReceiver<T> receiver)
        {
            this.receiver = receiver;
        }

        public void OnCompleted()
        {
            receiver = null;
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            receiver?.OnNext(value);
        }
    }
}