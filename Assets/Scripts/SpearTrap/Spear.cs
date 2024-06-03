using System.Collections;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float shootPower;

    public bool IsShoot { get; private set; } = false;

    private bool isEnd = false;

    private Coroutine updownCor;

    private void Update()
    {
        if (isEnd)
        {
            StopCoroutine(updownCor);
            isEnd = false;
        }
    }

    public void ShootUp()
    {
        if (!isEnd)
            IsShoot = true;

        updownCor = StartCoroutine(UpCoroutine());
    }

    public void GoReady()
    {
        if (!isEnd)
            IsShoot = false;

        updownCor = StartCoroutine(DownCoroutine());
    }

    private IEnumerator UpCoroutine()
    {
        while (1f > transform.position.y)
        {
            transform.position += Vector3.up * shootPower * Time.fixedDeltaTime;

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

        isEnd = true;
    }

    private IEnumerator DownCoroutine()
    {
        while (0f < transform.position.y)
        {
            transform.position += Vector3.down * shootPower * Time.fixedDeltaTime;

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        isEnd = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}