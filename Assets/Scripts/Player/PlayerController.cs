using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IJumpable
{
    [SerializeField] private Transform cameraContainer;

    private Vector2 curMovementInput;

    private Vector2 mouseDelta;
    private float camCurXRot;

    private PlayerCondition condition;
    private Rigidbody rigidbody;
    private MoveSO moveData;

    public event Action OnInventoryEvent;

    private bool canLook = true;
    private bool isFirst = true;

    public float AddSpeed { get; set; } = 0f;

    public MovingFloor Movingfloor { get; set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveData = CharacterManager.Instance.MainPlayer.MoveData;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    private void Move()
    {
        Vector3 dir;

        if (!IsOnWall())
        {
            dir = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);

            dir *= (moveData.moveSpeed + AddSpeed);

            dir.y = rigidbody.velocity.y;
        }
        else
        {
            dir = (transform.up * curMovementInput.y) + (transform.right * curMovementInput.x);

            dir *= (moveData.moveSpeed + AddSpeed);

            dir.z = rigidbody.velocity.z;
        }

        if (null != Movingfloor)
            transform.position += Movingfloor.GetMoveDirection();

        rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * moveData.lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, moveData.minXLook, moveData.maxXLook);

        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * moveData.lookSensitivity, 0f);

        if (isFirst)
        {
            cameraContainer.position = transform.position - (cameraContainer.rotation * new Vector3(0f, 0f, -0.3f));
            cameraContainer.position += new Vector3(0f, 1.1f, 0f);
        }
        else
        {
            cameraContainer.position = transform.position - (cameraContainer.rotation * new Vector3(0f, 0f, 2f));
            cameraContainer.position += new Vector3(0f, 1f, 0f);
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.forward * -0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * -0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; ++i)
        {
            if (Physics.Raycast(rays[i], 0.1f, moveData.groundLayerMask))
                return true;
        }

        return false;
    }

    private bool IsOnWall()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), transform.forward),
            new Ray(transform.position + (transform.forward * -0.2f) + (transform.up * 0.01f), -transform.forward),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), transform.right),
            new Ray(transform.position + (transform.right * -0.2f) + (transform.up * 0.01f), -transform.right)
        };

        for (int i = 0; i < rays.Length; ++i)
        {
            if (Physics.Raycast(rays[i], 0.5f, moveData.wallLayerMask))
                return true;
        }

        return false;
    }

    public void Jumping(float jumpPower)
    {
        rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (InputActionPhase.Performed == context.phase)
            curMovementInput = context.ReadValue<Vector2>().normalized;
        else if (InputActionPhase.Canceled == context.phase)
            curMovementInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if ((InputActionPhase.Started == context.phase) && IsGrounded() && condition.UseStemina(moveData.jumpUseStemina))
            rigidbody.AddForce(Vector2.up * moveData.jumpPower, ForceMode.Impulse);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (InputActionPhase.Started == context.phase)
        {
            OnInventoryEvent?.Invoke();
            canLook = !GameManager.Instance.ToggleCursor();
        }
    }

    public void OnChangeView(InputAction.CallbackContext context)
    {
        if (InputActionPhase.Started == context.phase)
            isFirst = !isFirst;
    }
}
