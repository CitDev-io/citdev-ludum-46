using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {
    // public NoParamDelegate OnStartRunning;
    // public NoParamDelegate OnStopRunning;
    public NoParamDelegate OnJumpSuccessful;
    public NoParamDelegate OnJumpFailed;
    // public NoParamDelegate OnLanding;
    // public BoolDelegate OnShootSuccess;
    // public NoParamDelegate OnShootFailure;
    public Vector3Delegate OnDropPlantSuccess;
    public NoParamDelegate OnDropPlantFailure;
    public NoParamDelegate OnPickupPlantSuccess;
    public NoParamDelegate OnPickupPlantFailure;
    public BoolDelegate OnChangeDirection;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private bool carryingPlant = true;
    private Coroutine firingSequence;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField]
    public Sprite withPlantNoAnimationSprite;
    [SerializeField]
    public Sprite withoutPlantNoAnimationSprite;

    private float droppedX;
    private float droppedY;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletOrigin;
    public float minimumDistanceToGetPlant = 1.5f;
    public float bulletSpeed = 100f;
    public float bulletRate = 200f;

    private bool isFiring = false;

    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        animator = GetComponent<Animator> ();
    }

    void Update() {
        base.Update();
        if (Input.GetButtonDown("UsePlant")) {
            if (carryingPlant) {
                AttemptToDropPlant();
            } else {
                AttemptToPickUpPlant();
            }
        }
    }

    void AttemptToDropPlant() {
        // TODO: Check if this is a good time/place?
        DropPlant();
    }

    void AttemptToPickUpPlant() {
        // TODO: Be in range?
        // if (grounded) {
        PickUpPlant();
        //     return;
        // }

        //OnPickupPlantFailure?.Invoke();
    }

    void DropPlant() {
        carryingPlant = false;
        OnDropPlantSuccess?.Invoke(transform.position);
        spriteRenderer.sprite = withoutPlantNoAnimationSprite;
        droppedX = transform.position.x;
        droppedY = transform.position.y;
    }

    void PickUpPlant() {
        bool closeEnoughX = Mathf.Abs(transform.position.x - droppedX) < minimumDistanceToGetPlant;
        bool closeEnoughY = grounded || transform.position.y < minimumDistanceToGetPlant;

        if (closeEnoughX && closeEnoughY) {
            carryingPlant = true;
            OnPickupPlantSuccess?.Invoke();
            spriteRenderer.sprite = withPlantNoAnimationSprite;
        } else {
            OnPickupPlantFailure?.Invoke();
        }
    }

    IEnumerator FireIfShooting()
    {
        var key = Random.Range(0,600);
        while(true) 
         { 
             isFiring = true;
            GameObject bulletClone = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2((spriteRenderer.flipX ? -1f : 1f) * bulletSpeed, 0f);
            yield return new WaitForSeconds(1f/bulletRate);
         }
     }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump")) {
            if (grounded) {
                velocity.y = jumpTakeOffSpeed;
                OnJumpSuccessful?.Invoke();
            } else {
                OnJumpSuccessful?.Invoke();
            }
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            if (firingSequence != null) {
                StopCoroutine(firingSequence);
                isFiring = false;
            }
            firingSequence = StartCoroutine(FireIfShooting());
            isFiring = true;
        }
        if (Input.GetButtonUp("Fire1")) {
            if (firingSequence != null) {
                StopCoroutine(firingSequence);
                isFiring = false;
            }
        }

        animator.SetBool("IsWalking", Mathf.Abs(move.x) > 0f);

        if (Mathf.Abs(move.x) > 0.025f) {
            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite) 
            {
                OnChangeDirection?.Invoke(!spriteRenderer.flipX);
                spriteRenderer.flipX = !spriteRenderer.flipX;
                bulletOrigin.transform.localPosition = new Vector3(Mathf.Abs(bulletOrigin.transform.localPosition.x) * (spriteRenderer.flipX ? -1 : 1), bulletOrigin.transform.localPosition.y, bulletOrigin.transform.localPosition.z);
            }
        }
        

        // animator.SetBool ("grounded", grounded);
        // animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
        float recoil = isFiring ? maxSpeed / 2f : maxSpeed;

        targetVelocity = move * recoil;
    }
}