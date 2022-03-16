using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MMAudio : MonoBehaviour
{

    public UnityEvent unityEvent;

    // Start is called before the first frame update
    void Start()
    {
        unityEvent.Invoke();
    }

}
