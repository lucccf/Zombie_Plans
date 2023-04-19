using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour, IPointerClickHandler
{
    public NewBag bag;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            bag.MouseButton = 0;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            bag.MouseButton = 1;
        }
    }
}
