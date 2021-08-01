using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

namespace Binder
{
    internal sealed class DataNode<T> : IObserver<T>, IDisposable
    {
        private IObserver<T> observer;

        private IDataLinkedList<T> list;

        public DataNode<T> Previous {get;internal set;}

        public DataNode<T> Next {get;internal set;}

        public DataNode(IObserver<T> observer,IDataLinkedList<T> list)
        {
            this.observer = observer;
            this.list = list;   
        }

        public void OnCompleted()
        {
            observer?.OnCompleted();
        }

        public void OnError(Exception error)
        {
            observer?.OnError(error);
        }

        public void OnNext(T value)
        {
            observer?.OnNext(value);
        }

        public DataNode<T> Dispose(bool unSubscribing = false)
        {
            this?.OnCompleted();     
            if(unSubscribing)
                list?.UnSubscribe(this);     

       
            observer = null;
            list = null;    

            var next = Next;
            Next = null;
            Previous = null;

            return next;      
        }

        public void Dispose()
        {
            this?.OnCompleted();
            list?.UnSubscribe(this);

            list = null;            
            observer = null;
            Next = null;
            Previous = null;
        }
    }
}