using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private Zombie Ref_Zombie;
    [SerializeField] private CircleCollider2D AttackCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var target))
        {
            target.Damage(Ref_Zombie.GetZombieDamage());
            ColliderSatus(false);
            Debug.Log("WTFFFFFFFFFFFFF");
        }
        Debug.Log(collision.gameObject.name);
    }

    public void ColliderSatus(bool status)
    {
        AttackCollider.enabled = status;
    }


}