using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestructible : MonoBehaviour
{
    public int maxHitPoints = 100;
    public int hitPoints = 0;

    void Awake() {
        hitPoints = maxHitPoints;
    }

    public void TakeDamage(int damage) {
        hitPoints -= damage;

        if (hitPoints <= 0) {
            HandleDeath();
        }
    }

    void HandleDeath() {
        BadGuyMotor bgm = GetComponent<BadGuyMotor>();
        if (bgm != null) {
            bgm.Die();
        }
    }
}
