using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
public class FacilityUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    public long buildingid;
    public GameObject itemimage;
    public GameObject itemtext;
    public GameObject itemprogress;
    public GameObject itemprogresstext;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Facility fa = Flow_path.facilities[buildingid];
        Dictionary<int, int> curmat = fa.materials;
        foreach (KeyValuePair<int, int> mat in curmat)
        {
            Item x = Main_ctrl.GetItemById(mat.Key);
            itemimage.GetComponent<Image>().sprite = x.image;
            itemtext.GetComponent<Text>().text = "还需数量：" + (fa.materials[mat.Key] - fa.commited[mat.Key]);
            Debug.Log("这是啥" + fa.commited[mat.Key]);
            itemprogress.GetComponent<Image>().fillAmount = ((float)fa.commited[mat.Key] / (float)fa.materials[mat.Key]);
            itemprogresstext.gameObject.GetComponent<Text>().text = (fa.commited[mat.Key] * 100 / fa.materials[mat.Key]).ToString() + "%";
        }
    }
}
