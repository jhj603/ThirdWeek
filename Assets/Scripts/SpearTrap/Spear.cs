using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float shootPower;

    private Rigidbody rigidbody;

    public bool IsShoot { get; private set; } = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (IsShoot)
        {
            rigidbody.AddForce(Vector2.up * shootPower, ForceMode.Impulse);
        }
    }

    public void ShootUp()
    {
        IsShoot = true;
    }

    public void GoReady()
    {
        IsShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}