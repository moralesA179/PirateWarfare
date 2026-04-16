using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempShopTester : MonoBehaviour
{

    public static List<int> currentTypes = new();
    public static List<BasicProjectile> currentProjectiles = new();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        { 
            SceneManager.LoadScene("Shop&Items");
            
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            PlayerData.score += 100;
        }
    }
}
