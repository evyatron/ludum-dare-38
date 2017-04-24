using UnityEngine;

[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
    public const float kTimeBeforeJump = 0.1f;
    public const float kMaxHealth = 4;
    public const float kHealthReductions = 0.5f;

    public Animator PlayerAnimator;
    public UIPlayerHealth UIPlayerHealth;

    [Header("Movement")]
    public float JumpSpeed = 1.0f;
    public float FallSpeed = 1.0f;
    public float JumpHeight = 1.0f;
    public float TimeInAir = 0.5f;
    public float RunSpeedModifier = 1.5f;
    public float CrouchSpeedModifier = 1.0f;
    public float JumpSpeedModifier = 1.0f;
    public float CrouchColliderHeight = 1.0f;

    public bool IsDead { get; private set; }
    public bool IsCrouching { get; private set; }
    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            UIPlayerHealth.Set(currentHealth);

            if (currentHealth == 0.0f)
            {
                Die();
            }
        }
    }

    private enum JumpState
    {
        Idle,
        Crouching,
        Jumping,
        InAir,
        Landing
    }

    private Planet planet;
    private BoxCollider playerCollider;
    private JumpState jumpState = JumpState.Idle;
    private float timer = 0.0f;
    private float previousRadius = 0.0f;
    private float currentSpeed = 1.0f;
    private float baseColliderHeight = 1.0f;

    private float currentHealth;

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider>();
        baseColliderHeight = playerCollider.size.y;
        Health = kMaxHealth;
    }

    private void Update()
    {
        if (planet == null)
        {
            planet = GetComponentInParent<Planet>();

            if (planet == null)
            {
                return;
            }

            SetDistance(planet.CurrentRadius);
        }

        if (!Application.isPlaying)
        {
            UpdateEditor();
            return;
        }

        UpdateMovement();

        if (!IsCrouching)
        {
            UpdateJump();
        }

        if (jumpState == JumpState.Idle)
        {
            LerpToSurface();
        }

        if (Application.isEditor && Input.GetKeyDown(KeyCode.Q))
        {
            planet.FinishLevel();
            FinishLevel();
        }
    }

    private void UpdateEditor()
    {
        if (previousRadius != planet.CurrentRadius)
        {
            SetDistance(planet.CurrentRadius);

            previousRadius = planet.CurrentRadius;
        }
    }

    private void UpdateMovement()
    {
        IsCrouching = !Tutorial.Instance.IsActive && !IsDead && jumpState == JumpState.Idle && Input.GetButton("Crouch");

        float speed = 1.0f;

        if (IsCrouching)
        {
            speed *= CrouchSpeedModifier;
        }
        else if (jumpState == JumpState.Jumping || jumpState == JumpState.InAir || jumpState == JumpState.Landing)
        {
            float rotationSpeedModifier = (1 - (planet.CurrentRing.RotateSpeed - planet.RotationSpeed.Minimum) / (planet.RotationSpeed.Maximum - planet.RotationSpeed.Minimum));
            rotationSpeedModifier = Mathf.Clamp(rotationSpeedModifier, 0.0f, 1.0f);
            speed *= JumpSpeedModifier * rotationSpeedModifier;
        }
        else
        {
            speed *= Mathf.Lerp(1.0f, RunSpeedModifier, Mathf.Max(Input.GetAxis("Horizontal"), 0.0f));
        }

        currentSpeed = Mathf.Lerp(currentSpeed, speed, 12.0f * Time.deltaTime);

        if (!IsDead)
        {
            planet.SetSpeed(currentSpeed * planet.CurrentRing.RotateSpeed, false);
        }

        PlayerAnimator.SetFloat("WalkSpeed", currentSpeed);

        PlayerAnimator.SetBool("IsWalking", jumpState == JumpState.Idle && planet.Rotator.RotationSpeed >= 0.5f);
        PlayerAnimator.SetBool("IsCrouching", IsCrouching);

        // Resize collider according to crouching
        Vector3 size = playerCollider.size;
        Vector3 center = playerCollider.center;
        size.y = IsCrouching ? CrouchColliderHeight : baseColliderHeight;
        center.y = size.y / 2.0f;
        playerCollider.size = size;
        playerCollider.center = center;
    }

    private void UpdateJump()
    {
        if (jumpState == JumpState.Crouching)
        {
            timer += Time.deltaTime;

            if (timer >= kTimeBeforeJump)
            {
                jumpState = JumpState.Jumping;
                PlayerAnimator.SetBool("IsPreparingToJump", false);
                PlayerAnimator.SetBool("IsJumping", true);
            }
        }
        else if (jumpState == JumpState.Jumping)
        {
            transform.Translate(Vector3.up * JumpSpeed * Time.deltaTime);

            if (transform.localPosition.y >= planet.CurrentRadius + JumpHeight)
            {
                SetDistance(planet.CurrentRadius + JumpHeight);
                timer = 0.0f;
                jumpState = JumpState.InAir;
            }
        }
        else if (jumpState == JumpState.InAir)
        {
            timer += Time.deltaTime;

            if (timer >= TimeInAir)
            {
                jumpState = JumpState.Landing;
            }
        }
        else if (jumpState == JumpState.Landing)
        {
            transform.Translate(Vector3.down * FallSpeed * Time.deltaTime);

            if (transform.localPosition.y <= planet.CurrentRadius)
            {
                SetDistance(planet.CurrentRadius);
                jumpState = JumpState.Idle;

                PlayerAnimator.SetBool("IsJumping", false);
                PlayerAnimator.SetTrigger("IsLanding");
            }
        }
        else if (jumpState == JumpState.Idle)
        {
            if (!Tutorial.Instance.IsActive && !IsDead && Input.GetButton("Jump"))
            {
                jumpState = JumpState.Crouching;
                timer = 0.0f;

                PlayerAnimator.SetBool("IsPreparingToJump", true);
            }
        }
    }

    private void LerpToSurface()
    {
        if (transform.localPosition.y != planet.CurrentRadius)
        {
            bool isAbove = transform.localPosition.y > planet.CurrentRadius;

            transform.Translate(Vector3.down * FallSpeed * Time.deltaTime * (isAbove ? 1.0f : -1.0f));

            if (isAbove && transform.localPosition.y <= planet.CurrentRadius ||
                !isAbove && transform.localPosition.y >= planet.CurrentRadius)
            {
                SetDistance(planet.CurrentRadius);
            }
        }
    }

    public void TakeDamage(int count = 1)
    {
        Health = Mathf.Clamp(Health - count * kHealthReductions, 0, kMaxHealth);

        if (count > 0)
        {
            planet.DamageTweener.Play();
        }
    }

    public void FinishLevel()
    {
        jumpState = JumpState.Landing;
    }

    private void Die()
    {
        IsDead = true;
        planet.SetSpeed(0.0f, true);
    }

    private void SetDistance(float height)
    {
        Vector3 position = transform.localPosition;
        position.y = height;
        transform.localPosition = position;
    }
}