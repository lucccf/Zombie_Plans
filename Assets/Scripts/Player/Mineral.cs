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
        bool death = GetHited();
        if(death == true)
        {
            Main_ctrl.NewItem(f.pos.Clone(), "Mineral", 10,1f);
            Main_ctrl.Desobj(id);
        }
    }

    private bool GetHited()
    {
        while (((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Count > 0)
        {
            Fix_col2d_act a = ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Peek();
            ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
            if (a.type != Fix_col2d_act.col_action.Attack) continue;
            long AttackId = a.opsite.id;
            if (!Main_ctrl.All_objs.ContainsKey(AttackId)) continue;
            Attack attack = (Attack)(Main_ctrl.All_objs[AttackId].modules[Object_ctrl.class_name.Attack]);
            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;
            status.GetAttacked(HpDamage, ToughnessDamage);

            GameObject beat = (GameObject)AB.getobj("beat");
            beat.transform.localScale = new Vector3(3f, 3f, 1f);
            Instantiate(beat, transform.position, transform.rotation);
            GameObject num = (GameObject)AB.getobj("HurtNumber");
            GameObject num2 = Instantiate(num, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            num2.GetComponent<BeatNumber>().ChangeNumber(attack.HpDamage.to_int());

            if (attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                attack2.DestroySelf();
            }
        }
        if (status.death == true) return true;
        else return false;
    }
}
