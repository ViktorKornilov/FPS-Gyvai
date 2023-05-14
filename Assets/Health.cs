using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxXp = 100;
    public int hp;

    public UnityEvent onDamage;
    public UnityEvent onDeath;

    private bool isDead;

    public bool autoDestroy = false;

    private void Start()
    {
        if (hp <= 0) hp = maxXp;
    }

    public void Damage(int amount)
    {
        hp -= amount;
        onDamage.Invoke();
        if (hp <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        
        onDeath.Invoke();
        if(autoDestroy)Destroy(gameObject);
    }
}
