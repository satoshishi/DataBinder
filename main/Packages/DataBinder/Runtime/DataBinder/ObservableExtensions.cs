using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            return source.Subscribe(new ActionObserver<T>(onNext));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
        {
            return source.Subscribe(new ActionObserver<T>(onNext, onCompleted));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return source.Subscribe(new ActionObserver<T>(onNext, onError, onCompleted));
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, IDataReceiver<T> receiver)
        {
            return source.Subscribe(new DataReceiveObserver<T>(receiver));
        }

        public static TranslatedDataBinder<FROM, TO> AsTranslatedDataBinder<FROM, TO>(this IObservable<FROM> source, IDataTranslator<FROM, TO> translator)
        {
            return new TranslatedDataBinder<FROM, TO>(translator, source);
        }

        public static TranslatedDataBinder<FROM, TO> AsTranslatedDataBinder<FROM, TO>(this IObservable<FROM> source, Func<FROM, TO> func)
        {
            return new TranslatedDataBinder<FROM, TO>(new FuncDataTranslator<FROM, TO>(func), source);
        }
    }
}