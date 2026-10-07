using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] float bounceVelocity = 10f;
    [SerializeField] float minimumDownwardSpeed = 0.1f;
    [SerializeField] LayerMask enemyLayers;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsEnemy(other)) return;

        // Only stomp while falling, not while standing beside or below an enemy.
        if (playerRigidbody.linearVelocity.y >= -minimumDownwardSpeed)
            return;

        DamageableEnemy enemy = other.GetComponentInParent<DamageableEnemy>();
        if (enemy == null) return;

        enemy.TakeDamage(1);
        playerRigidbody.linearVelocity =
            new Vector2(playerRigidbody.linearVelocity.x, bounceVelocity);
    }

    bool IsEnemy(Collider2D other)
    {
        return (enemyLayers.value & (1 << other.gameObject.layer)) != 0;
    }
}
