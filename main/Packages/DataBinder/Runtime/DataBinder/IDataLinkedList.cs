using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{
    internal interface IDataLinkedList<T>
    {
        void UnSubscribe(DataNode<T> node);        
    }
}