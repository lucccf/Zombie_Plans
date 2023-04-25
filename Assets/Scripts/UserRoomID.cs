using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRoomID : MonoBehaviour
{
    // Start is called before the first frame update
    public Text RoomID;
    public Transform target;
    public BasicCharacter character;
    private Vector3 offset = new Vector3(0f, 1f, 0f);
    Vector3 screenPos;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        RoomID.transform.position = screenPos;
    }
}
