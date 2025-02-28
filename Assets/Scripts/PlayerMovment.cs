using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    public FixedJoystick movementJoystick;
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;

    private CharacterController controller;
    private ItemInteraction itemInteraction;

    [Inject]
    private void Construct(ItemInteraction _itemInteraction)
    {
        itemInteraction = _itemInteraction;
    }

    private float cameraYRotation = 0f;
    public float minCameraAngle = -60f; 
    public float maxCameraAngle = 60f; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleItemInteraction();
    }

    void HandleMovement()
    {
        if (controller != null)
        {
            Vector3 moveDirection = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    void HandleLook()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    float deltaX = touch.deltaPosition.x * lookSensitivity * Time.deltaTime;
                    float deltaY = touch.deltaPosition.y * lookSensitivity * Time.deltaTime;
                    transform.Rotate(Vector3.up, deltaX); 

                    cameraYRotation -= deltaY;
                    cameraYRotation = Mathf.Clamp(cameraYRotation, minCameraAngle, maxCameraAngle);

                    Camera.main.transform.localEulerAngles = new Vector3(cameraYRotation, 0, 0);
                }
            }
        }
    }

    void HandleItemInteraction()
    {
        if (itemInteraction != null)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2 && touch.phase == TouchPhase.Began)
                {
                    itemInteraction.PickUpItem();
                }
            }
        }
    }
}