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

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField]
    public Sprite withPlantNoAnimationSprite;
    [SerializeField]
    public Sprite withoutPlantNoAnimationSprite;
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        // animator = GetComponent<Animator> ();
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
        if (grounded) {
            DropPlant();
            return;
        }

        OnDropPlantFailure?.Invoke();
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
    }

    void PickUpPlant() {
        carryingPlant = true;
        OnPickupPlantSuccess?.Invoke();
        spriteRenderer.sprite = withPlantNoAnimationSprite;
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


        if (Mathf.Abs(move.x) > 0.025f) {
            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite) 
            {
                OnChangeDirection?.Invoke(!spriteRenderer.flipX);
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        

        // animator.SetBool ("grounded", grounded);
        // animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}