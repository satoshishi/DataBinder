using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Binder;

public class DataBinderSample : MonoBehaviour,IDataReceiver<int>
{
    public DataBinder<int> binder;

    // Start is called before the first frame update
    void Start()
    {
        binder = new DataBinder<int>();
        //binder.Subscribe((val)=>Debug.Log(val));
        binder.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
            binder.Value += 1;

        if(Input.GetKeyUp(KeyCode.S))
            binder.Dispose();
    }

    public void OnNext(int val)
    {
        Debug.Log(val);
    }
}
