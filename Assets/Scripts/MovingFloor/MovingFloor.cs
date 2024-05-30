using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingFloor : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float moveSpeed;

    private Vector3 moveDirection;
    private bool isGoEnd = true;

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (isGoEnd && (0.1f > (endPos - transform.position).magnitude))
            isGoEnd = false;
        else if (!isGoEnd && (0.1f > (startPos - transform.position).magnitude))
            isGoEnd = true;
    }

    private void Move()
    {
        if (isGoEnd)
            moveDirection = (endPos - transform.position).normalized;
        else
            moveDirection = (startPos - transform.position).normalized;

        moveDirection *= (moveSpeed * Time.fixedDeltaTime);

        transform.position += moveDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsLayerMatched(playerLayer.value, collision.gameObject.layer))
        {
            collision.gameObject.GetComponent<PlayerController>().Movingfloor = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsLayerMatched(playerLayer.value, collision.gameObject.layer))
        {
            collision.gameObject.GetComponent<PlayerController>().Movingfloor = null;
        }
    }

    private bool IsLayerMatched(int value, int layer)
    {
        return value == (value | 1 << layer);
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}