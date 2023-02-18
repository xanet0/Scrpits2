using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public GameObject[] Slots;
    public GameObject[] storageSlots;
    public ItemData dataItem;
    public GameObject objecthit;
    public GameObject showingForBuild;
    public GameObject Camera;
    public int id;
    public RaycastHit hit;
    public Material greenMaterial;
    public List<Color32> TemporaryColors;
    private bool nowBuild;
    private float yRotate, zRotate;
    [SerializeField] private GameObject inHand;
    [SerializeField] private Transform dropPos;
    [SerializeField] private int nowUsing;
    [SerializeField] private Transform handPos;
    [SerializeField] private Material canMove;
    [SerializeField] private Material cantMove;
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5 };

    private void Start()
    {
        Slots[0].GetComponent<Slot>().isUsing = true;
        Slots[0].GetComponent<Slot>().bg.color = new Color32(255, 255, 255, 200);
        id = Slots[0].GetComponent<Slot>().idItem;
        nowUsing = 0;
    }

    public void Update()
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, 5f))
        {
            if (Slots[nowUsing].GetComponent<Slot>().isBusy == true && Slots[nowUsing].GetComponent<Slot>().item.canBuild == true)
            {
                showingForBuild.SetActive(true);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (yRotate == 360)
                    {
                        if (zRotate >= 45)
                        {
                            showingForBuild.transform.rotation = Quaternion.Euler(showingForBuild.transform.eulerAngles + new Vector3(0, 45, 0));
                            zRotate += 45;
                            Debug.Log(zRotate);
                        }
                        if (zRotate < 45)
                        {
                            showingForBuild.transform.rotation = Quaternion.Euler(showingForBuild.transform.eulerAngles + new Vector3(0, 0, 45));
                            zRotate += 45;
                        }
                        if (zRotate >= 450)
                        {
                            zRotate = 0;
                            yRotate = 0;
                            showingForBuild.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    else if (yRotate < 360)
                    {
                        showingForBuild.transform.rotation = Quaternion.Euler(showingForBuild.transform.eulerAngles + new Vector3(0, 45, 0));
                        yRotate += 45;
                    }
                }

                if (hit.collider.tag == "Ground" || hit.collider.GetComponent<Item>() != null && hit.collider.GetComponent<Item>().data.canBuild == true)
                {
                    SetGreenColor();
                    showingForBuild.transform.position = hit.point;

                    if (Input.GetMouseButtonDown(1))
                    {
                        Vector3 buildRotate = showingForBuild.transform.eulerAngles;
                        Destroy(showingForBuild);
                        showingForBuild = null;
                        GameObject temp = Instantiate(Slots[nowUsing].GetComponent<Slot>().item.prefab, hit.point, Quaternion.Euler(buildRotate));
                        id = 0;
                        yRotate = 0;
                        zRotate = 0;
                        Slots[nowUsing].GetComponent<Slot>().deleteItem();
                        Destroy(inHand);
                    }
                }

                else
                {
                    SetRedColor();
                    showingForBuild.transform.position = hit.point;
                }
            }

        }

        else
        {
            if (Slots[nowUsing].GetComponent<Slot>().isBusy == true && Slots[nowUsing].GetComponent<Slot>().item.canBuild == true)
            {
                showingForBuild.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }



        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Slots[i].GetComponent<Slot>().canUse == true)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    Slots[nowUsing].GetComponent<Slot>().bg.color = new Color32(255, 255, 255, 100);
                    Slots[nowUsing].GetComponent<Slot>().isUsing = false;
                    Slots[i].GetComponent<Slot>().isUsing = true;
                    Slots[i].GetComponent<Slot>().bg.color = new Color32(255, 255, 255, 200);
                    id = Slots[i].GetComponent<Slot>().idItem;
                    
                    if (inHand == true) Destroy(inHand);  
                    if (showingForBuild != null) Destroy(showingForBuild);
                    if (Slots[i].GetComponent<Slot>().item != null && Slots[i].GetComponent<Slot>().item.canBuild == true)
                    {
                        showingForBuild = Instantiate(Slots[i].GetComponent<Slot>().item.prefab, hit.point, Quaternion.Euler(hit.normal));
                        showingForBuild.GetComponent<Collider>().enabled = false;
                    }
                    if (Slots[i].GetComponent<Slot>().item)
                    {
                        inHand = Instantiate(Slots[i].GetComponent<Slot>().item.showInHandPrefab, handPos);
                        inHand.GetComponent<Rigidbody>().isKinematic = true;
                        inHand.GetComponent<Collider>().enabled = false;
                    }
                    
                    nowUsing = i;
                }
            }
        }

    }
    public void AddItem()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slot temp = Slots[i].GetComponent<Slot>();
            if (temp.isBusy == false & temp.canUse == true)
            {
                Slots[i].GetComponent<Slot>().item = dataItem;
                Destroy(objecthit);
                Slots[i].GetComponent<Slot>().setNewItem();

                if (i == nowUsing)
                {
                    
                    id = Slots[i].GetComponent<Slot>().idItem;
                    inHand = Instantiate(Slots[i].GetComponent<Slot>().item.showInHandPrefab, handPos);
                    inHand.GetComponent<Rigidbody>().isKinematic = true;
                    inHand.GetComponent<Collider>().enabled = false;
                }
                if (i == nowUsing && Slots[i].GetComponent<Slot>().item != null && Slots[i].GetComponent<Slot>().item.canBuild == true)
                {
                    showingForBuild = Instantiate(Slots[i].GetComponent<Slot>().item.prefab, hit.point, Quaternion.Euler(hit.normal));
                    showingForBuild.GetComponent<Collider>().enabled = false;
                }
                break;
            }
        }
    }
    public void DropItem()
    {
        if (Slots[nowUsing].GetComponent<Slot>().isBusy)
        {
            Destroy(inHand);
            if (Slots[nowUsing].GetComponent<Slot>().item.canBuild == true)
            {
                Destroy(showingForBuild);
            }
            Instantiate(Slots[nowUsing].GetComponent<Slot>().item.prefab, dropPos.position, dropPos.rotation).GetComponent<Rigidbody>().isKinematic = false;
            Slots[nowUsing].GetComponent<Slot>().deleteItem();
            id = 0;
        }
    }

    public void SetGreenColor()
    {
        for (int i = 0; i < showingForBuild.GetComponentsInChildren<Renderer>().Length; i++)
        {
            Material[] f = showingForBuild.GetComponentsInChildren<Renderer>()[i].materials;
            for (int c = 0; c < f.Length; c++)
            {
                f[c] = canMove;
                showingForBuild.GetComponentsInChildren<Renderer>()[i].materials = f;
            }

        }
    }

    

        public void SetRedColor()
    {
        for (int i = 0; i < showingForBuild.GetComponentsInChildren<Renderer>().Length; i++)
        {
            Material[] f = showingForBuild.GetComponentsInChildren<Renderer>()[i].materials;
            for (int c = 0; c < f.Length; c++)
            {
                f[c] = cantMove;
                showingForBuild.GetComponentsInChildren<Renderer>()[i].materials = f;
            }

        }
    }   
}
