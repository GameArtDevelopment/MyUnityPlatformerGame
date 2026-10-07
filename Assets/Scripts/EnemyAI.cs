using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum Behavior { Patrol, Chase }

    [SerializeField] Behavior behavior;
    [SerializeField] Transform player;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float chaseSpeed = 3f;
    [SerializeField] float detectionRange = 5f;

    Transform targetPoint;

    void Start()
    {
        targetPoint = pointB;
    }

    void Update()
    {
        if (player == null) return;

        bool canSeePlayer =
            Vector2.Distance(transform.position, player.position) <= detectionRange;

        if (behavior == Behavior.Chase && canSeePlayer)
        {
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        MoveTowards(targetPoint.position, patrolSpeed);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
            targetPoint = targetPoint == pointA ? pointB : pointA;
    }

    void MoveTowards(Vector3 destination, float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position, destination, speed * Time.deltaTime);
    }
}
