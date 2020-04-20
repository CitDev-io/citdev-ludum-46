using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {
    public DoubleIntDelegate OnGunChargeChange;
    public NoParamDelegate OnStartRunning;
    public NoParamDelegate OnStopRunning;
    public NoParamDelegate OnJumpSuccessful;
    public NoParamDelegate OnJumpFailed;
    public NoParamDelegate OnLanding;
    public NoParamDelegate OnShootStart;
    public NoParamDelegate OnShootEnd;
    public NoParamDelegate OnShootSuccess;
    public NoParamDelegate OnShootFailedNoEnergy;
    public NoParamDelegate OnShootFailedNoGun;
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
    private Transform bulletOrigin;
    public float minimumDistanceToGetPlant = 1.5f;

    private bool isFiring = false;
    private Vector2 lastPosition = Vector2.zero;
    private bool lastWalkingStatus = false;
    private bool lastGrounded = true;

    private int gun_energy; // = 0;
    private bool isPaused = false;
    [SerializeField]
    public GunConfiguration gun;

    [SerializeField]
    public RuntimeAnimatorController WithGunController;
    [SerializeField]
    public RuntimeAnimatorController WithPlantController;
    
    void Start() {
        base.Start();
        EventManager.Instance.OnPlantDied += HandlePlantDied;
    }

    void HandlePlantDied() {
        AttemptToDropPlant();
        isPaused = true;
    }

    private void StopFiring() {
        if (firingSequence != null) {
            StopCoroutine(firingSequence);
            isFiring = false;
            OnShootEnd?.Invoke();
        }
    }

    private void adjustGunEnergy(int adjustment) {
        gun_energy = Mathf.Clamp(gun_energy + adjustment, 0, gun.maxEnergy);
        OnGunChargeChange?.Invoke(gun_energy, gun.maxEnergy);
    }

    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        animator = GetComponent<Animator> ();
        lastPosition = GetComponent<Transform>().position;
        adjustGunEnergy(gun.maxEnergy);
        gameObject.layer = LayerMask.NameToLayer("Plant");
        animator.runtimeAnimatorController = WithPlantController;
    }

    void FixedUpdate() {
        base.FixedUpdate();
        int bonus = carryingPlant ? gun.rechargeCarryModeBonus : 0;
        adjustGunEnergy(gun.rechargeRate + bonus);
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
        if (carryingPlant) {
            DropPlant();
        }
    }

    void AttemptToPickUpPlant() {
        bool closeEnoughX = Mathf.Abs(transform.position.x - droppedX) < minimumDistanceToGetPlant;
        bool closeEnoughY = grounded || transform.position.y < minimumDistanceToGetPlant;

        if (closeEnoughX && closeEnoughY) {
            PickUpPlant();
        } else {
            OnPickupPlantFailure?.Invoke();
        }
    }

    void DropPlant() {
        carryingPlant = false;
        OnDropPlantSuccess?.Invoke(transform.position);
        spriteRenderer.sprite = withoutPlantNoAnimationSprite;
        droppedX = transform.position.x;
        droppedY = transform.position.y;
        animator.runtimeAnimatorController = WithGunController;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void PickUpPlant() {
        carryingPlant = true;
        OnPickupPlantSuccess?.Invoke();
        spriteRenderer.sprite = withPlantNoAnimationSprite;
        animator.runtimeAnimatorController = WithPlantController;
        StopFiring();
        gameObject.layer = LayerMask.NameToLayer("Plant");
    }

    IEnumerator FireIfShooting()
    {
        OnShootStart.Invoke();
        var key = Random.Range(0,600);
        while(true) 
         { 
            int shotCost = gun.fireCost + (gun_energy/gun.maxEnergy < 0.25 ? gun.lowEnergyCost : 0);
            if (gun_energy >= shotCost) {
                OnShootSuccess?.Invoke();
                isFiring = true;
                adjustGunEnergy(-shotCost);
                GameObject bulletClone = Instantiate(gun.projectilePrefab, bulletOrigin.position, Quaternion.identity);
                bulletClone.GetComponent<Projectile>()?.SetDamage(gun.damage);
                Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2((spriteRenderer.flipX ? -1f : 1f) * gun.projectileSpeed, 0f);
            } else {
                OnShootFailedNoEnergy?.Invoke();
            }
            yield return new WaitForSeconds(1f/gun.projectileRate);
         }
     }

    protected override void ComputeVelocity()
    {
        if (isPaused) return;
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");

        if (Input.GetButtonDown ("Jump")) {
            if (grounded) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpTakeOffSpeed));
                OnJumpSuccessful?.Invoke();
            }
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (!grounded) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -0.25f * jumpTakeOffSpeed));
            }
        }


        if (Input.GetButtonDown("Fire1")) {
            if (carryingPlant) {
                OnShootFailedNoGun?.Invoke();
                return;
            }
            StopFiring();
            firingSequence = StartCoroutine(FireIfShooting());
            isFiring = true;
        }
        if (Input.GetButtonUp("Fire1")) {
            StopFiring();
        }

        if (Mathf.Abs(move.x) > 0.025f) {
            bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
            if (flipSprite) 
            {
                OnChangeDirection?.Invoke(!spriteRenderer.flipX);
                spriteRenderer.flipX = !spriteRenderer.flipX;
                bulletOrigin.transform.localPosition = new Vector3(Mathf.Abs(bulletOrigin.transform.localPosition.x) * (spriteRenderer.flipX ? -1 : 1), bulletOrigin.transform.localPosition.y, bulletOrigin.transform.localPosition.z);
            }
        }

        bool isWalking = Mathf.Abs(move.x) > 0f;
        animator.SetBool("IsWalking", isWalking);

        if (!grounded) {
            float positionY = transform.position.y;
            // TODO: second part of IF is hacky way to stop ground bounce from being read as !grounded + falling
            // only really fixes if the map stays flat
            if (lastPosition.y != positionY && positionY > - 0.80f) {
                bool isJumping = positionY > lastPosition.y;
                animator.SetBool("IsFalling", !isJumping);
                animator.SetBool("IsJumping", isJumping);
            }
        } else {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
        
        
        if (lastWalkingStatus != isWalking) {
            if (isWalking) {
                OnStartRunning?.Invoke();
            } else {
                OnStopRunning?.Invoke();
            }
        }

        if (grounded && !lastGrounded) {
            OnLanding?.Invoke();
        }

        float recoil = isFiring ? maxSpeed / 2f : maxSpeed;

        targetVelocity = move * recoil;
        lastGrounded = grounded;
        lastPosition = transform.position;
        lastWalkingStatus = isWalking;
    }
}