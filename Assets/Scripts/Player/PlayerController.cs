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

    private Rigidbody rigidbody;
    private CharacterStatsHandler characterStats;

    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.Log("Rigidbody È¹µæ ½ÇÆÐ!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (!TryGetComponent<CharacterStatsHandler>(out characterStats))
        {
            Debug.Log("CharacterStatsHandler È¹µæ ½ÇÆÐ!");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void Move()
    {
        Vector3 dir = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);

        dir *= characterStats.CurrentStat.moveSO.moveSpeed;

        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        MoveSO move = characterStats.CurrentStat.moveSO;

        camCurXRot += mouseDelta.y * move.lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, move.minXLook, move.maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f);

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * move.lookSensitivity, 0f);
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
            if (Physics.Raycast(rays[i], 0.1f, characterStats.CurrentStat.moveSO.groundLayerMask))
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
        if ((InputActionPhase.Started == context.phase) && IsGrounded())
            rigidbody.AddForce(Vector2.up * characterStats.CurrentStat.moveSO.jumpPower, ForceMode.Impulse);
    }
}
