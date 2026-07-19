using UnityEngine;

public class DamageTarget : MonoBehaviour
{
    public float health = 100f;
    public bool destroyOnDeath = true;

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log(gameObject.name + " took damage: " + damage + ". Health: " + health);

        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died.");

        if (destroyOnDeath)
            Destroy(gameObject);
    }
}