using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ctrl : MonoBehaviour
{
    public Main_ctrl.objtype type;

    public long id;

    public enum class_name
    {
        Fix_rig2d,
        Fix_col2d,
        Player,
        Moster,
        Artical,
        Attack,
        Bag,
        Trigger,
        Facility,
        Tinymap,
        Tinybutton,
        //...
    }

    public Dictionary<class_name, object> modules = new Dictionary<class_name, object>();
}
