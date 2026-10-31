using UnityEngine;

public class DamageTextMechanics : MonoBehaviour
{
    public int lifeTime = 100;
    public int speed = 3;
    public float scaleFactor = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Vector3.up * Time.deltaTime;
        if (transform.localScale.x > 0 && transform.localScale.y > 0) //make sure scale never becomes zero...
            transform.localScale -= new Vector3(scaleFactor, scaleFactor, 0) * Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
        lifeTime--;
        //Debug.Log($"Scale: {transform.localScale.x}, {transform.localScale.y}");
    }
}
