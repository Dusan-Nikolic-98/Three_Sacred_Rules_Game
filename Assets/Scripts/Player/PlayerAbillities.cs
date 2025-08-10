using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbillities : MonoBehaviour
{
    //dash delovi
    public bool canDash = false;
    public bool IsDashing { get; private set; } = false;
    public float dashDuration = 0.2f;

    public GameObject dashIconUI;
    public float dashForce = 5f;
    public float dashCooldown = 2f;
    private float lastDashTime = -Mathf.Infinity;
    public Image dashCooldownMask;

    private bool hasBounced = false;
    private Vector2 currentDashDir;
    //za viz dash
    private TrailRenderer dashTrail;
    float originalGravity = 0f;


    private Rigidbody2D rb;

    //totem delovi
    public GameObject totem;
    public float totemDuration = 5f;
    private GameObject activeTotem;
    public bool canTotem = false;
    public GameObject totemIconUI;
    public float totemCooldown = 15f;
    public Image totemCooldownMask;
    private float lastTotemTime = -Mathf.Infinity;

    //strah
    public bool canFear = false;
    public float fearDuration = 5f;
    public KeyCode fearKey = KeyCode.Q;
    public GameObject fearIconUI;
    public Image fearCooldownMask;
    private float lastFearTime = -Mathf.Infinity;
    public float fearCooldown = 20f;


    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(dashIconUI != null)dashIconUI.SetActive(false);
        if(totemIconUI != null)totemIconUI.SetActive(false);
        if(fearIconUI != null)fearIconUI.SetActive(false);

        dashTrail = GetComponent<TrailRenderer>();
        if (dashTrail != null) 
        {
            dashTrail.enabled = false;
        }
    }

    void Update()
    {
        //dash
        if (canDash && Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown) {
            DoDash();
        }
        if (canDash && dashCooldownMask != null) 
        {
            float elapsed = Time.time - lastDashTime;
            float fill = Mathf.Clamp01(1 - (elapsed / dashCooldown));
            dashCooldownMask.fillAmount = fill;
        }

        //totem deo
        if (canTotem && Input.GetKeyDown(KeyCode.S) && Time.time >= lastTotemTime + totemCooldown) 
        {
            SpawnTotem();
        }

        if (canTotem && totemCooldownMask != null) 
        {
            float elapsed = Time.time - lastTotemTime;
            float fill = Mathf.Clamp01(1 - (elapsed /  totemCooldown));
            totemCooldownMask.fillAmount = fill;
        }

        //fear
        if (canFear && Input.GetKeyDown(fearKey) && Time.time >= lastFearTime + fearCooldown)
        {
            StartCoroutine(ActivateFear());
        }

        if(canFear && fearCooldownMask != null)
        {
            float elapsed = Time.time - lastFearTime;
            float fill = Mathf.Clamp01(1 - elapsed / fearCooldown);
            fearCooldownMask.fillAmount = fill;
        }
    }

    // DASH
    void DoDash()
    {

        Vector2 dashDirection;

        bool up = Input.GetKey(KeyCode.W);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (up && left)
        {
            dashDirection = new Vector2(-1f, 1f).normalized;
        }
        else if (up && right) 
        {
            dashDirection = new Vector2(1f, 1f).normalized;
        }
        else if (up)
        {
            dashDirection = Vector2.up;
        }
        else
        {
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0f).normalized;
            if (dashDirection == Vector2.zero)
            {
                dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            }

        }
        lastDashTime = Time.time;
        StartCoroutine(DashRoutine(dashDirection));
    }

    private IEnumerator DashRoutine(Vector2 direction, bool isBounce = false) {
        IsDashing = true;
        rb.linearVelocity = Vector2.zero;
        originalGravity = rb.gravityScale == 0f? originalGravity : rb.gravityScale;
        rb.gravityScale = 0f; //da bude konzistentan dash

        if (!isBounce) {
            hasBounced = false;
            currentDashDir = direction.normalized;
        }

        //trail
        if (dashTrail != null) 
        {
            dashTrail.Clear();
            dashTrail.enabled = true;
        }
        
        rb.linearVelocity = direction.normalized * dashForce;
        if (isBounce) rb.linearVelocity *= 2.5f;

        yield return new WaitForSeconds(dashDuration);

        if (dashTrail != null) 
        {
            dashTrail.enabled = false;
        }

        rb.gravityScale = originalGravity;
        IsDashing = false;

    }

    //za bouns

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsDashing || hasBounced)
            return;
        if (collision.collider.CompareTag("Tile")) 
        {
            if (Mathf.Abs(currentDashDir.x) > 0.1f && Mathf.Abs(currentDashDir.y) > 0.1f) 
            {
                Vector2 mirroredDir = new Vector2(-currentDashDir.x, currentDashDir.y).normalized;
                hasBounced = true;
                StartCoroutine(DashRoutine(mirroredDir, true));
            }
        }
    }


    // TOTEM
    private void SpawnTotem()
    {
        if (activeTotem != null) Destroy(activeTotem); //za svaki slucaj

        Vector3 spawnPos = transform.position;
        activeTotem = Instantiate(totem, spawnPos, Quaternion.identity);

        //TODO DEO ZA PREBACIVANJE DUHOVA NA NJEGA
        GhostTargetManager.Instance.SetTarget(activeTotem.transform);
        lastTotemTime = Time.time;
        StartCoroutine(RemoveTotemAfterDelay());
    }

    private IEnumerator RemoveTotemAfterDelay()
    {
        yield return new WaitForSeconds(totemDuration);
        if (activeTotem != null) 
        { 
            Destroy(activeTotem);
            //TODO DEO ZA PREBACIVANJE NAZAD NA PLAYERA
            GhostTargetManager.Instance.SetTarget(GameObject.FindWithTag("Player").transform);
        }
    }

    // FEAR
    private IEnumerator ActivateFear()
    {
        //isFearing = true;
        lastFearTime = Time.time;
        GhostTargetManager.Instance.SetGhostsAfraid(true);
        yield return new WaitForSeconds(fearDuration);
        GhostTargetManager.Instance.SetGhostsAfraid(false);
        //isFearing=false;

    }

    // UNLOCKOVANJE
    public void UnlockAbility(string scrollType)
    {
        switch (scrollType)
        {
            case "RuleOfWind":
                {
                    canDash = true;
                    if(dashIconUI != null) dashIconUI.SetActive(true);
                }
                break;
            case "RuleOfMischief":
                {
                    canTotem = true;
                    if (totemIconUI != null) totemIconUI.SetActive(true);
                }
                break;
            case "RuleOfFear":
                {
                    canFear = true;
                    if(fearIconUI != null) fearIconUI.SetActive(true);
                }
                break;
            default:
                {
                    Debug.Log("aj aj aj ko si ti");
                }
                break;

        }
    }

}
