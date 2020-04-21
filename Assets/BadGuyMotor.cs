using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BadGuyMotor : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private GameSceneManager mgr;
    public float movementSpeed = 10f;
    public int damageToPlant = 10;
    private bool isChasing = true;
    private bool entering = true;
    private float maxSpeed = 6.4f;
    public bool isFlying = true;
    private float yTarget = 2f;
    private float startDirection = -1f;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mgr = GameSceneManager.Instance;
        animator.SetBool("IsEntering", true);
        if (gameObject.name == "Slug") {
            rigidbody.isKinematic = true;
        } else {
            AnimationComplete();
        }
        Transform target = mgr.GetTarget();
        float targetX = target.position.x;
        startDirection = targetX < transform.position.x ? -1f : 1f;
        if (startDirection >= 0f) {
            spriteRenderer.flipX = true;
        }

    }

    public void Die() {
        animator.SetBool("IsDead", true);
        movementSpeed = 0f;
        isChasing = false;
        gameObject.layer = LayerMask.NameToLayer("TheDead");
        EventManager.Instance.ReportBadGuyDied(transform.position);
        if (isFlying) {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            GetComponent<Rigidbody2D>().gravityScale = -0.25f;
        }
    }

    void FixedUpdate() {
        if (isFlying && !isChasing && movementSpeed == 0f) {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        if (isChasing && !entering) {
            Transform target = mgr.GetTarget();
            float targetX = target.position.x;
            bool goLeft = targetX < transform.position.x;
            float force = (goLeft ? -1 : 1) * movementSpeed * 10;
            

            if (isFlying) {
                force = startDirection * movementSpeed * 10f;
            } else {
                spriteRenderer.flipX = !goLeft;
            }
            if (rigidbody.velocity.magnitude < maxSpeed) {
                rigidbody.AddForce(new Vector2(force, 0.01f));
            }

        }
    }

    void Despawn() {
        gameObject.layer = LayerMask.NameToLayer("TheDead");
        isChasing = false;
        movementSpeed = 0f;
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("IsDead", true);

    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (LayerMask.LayerToName(coll.gameObject.layer) == "Plant") {
            Despawn();
            GameSceneManager.Instance.ReportBadGuyDamagedPlant(damageToPlant);
            EventManager.Instance.ReportBadGuyDealtDamage();
        }
    }
    public void AnimationComplete() {
        StartCoroutine(Activate());
    }

    IEnumerator Activate() {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("IsEntering", false);
        entering = false;
        rigidbody.isKinematic = false;
    }
}
