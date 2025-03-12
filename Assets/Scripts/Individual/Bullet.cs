using UnityEngine;


public class Bullet : MonoBehaviour
{
    private int _damage = 10;

    [SerializeField] private Rigidbody2D Bullet_RigidBody;



    public void StartBullet(float speed, int damage)
    {
        _damage = damage;
        Bullet_RigidBody.linearVelocityX = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var target))
        {
            target.Damage(_damage);
        }

        gameObject.SetActive(false);
        Bullet_RigidBody.linearVelocityX = 0;
        //Back to pool
        GameManager.Instance.BackToPool_Bullet(this);
    }
}
