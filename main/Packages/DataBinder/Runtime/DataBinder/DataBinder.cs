using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{
    #region TranslatedDataBinder

    public interface IDataTranslator<FROM,TO>
    {
        TO Handle(FROM from);
    }

    internal sealed class FuncDataTranslator<FROM,TO> : IDataTranslator<FROM,TO>
    {
        private Func<FROM,TO> translateFunc;

        public FuncDataTranslator(Func<FROM,TO> translateFunc)
        {
            this.translateFunc = translateFunc;
        }

        public TO Handle(FROM from) => translateFunc(from);
    }

    public class TranslatedDataBinder<FROM, TO> : IReadonlyDataBinder<TO>, IObservable<TO>, IDataReceiver<FROM>, IDisposable
    {
        private DataBinder<TO> dataBinder;
        private IDisposable disposable;

        private IDataTranslator<FROM,TO> translator;

        public TO Value
        {
            get => dataBinder.Value;
        }

        public TranslatedDataBinder(IDataTranslator<FROM,TO> translator,IObservable<FROM> observable)
        {
            dataBinder = new DataBinder<TO>();
            disposable = observable.Subscribe(this);

            this.translator = translator;
        }

        public IDisposable Subscribe(IObserver<TO> observer)
        {
            return dataBinder.Subscribe(observer);
        }

        public void OnNext(FROM value)
        {
            dataBinder.Value = translator.Handle(value);
        }        

        public void Dispose()
        {
            disposable.Dispose();
            dataBinder.Dispose();
        }
    }

    #endregion

    #region DataBinder

    [System.Serializable]
    public class DataBinder<T> : IDataBinder<T>, IObservable<T>, IDataLinkedList<T>, IDisposable
    {
        [SerializeField]
        T value;

        protected IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

        DataNode<T> root;

        DataNode<T> last;

        protected bool isDisposed = false;

        public T Value
        {
            get => value;
            set
            {
                if (!EqualityComparer.Equals(this.value, value))
                {
                    this.value = value;
                    if (isDisposed) return;

                    RaiseOnNext(value);
                }
            }
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (isDisposed)
            {
                observer.OnCompleted();
                return EmptyDisposable.Singleton;
            }

            var next = new DataNode<T>(observer, this);
            if (root == null)
            {
                root = last = next;
            }
            else
            {
                last.Next = next;
                next.Previous = last;
                last = next;
            }
            return next;
        }

        void IDataLinkedList<T>.UnSubscribe(DataNode<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }
            if (node == last)
            {
                last = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        protected virtual void RaiseOnNext(T value)
        {
            var node = root;

            while (node != null)
            {
                node.OnNext(value);
                node = node.Next;
            }
        }

        public void Dispose()
        {
            if (isDisposed) return;

            var node = root;
            root = last = null;
            isDisposed = true;

            while (node != null)
            {
                node = node.Dispose(false);
            }

            GC.SuppressFinalize(this);
        }
    }

    #endregion
}