using UnityEngine;

public class ProjectileUpgradePickup : MonoBehaviour
{
    [Range(5, 10)] public int projectileIncrease = 7; //increase cannon shots to this number
    //pickup degrades and eventually is destroyed if not picked up in time
    public int lifetime = 500;

    // Update is called once per frame
    void Update()
    {
        if(lifetime <= 0)
            Destroy(gameObject);
        lifetime--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!PlayerData.superState) //if not in super state, then enter it 
                PlayerData.EnterSuperState(projectileIncrease);
            lifetime = 0;
        }
    }
}
