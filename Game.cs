using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class Game : MonoBehaviour
{
    public float walkingSpeedx = 7.5f;
    public float walkingSpeedy = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Camera Camera;
    private Animator animator;
    public Game gameScript;
    public GameObject Body;
    public Transform map;
    public Transform playert;
    public float distance;
    public GameObject TreeFX;
    public AudioSource audioSource;
    public AudioClip cuttingTree;
    public AudioClip fallingTree;
    public TextMeshProUGUI hpShow;
    public RaycastHit hit;
    public Inventory inventoryScript;
    public GameObject StorageUI;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if (!view.IsMine)
        {
            Camera.gameObject.SetActive(false);
            gameScript.enabled = false;
            Body.SetActive(true);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeedx) * Input.GetAxis("Vertical")   : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeedy) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        animator.SetFloat("x", Input.GetAxis("Horizontal"));
        animator.SetFloat("y", Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            animator.SetBool("Jump", false);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            animator.SetBool("Jump", true);

        }

        if (characterController.isGrounded)
        {
            animator.SetBool("Jump", false);
        }

        characterController.Move(moveDirection * Time.deltaTime);


        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.GetKey(KeyCode.W) & isRunning == true)
        {
            animator.SetBool("Run", true);
        }

        if (isRunning == false || Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("Run", false);
        }


        

        

        // Пускает Raycast
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, distance))
        {
            if (hit.collider.tag == "Tree1")
            {
                hpShow.text = hit.collider.GetComponent<HealthObject>().health + "/5";
                hpShow.gameObject.SetActive(true);
            }
            if (hit.collider.tag == "Tree")
            {
                hpShow.text = hit.collider.GetComponent<HealthObject>().health + "/15";
                hpShow.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.tag == "Item" | hit.collider.tag == "Storage")
                {
                    inventoryScript.dataItem = hit.transform.GetComponent<Item>().data;
                    inventoryScript.objecthit = hit.transform.gameObject;
                    inventoryScript.AddItem();
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (hit.collider.tag == "Storage")
                {
                    for (int i = 0; i < hit.collider.GetComponent<StorageScript>().itemScript.Length; i++)
                    {
                        inventoryScript.storageSlots[i].GetComponent<Slot>().item = hit.collider.GetComponent<StorageScript>().itemScript[i];
                        inventoryScript.storageSlots[i].GetComponent<Slot>().setNewItem();
                    }
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    StorageUI.SetActive(true);
                }
            }
        }
        else
        {
            hpShow.gameObject.SetActive(false);
        }

        

        
    }
    //Ивент для удара
    public void Hit()
    {
        {
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, distance))
            {
                if (hit.transform.tag == "Tree1" || hit.transform.tag == "Tree")
                {
                    if (inventoryScript.id == 3)
                    {
                        hit.collider.GetComponent<HealthObject>().health = hit.collider.GetComponent<HealthObject>().health - 2;
                        Instantiate(TreeFX, hit.point, Quaternion.Euler(hit.normal));
                        audioSource.PlayOneShot(cuttingTree);
                        if (hit.collider.GetComponent<HealthObject>().health <= 0)
                        {
                            hit.collider.GetComponent<HealthObject>().health = 0;
                            hit.collider.GetComponent<HealthObject>().Fall();
                            audioSource.PlayOneShot(fallingTree);
                        }   
                    }
                    
                }
            }
                
        }
    }

}