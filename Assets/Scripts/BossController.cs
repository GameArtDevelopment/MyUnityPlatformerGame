using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Slider healthSlider;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float detectionRange = 7f;
    [SerializeField] float patrolSpeed = 1.5f;
    [SerializeField] float chaseSpeed = 2.5f;

    float health;
    Transform targetPoint;

    void Start()
    {
        health = maxHealth;
        targetPoint = pointB;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    void Update()
    {
        if (player == null || pointA == null || pointB == null) return;

        bool chasing =
            Vector2.Distance(transform.position, player.position) <= detectionRange;

        if (chasing)
        {
            MoveTowards(player.position, chaseSpeed);
        }
        else
        {
            MoveTowards(targetPoint.position, patrolSpeed);

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
                targetPoint = targetPoint == pointA ? pointB : pointA;
        }
    }

    void MoveTowards(Vector3 destination, float speed)
    {
        transform.position = Vector2.MoveTowards(
            transform.position, destination, speed * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0f, health - amount);

        if (healthSlider != null)
            healthSlider.value = health;

        if (health <= 0f)
            Destroy(gameObject);
    }

    
}
