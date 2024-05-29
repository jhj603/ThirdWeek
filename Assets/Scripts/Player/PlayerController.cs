using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

    public float AddSpeed { get; set; } = 0f;

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
        Vector3 dir = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);

        dir *= (moveData.moveSpeed + AddSpeed);

        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * moveData.lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, moveData.minXLook, moveData.maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * moveData.lookSensitivity, 0f);
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
        if ((InputActionPhase.Started == context.phase) && condition.UseStemina(moveData.jumpUseStemina) && IsGrounded())
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
}
