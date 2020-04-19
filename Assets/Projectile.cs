using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
        ProjectileDestructible pd = col.collider.gameObject.GetComponent<ProjectileDestructible>();
        if (pd != null) {
            pd.TakeDamage(damage);
        }
    }

    public void SetDamage(int dmg) {
        damage = dmg;
    }
}
