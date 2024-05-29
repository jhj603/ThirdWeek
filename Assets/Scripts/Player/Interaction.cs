using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private LayerMask layerMask;

    private float checkRate = 0.05f;
    private float lastCheckTime;
    private GameObject curInteractGameObject;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (checkRate < (Time.time - lastCheckTime))
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, layerMask))
            {
                if (curInteractGameObject != hit.collider.gameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractGameObject.GetComponent<ItemObject>().OnPrompt();
                }
            }
            else
            {
                if (null != curInteractGameObject)
                    curInteractGameObject.GetComponent<ItemObject>().OffPrompt();

                curInteractGameObject = null;
            }
        }
    }
}