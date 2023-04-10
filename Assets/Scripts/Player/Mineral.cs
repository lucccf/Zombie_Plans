using UnityEngine;

public class Mineral : Monster
{
    // Start is called before the first frame update
    void Start()
    {
        CharacterType = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Updatex()
    {
        float tmp = 0;
        GetHited(ref tmp);
        if(status.death == true)
        {
            DeathFall("Mineral", 10, 1f);
            Main_ctrl.Desobj(id);
        }
    }
}
 