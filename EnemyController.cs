using UnityEngine;

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

    private enum EnemyState { Chasing, Idling }
    [SerializeField] private EnemyState _currentState;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        _currentState = EnemyState.Idling;
        idleTimer = idleDuration;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerDetected = distanceToPlayer <= detectionRange;

        if (playerDetected && _currentState != EnemyState.Chasing)
            ChangeState(EnemyState.Chasing);
        else if (!playerDetected && _currentState != EnemyState.Idling)
            ChangeState(EnemyState.Idling);

        switch (_currentState)
        {
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Idling:
                Idle();
                break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        _currentState = newState;
        Debug.Log("State changed to: " + newState);
    }

    void ChasePlayer()
    {
        isChasing = true;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        idleTimer = idleDuration; // reset timer
    }

    void Idle()
    {
        isChasing = false;
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            idleTimer = idleDuration;
           
        }
    }
}