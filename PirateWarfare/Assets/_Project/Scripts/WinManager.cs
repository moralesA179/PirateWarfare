using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{

    public TMP_Text winText;
    public float maxRotation = 30f;
    public int rotationDegs = 3;
    private bool goLeft = true, scaleUp = true;
    public float maxScale = 1.5f, minScale = 0.5f;
    // Update is called once per frame
    
    void Update()
    {
        //if it exceeds 180 we subtract 360 to make the angle effectively negative so the bounce code can work
        float currentRotation = winText.transform.eulerAngles.z > 180 ? winText.transform.eulerAngles.z - 360f : winText.transform.eulerAngles.z;
        if (winText.transform.localScale.x >= maxScale)
            scaleUp = false;
        if (winText.transform.localScale.x <= minScale)
            scaleUp = true;
        //Debug.Log(currentRotation);
        //bouncing between positive and negative bounds...
        if (currentRotation > maxRotation)
        {
            goLeft = false;
            Debug.Log($"Went left too much!");
        }
        if (currentRotation < -maxRotation)
        {
            Debug.Log("Went right too much");
            goLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
            SceneManager.LoadScene("MainMenu");
        winText.transform.Rotate(Vector3.forward, goLeft ? rotationDegs * Time.deltaTime : -rotationDegs * Time.deltaTime);
        winText.transform.localScale += scaleUp ? new Vector3(0.1f * Time.deltaTime, 0.1f * Time.deltaTime) : new Vector3(-0.1f * Time.deltaTime, -0.1f * Time.deltaTime);




    }
}
