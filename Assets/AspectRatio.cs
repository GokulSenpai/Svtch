using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    public Vector2 aspectRatio;
    public bool fullscreen;

    private void Start()
    {
        SetRatio(aspectRatio.x, aspectRatio.y);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetRatio(float w, float h)
    {
        if (Screen.width / (float)Screen.height > w / h)
        {
            Screen.SetResolution((int)(Screen.height * (w / h)), Screen.height, fullscreen);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(Screen.width * (h / w)), fullscreen);
        }
    }
}
