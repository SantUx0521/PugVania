using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class MovementPlayer : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float WalkSpeed = 1;
    private float xAxis, yAxis;
    private bool moveset = true;
    float timebystep = 0.5f;
    float cont = 0f;
    [Space(5)]

    [Header("Salto sostenido")]
    [SerializeField] private float JumpForce = 15f;
    [SerializeField] private float MaxJumpTime = 0.5f;
    [SerializeField] private Transform GCheck;
    [SerializeField] private float GCheckRadius = 0.2f;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private LayerMask WhatIsPlat;
    [SerializeField] private LayerMask WhatIsOverEnemy;
    private float coyoteTimeCounter = 0;
    [Space(5)]

    [Header("Verticalidad")]
    [SerializeField] private float coyoteTime;
    [SerializeField] private int maxAirJump;
    [SerializeField] private bool isJumping;
    private int airJumpCounter = 0;
    public bool isGrounded;
    private bool isPlatform;
    private float jumpTimeCounter;
    [Space(5)]


    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;
    private bool canDash = true;
    private float GravityDash;
    private bool Dashed;
    [Space(5)]

    [Header("Attack")]
    private bool attack = false;
    [SerializeField] float timeBetwween;
    float timeSinceAttack;
    [SerializeField] Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    [SerializeField] Vector3 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask AttackableLayer;
    [SerializeField] float damage;
    [SerializeField] GameObject slashEffect;
    [Space(5)]

    [Header("Recoil")]
    [SerializeField] int recoilStepsX = 5;
    [SerializeField] int recoilStepsY = 5;
    [SerializeField] float recoilXSpeed = 100;
    [SerializeField] float recoilYSpeed = 100;
    int stepXrecoil, stepYrecoil;
    [Space(5)]

    [Header("Health")]
    public int _health;
    public int maxHealth;
    [SerializeField] GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed;
    public bool restoreTime;
    float restoreTimeSpeed;
    public delegate void OnHeartChangedDelegate();
    [HideInInspector] public OnHeartChangedDelegate OnHeartChangedCallBack;
    [Space(5)]

    [Header("Death Animation")]
    [SerializeField] public float deathAnimationDuration = 1.5f;
    public bool isDead = false;
    [SerializeField] public float deathGravityScale = 5f;
    [Space(5)]

    private Rigidbody2D rb;
    [HideInInspector] public PlayerStateList pState;
    public Animator anim;
    public static MovementPlayer Instance;
    private SpriteRenderer sr;
    private Vector2 lastSafePosition;

    public PlayerSoundController playerSoundController;


    void Awake()
    {


        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        pState = GetComponent<PlayerStateList>();
        if (pState == null)
        {
            Debug.LogError("PlayerStateList no encontrado en MovementPlayer.");
        }

        Health_P = maxHealth;
    }


    void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        GravityDash = rb.gravityScale;

        sr = GetComponent<SpriteRenderer>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    void Update()
    {
        canNotMove();
        Actions();
        JumpVariables();

        if (pState.dashing) return;
        flip();
        Movement();
        HandleJump();
        gravity();
        Attack();
        recoil();
        restoreTimeScale();
        FlashWhileInvincible();
        if (Grounded())
        {
            lastSafePosition = transform.position;
        }

        if (pState.ableToDash == false) return;
        StartDash();
    }

    void canNotMove()
    {
        if (pState.canMove == false)
        {
            anim.Play("Player_idle");
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        if (pState.dashing) return;
        recoil();
    }

    void Actions()
    {
        if (pState.canMove && isDead == false)
        {
            xAxis = Input.GetAxisRaw("Horizontal");
            yAxis = Input.GetAxisRaw("Vertical");
            attack = Input.GetButtonDown("Attack");
        }
    }

    void flip()
    {
        Vector3 newScale = transform.localScale;

        if (xAxis < 0)
        {
            newScale.x = -Mathf.Abs(newScale.x);
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            newScale.x = Mathf.Abs(newScale.x);
            pState.lookingRight = true;
        }

        transform.localScale = newScale;
    }

    private void Movement()
    {
        rb.linearVelocity = new Vector2(WalkSpeed * xAxis, rb.linearVelocity.y);
        anim.SetBool("Walking", rb.linearVelocity.x != 0 && (Grounded()|| isPlatform));
        if (rb.linearVelocityX != 0 && (isGrounded || isPlatform))
        {
            cont += Time.deltaTime;
            if (cont >= timebystep)
            {
                cont = 0f;
                playerSoundController.playPasos();
            }
        }

    }

    void stopRecoilX()
    {
        stepXrecoil = 0;
        pState.recoilingX = false;
    }
    void stopRecoilY()
    {
        stepYrecoil = 0;
        pState.recoilingY = false;
    }

    public void takeDamage(int _damage, Vector2 knockbackDirection, float knockbackForce)
    {
        if (pState.invincible) return;

        Health_P -= _damage;
        GameManager.Instance.TakeDamage(_damage);
        moveset = false;
        if (pState.dashing)
        {
            StopCoroutine("Dash"); 
            pState.dashing = false;
            canDash = true;
        }

        if (!pState.invincible)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        pState.invincible = true;


        StartCoroutine(StopDamage());
    }

    IEnumerator StopDamage()
    {
        GameObject _bloodSpurtParticle = Instantiate(bloodSpurt, transform.position, quaternion.identity);
        Destroy(_bloodSpurtParticle, 1.5f);
        anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
        moveset = true;
    }
    void restoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }
    public void FlashWhileInvincible()
    {
        sr.material.color = pState.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 0.1f)) : Color.white;
    }
    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;

        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }
    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
        moveset = true;
        pState.invincible = false;
    }
    public int Health_P
    {
        get => _health;
        set
        {
            int newHealth = Mathf.Clamp(value, 0, maxHealth);
            if (_health != newHealth)
            {
                _health = newHealth;
                OnHeartChangedCallBack?.Invoke();
            }
        }
    }
    public void TriggerDeathAnimation()
    {
        if (pState.alive)
        {
            isDead = true;
            anim.SetTrigger("Death");
            StartCoroutine(DeathRoutine());
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killable") && !pState.invincible)
        {
            Health_P -= 2;
            GameManager.Instance.TakeDamage(2);
            RespawnPosition();
        }
    }

    public void RespawnPosition()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = lastSafePosition + Vector2.up * 0.5f;
        anim.SetTrigger("TakeDamage");
        pState.invincible = true;
        StartCoroutine(RemoveInvincibility());
    }

    private IEnumerator RemoveInvincibility()
    {
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
    }
    private IEnumerator DeathRoutine()
    {
        pState.alive = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        yield return new WaitForSecondsRealtime(deathAnimationDuration);

    }


    public bool Grounded()
    {
        return Physics2D.OverlapCircle(GCheck.position, GCheckRadius, WhatIsGround);
    }
    public bool OverEnemy()
    {
        Collider2D enemyCollider = Physics2D.OverlapCircle(GCheck.position, GCheckRadius, WhatIsOverEnemy);

        if (enemyCollider != null)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    void StartDash()
    {
        if (moveset == true & isDead == false)
        {
            if (Input.GetButtonDown("Dash") && canDash && !Dashed)
            {
                anim.SetTrigger("Dashing");
                playerSoundController.playDash();
                StartCoroutine(Dash());
                Dashed = true;
            }
            if (Grounded() || isPlatform)
            {
                Dashed = false;
            }
            if (OverEnemy() && rb.linearVelocity.y < 0)
            {
                Dashed = false;
            }
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true; //si aqui esta verdad
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = GravityDash;
        pState.dashing = false; 
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetwween)
        {
            timeSinceAttack = 0;
            playerSoundController.playAtack();

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {
                anim.SetTrigger("Attacking");
                hit(SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
                slashEffect_angle(slashEffect, 0, SideAttackTransform);
            }
            else if (yAxis > 0)
            {
                anim.SetTrigger("UpAttacking");
                hit(UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
                slashEffect_angle(slashEffect, 80, UpAttackTransform);
            }
            else if (yAxis < 0 && !Grounded())
            {
                anim.SetTrigger("Attacking");
                hit(DownAttackTransform, DownAttackArea, ref pState.recoilingY, recoilYSpeed);
                slashEffect_angle(slashEffect, -80, DownAttackTransform);
            }
        }
    }

    void hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrenght)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, AttackableLayer);
        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<InterfaceEnemy>() != null)
            {
                objectsToHit[i].GetComponent<InterfaceEnemy>().Damage((int)damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrenght);
            }
        }
    }

    void slashEffect_angle(GameObject _slashEffect, int effect_angle, Transform _attackTransform)
    {
        GameObject instantiatedEffect = Instantiate(_slashEffect, _attackTransform.position, Quaternion.identity);
        float characterDirection = transform.localScale.x;
        instantiatedEffect.transform.rotation = Quaternion.Euler(0, 0, effect_angle + (characterDirection < 0 ? 180 : 0));
        instantiatedEffect.transform.localScale = new Vector3(1, 1, 1);
    }

    void recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }
        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
            airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = GravityDash;
        }

        //para detener el recoil bro
        if (pState.recoilingX && stepXrecoil < recoilStepsX)
        {
            stepXrecoil++;
        }
        else
        {
            stopRecoilX();
        }

        if (pState.recoilingY && stepYrecoil < recoilStepsY)
        {
            stepYrecoil++;
        }
        else
        {
            stopRecoilY();
        }

        if (Grounded())
        {
            stopRecoilY();
        }
    }

    public bool Platform()
    {
        return Physics2D.OverlapCircle(GCheck.position, GCheckRadius, WhatIsPlat);
    }


    void HandleJump()
    {
        if (pState.canMove && isDead == false)
        {
            isGrounded = Grounded();
            isPlatform = Platform();

            if ((isGrounded || coyoteTimeCounter > 0f || isPlatform) && Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                jumpTimeCounter = MaxJumpTime;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            }

            if (isJumping && Input.GetButton("Jump"))
            {
                if (jumpTimeCounter > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }
            else if (!Grounded() && airJumpCounter < maxAirJump && Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                airJumpCounter++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }

            if (isJumping == true)
            {
                anim.SetBool("Jumping", true);
            }
            else
            {
                anim.SetBool("Jumping", false);
            }

            anim.SetFloat("YVelocity", rb.linearVelocity.y);
        }
    }
    void gravity()
    {
        if (isJumping == false)
        {
            anim.SetBool("Gravity", true);
        }
        else
        {
            anim.SetBool("Gravity", false);
        }
    }

    void JumpVariables()
    {
        if (Grounded())
        {
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public void resetPlayer()
    {
        rb.linearVelocity = Vector2.zero;
        isDead = false;
        anim.Play("Player_idle");
    }
}