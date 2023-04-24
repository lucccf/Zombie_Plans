using System.Collections.Generic;
using UnityEngine;

public class Facility : BasicCharacter
{
    public long id;
    public Dictionary<int, int> materials;
    public Dictionary<int, int> commited;
    public int cond;
    public bool repaired;
    public bool buff;

    // Start is called before the first frame update
    void Start()
    {
        repaired= false;
        buff = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
