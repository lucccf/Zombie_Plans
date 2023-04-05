using UnityEngine;

public class Mineral : Monster
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Updatex()
    {
        GetHited(1f);
        if(status.death == true)
        {
            DeathFall("Mineral", 10, 1f);
            Main_ctrl.Desobj(id);
        }
    }
}
