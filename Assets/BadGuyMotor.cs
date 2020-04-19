using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BadGuyMotor : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameSceneManager mgr;
    public float movementSpeed = 10f;
    private bool isDead = false;

    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mgr = GameSceneManager.Instance;
    }

    public void Die() {
        animator.SetBool("IsDead", true);
        movementSpeed = 0f;
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("TheDead");
        EventManager.Instance.ReportBadGuyDied(transform.position);
    }

    void FixedUpdate() {
        if (!isDead) {
            float targetX = mgr.GetTarget().position.x;
            bool goLeft = targetX < transform.position.x;
            spriteRenderer.flipX = !goLeft;
            float force = (goLeft ? -1 : 1) * movementSpeed * 10;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0.1f));
        }
    }
}
