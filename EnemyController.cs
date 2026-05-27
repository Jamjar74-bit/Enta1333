using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour

{
    [Header("Movement Settings")]
    public float chaseSpeed = 5f;
    public float idleDuration = 2f;
    public float detectionRange = 10f;

    [Header("References")]
    public Transform player;
    public Animator animator;

    private float idleTimer;
    private bool isChasing;

    // State names (must match your Animator parameter names)
    private const string SPEED_PARAM = "Speed";
    private const string IS_CHASING_PARAM = "IsChasing";

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        idleTimer = idleDuration;
        isChasing = false;
    }

    void Update()
    {
        // Check if player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerDetected = distanceToPlayer <= detectionRange;

        // State logic
        if (playerDetected)
        {
            ChasePlayer();
        }
        else
        {
            IdleState();
        }

        // Update Animator parameters
        UpdateAnimator();
    }

    void ChasePlayer()
    {
        isChasing = true;

        // Move towards player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Optional: Rotate to face player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Reset idle timer when chasing
        idleTimer = idleDuration;
    }

    void IdleState()
    {
        isChasing = false;

        // Countdown idle timer
        idleTimer -= Time.deltaTime;

        // Simple idle behavior (can add wandering or looking around)
        if (idleTimer <= 0)
        {
            // Reset timer and maybe do a random idle animation
            idleTimer = idleDuration;
            // You could trigger a random idle animation here
        }
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            // Set speed parameter (0 = idle, 1 = running)
            float speed = isChasing ? 1f : 0f;
            animator.SetFloat(SPEED_PARAM, speed);

            // Alternative: Use boolean parameter
            animator.SetBool(IS_CHASING_PARAM, isChasing);
        }
    }

    // Visualize detection range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
