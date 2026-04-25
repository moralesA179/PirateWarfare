using UnityEngine;

public class Resolution : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }
}