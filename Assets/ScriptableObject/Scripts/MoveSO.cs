using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMoveSO", menuName = "Controller/Moves/Default", order = 0)]
public class MoveSO : ScriptableObject
{
    [Header("Movement")]
    [Range(1f, 20f)] public float moveSpeed;
    [Range(1f, 100f)] public float jumpPower;
    [Range(1f, 100f)] public float jumpUseStemina;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;

    [Header("Look")]
    [Range(-100f, 100f)] public float minXLook;
    [Range(-100f, 100f)] public float maxXLook;
    [Range(0f, 1f)] public float lookSensitivity;
}
