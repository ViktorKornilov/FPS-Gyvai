using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Health health;
    

    private void Awake()
    {
        health = GetComponent<Health>();
        health.onDamage.AddListener(OnDamage);
    }

    void OnDamage()
    {
        print(":(");
    }
}
