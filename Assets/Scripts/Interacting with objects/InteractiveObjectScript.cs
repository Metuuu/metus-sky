using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObjectScript : MonoBehaviour {

    // Enums
    public enum type { Info, Button, Switch, Grabbable, Pickup }

    #region [ - Init - ]
    public type objectType;
    
    // info
    [Multiline] public string info;

    // button
    public UnityEvent buttonEvent;

    // switch
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    #endregion


}
