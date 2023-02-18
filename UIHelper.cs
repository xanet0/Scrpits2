using UnityEngine;

public class UIHelper : MonoBehaviour
{

    public Game gameScript;
    public GameObject EscUI;
    public GameObject crosshair;
    public bool isOpen = false;
    public float TV = 0f;
    public float TV2 = 0f;
    public float TV3 = 0f;

    void Start()
    {
        TV = gameScript.lookSpeed;
        EscUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESC();
        }
    }

    void ESC()
    {
        if (isOpen == false)
        {
            TV = gameScript.lookSpeed;
            gameScript.lookSpeed = 0;
            TV2 = gameScript.walkingSpeedx;
            TV3 = gameScript.walkingSpeedy;
            EscUI.SetActive(true);
            crosshair.SetActive(false);
            isOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            gameScript.lookSpeed = TV;
            EscUI.SetActive(false);
            crosshair.SetActive(true);
            isOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


}
