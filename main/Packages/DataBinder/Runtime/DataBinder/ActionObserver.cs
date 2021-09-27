using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{
    public class ActionObserver<T> : IObserver<T>
    {
        private Action<T> onNext { get; set; }

        private Action<Exception> onError { get; set; }

        private Action onCompleted { get; set; }

        public ActionObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public ActionObserver(Action<T> onNext)
        {
            this.onNext = onNext;
        }

        public ActionObserver(Action<T> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
        }

        public void OnCompleted()
        {
            if (onCompleted != null)
                onCompleted();

            onNext = null;
            onError = null;
            onCompleted = null;
        }

        public void OnError(Exception error)
        {
            if (onError != null)
                onError(error);
        }

        public void OnNext(T value)
        {
            if (onNext != null)
                onNext(value);
        }
    }
}