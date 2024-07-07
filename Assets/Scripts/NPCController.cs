using System.Collections;
using UnityEngine;

public class NPCController : MonoBehaviour
{    
    [Header("Movement Settings")]
    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 3f;

    public float minChangeDirectionInterval = 2f;
    public float maxChangeDirectionInterval = 5f;

    public float minMoveRange = 3f;
    public float maxMoveRange = 7f;
    public float parameterChangeInterval = 10f; // How often to change parameters
    public float fallSpeed = 5;
    private bool isFalling = false;

    [Header("Ground Settings")]
    public float groundLevel = -10f;

    [Header("Faith Settings")]
    public float faith = 0f;
    public ParticleSystem faithParticles;

    private float moveSpeed;
    private float changeDirectionInterval;
    private float moveRange;

    private float startingX;
    private float targetX;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        faithParticles = GetComponentInChildren<ParticleSystem>();
        startingX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("isGrounded", true);

        // Initialize movement parameters
        UpdateMovementParameters();

        // Start the movement coroutine
        StartCoroutine(MoveRandomly());

        // Start the parameter update coroutine
        StartCoroutine(UpdateParametersPeriodically());
    }

    void Update()
    {
        if (transform.position.y < groundLevel)
        {
            Vector3 position = transform.position;
            position.y = groundLevel;
            transform.position = position;
        }

        animator.SetBool("isGrounded", !isFalling);
    }

    void UpdateMovementParameters()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval);
        moveRange = Random.Range(minMoveRange, maxMoveRange);
    }

    IEnumerator UpdateParametersPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(parameterChangeInterval);
            UpdateMovementParameters();
        }
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            // Choose a random position along the X-axis within the current move range
            targetX = startingX + Random.Range(-moveRange, moveRange);

            // Flip the sprite based on movement direction
            if (targetX > transform.position.x)
                spriteRenderer.flipX = false;
            else if (targetX < transform.position.x)
                spriteRenderer.flipX = true;

            // Set walking animation
            animator.SetBool("isWalk", true);

            // Move towards the target position
            while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = Mathf.MoveTowards(newPosition.x, targetX, moveSpeed * Time.deltaTime);
                transform.position = newPosition;
                yield return null;
            }

            // Set idle animation
            animator.SetBool("isWalk", false);

            // Wait before choosing a new direction
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }
    public void IncreaseFaith(float amount)
    {
        float oldFaith = faith;
        faith = Mathf.Clamp(faith + amount, 0f, 100f);
        Debug.Log($"Faith increased from {oldFaith} to {faith}");
        UpdateFaithVisuals();
    }
    private void UpdateFaithVisuals()
    {
        var emission = faithParticles.emission;
        emission.rateOverTime = faith;
        if (faith > 0 && !faithParticles.isPlaying)
            faithParticles.Play();
        else if (faith <= 0 && faithParticles.isPlaying)
            faithParticles.Stop();
    }
    public void OnDragStart()
    {
        isDragging = true;
        isFalling = false;
        animator.SetBool("isWalk",  false);
        StopAllCoroutines();
    }

    public void OnDragEnd()
    {
        isDragging = false;
        if (transform.position.y > groundLevel)
        {
            isFalling = true;
            StartCoroutine(FallToGround());
        }
        else
            StartCoroutine(MoveRandomly());
    }

    private IEnumerator FallToGround()
    {
        while (transform.position.y > groundLevel)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= fallSpeed * Time.deltaTime;
            
            if (newPosition.y < groundLevel)
            {
                newPosition.y = groundLevel;
            }
            
            transform.position = newPosition;
            yield return null;
        }

        isFalling = false;
        StartCoroutine(MoveRandomly());
    }
}