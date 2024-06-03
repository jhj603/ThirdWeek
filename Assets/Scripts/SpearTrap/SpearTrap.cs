using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    [SerializeField] private Spear Spears;
    [SerializeField] private LayerMask layerMask;

    private float checkRate = 0.05f;
    private float lastCheckTime;

    private void Update()
    {
        if (checkRate < (Time.time - lastCheckTime))
        {
            lastCheckTime = Time.time;

            if (Physics.SphereCast(transform.position, 1.5f, transform.up, out RaycastHit hit, 10f, layerMask))
            {
                if (!Spears.IsShoot)
                    Spears.ShootUp();
            }
            else if (Spears.IsShoot)
            {
                Spears.GoReady();
            }
        }
    }
}
