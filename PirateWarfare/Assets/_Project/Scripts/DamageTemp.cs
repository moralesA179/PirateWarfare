using UnityEngine;
using UnityEngine.UI;

public class DamageTemp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0, 100)]
    public int damage = 50;

    public Button button;
    public void Start()
    {
        button.onClick.AddListener(() => { PlayerData.TakeDamage(damage); });
    }
}
