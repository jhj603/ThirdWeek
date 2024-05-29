using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public bool ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        return toggle;
    }
}
