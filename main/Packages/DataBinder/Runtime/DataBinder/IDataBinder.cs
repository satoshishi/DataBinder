using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Binder
{

    public interface IDataBinder<T>
    {
        T Value { get; set; }
    }

    public interface IReadonlyDataBinder<T>
    {
        T Value { get; }
    }
}