using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public Image icon;
    public Image bg;
    public bool isBusy;
    public bool isUsing;
    public bool canUse;
    public int idItem;
    
    public ItemData item;
    

    public void Start()
    {
        if (canUse == false)
        {
            bg.color = new Color32(255, 0, 0, 100);
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("pon");
    }

    public void setNewItem()
    {
        isBusy = true;
        icon.color = new Color32(255, 255, 225, 255);
        icon.sprite = item.icon;
        idItem = item.id;
    }
    public void deleteItem()
    {
        isBusy = false;
        icon.color = new Color32(255, 255, 225, 0);
        icon.sprite = null;
        item = null;
        idItem = 0;
    }
}
