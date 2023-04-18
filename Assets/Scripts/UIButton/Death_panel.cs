using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_panel : MonoBehaviour
{
    // Start is called before the first frame update
    public void next()
    {
        Flow_path.next();
    }

    public void pre()
    {
        Flow_path.pre();
    }
}
