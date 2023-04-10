using Net;
using UnityEngine;
using UnityEngine.UI;

public class CommitButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(CommitMaterial);
    }

    void CommitMaterial() {
        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.FixFacility;
        x.Userid = (int)Main_ctrl.user_id;
        x.Itemid = (int)Flow_path.Now_fac;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
