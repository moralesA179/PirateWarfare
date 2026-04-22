using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public enum HealthState
    {
        Full = 0,
        Half = 1,
        Low = 2
    }

    int maxHealth = 100;
    int currentHealth = 100;
    Animator animator;

    public GameObject scrapPrefab;
    public TextMeshPro damageNum;
    [Range(0f, 1f)] public float dropChance = 0.3f;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            //TryDropScrap(); <-- We can try to edit this later...
            Destroy(gameObject);
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Modifies the ships sprite based on their health
        if (currentHealth <= (maxHealth * 0.25))
        {
            animator.SetInteger("HealthState", (int)HealthState.Low);
        }
        else if (currentHealth <= (maxHealth * 0.5))
        {
            animator.SetInteger("HealthState", (int)HealthState.Half);
        }
        else if (currentHealth == maxHealth)
        {
            animator.SetInteger("HealthState", (int)HealthState.Full);
        }

        Debug.Log("Enemy Current Health: " + currentHealth);

    }

    void TryDropScrap()
    {
        if (scrapPrefab != null && Random.value <= dropChance)
        {
            Instantiate(scrapPrefab, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Vector3 offset = new Vector3(Random.Range(0f, 1f), 0, 0);
        Quaternion randomRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-30f, 30f)));
        TextMeshPro element = Instantiate(damageNum, transform.position + offset, randomRotation);
        element.text = damage.ToString();
        
    }
}
