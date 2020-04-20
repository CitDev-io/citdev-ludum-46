using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Animator animator;
    public int damage = 10;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ProjectileDestructible pd = col.collider.gameObject.GetComponent<ProjectileDestructible>();
        if (pd != null) {
            pd.TakeDamage(damage);
        }
        animator.SetBool("IsPopped", true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void SetDamage(int dmg) {
        damage = dmg;
    }

    public void DiscardProjectile() {
        Destroy(gameObject);
    }
}
