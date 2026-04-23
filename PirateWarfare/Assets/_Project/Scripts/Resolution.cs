using UnityEngine;

public class Resolution : MonoBehaviour
{
    void Awake()
    {
        // Forces 1920x1080 in Borderless Windowed mode
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }
}