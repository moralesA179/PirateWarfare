using UnityEngine;
using UnityEngine.UIElements;

public class BasicProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(100f, 1000f)]
    public float lifeTime;

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
            lifeTime--;
        else
            Destroy(gameObject);
    }
}
