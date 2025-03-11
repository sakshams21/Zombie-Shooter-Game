using UnityEngine;
using ZombieShooter;
using Microlight.MicroBar;
using System.Collections;
using System;
public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] private CapsuleCollider2D ZombieCollider;
    [SerializeField] private Animator Zombie_Animator;
    [SerializeField] private MicroBar Zombie_Health;
    private int _health;

    private ZombieData _data;

    private int Attack_AnimationHash = Animator.StringToHash("Attack");
    private int Walk_AnimationHash = Animator.StringToHash("Walk");
    private int Death_AnimationHash = Animator.StringToHash("Dead");
    private int Hit_AnimationHash = Animator.StringToHash("Hit");

    public static event Action OnZombieDeath;
    private bool isWalkable;
    private WaitForSeconds _zombieAttackCooldown;
    private WaitForSeconds _zombieAttackDelay;
    private Coroutine _attackCoro;
    public void ZombieStart(ZombieData zombieData)
    {
        _data = zombieData;
        isWalkable = true;
        _health = _data.Health;
        AnimationUpdate_Walk(true);
        ZombieCollider.enabled = true;
        Zombie_Health.Initialize(_data.Health);
        Zombie_Health.gameObject.SetActive(true);
        _zombieAttackCooldown = new WaitForSeconds(_data.Attack_Cooldown);
        _zombieAttackDelay = new WaitForSeconds(0.5f);
    }

    private void Update()
    {
        if (isWalkable)
        {
            transform.position += Vector3.left * Time.deltaTime * _data.Movement_Speed;
        }
    }

    public int GetZombieDamage()
    {
        return _data.Attack_Damage;
    }


    public void Damage(int damage)
    {
        AnimationUpdate_Hit();
        _health -= damage;
        Zombie_Health.UpdateBar(_health);
        if (_health <= 0)
        {
            _health = 0;
            OnDeath();
        }
    }

    private void OnDeath()
    {
        isWalkable = false;
        AnimationUpdate_Death();
        ZombieCollider.enabled = false;
        Zombie_Health.gameObject.SetActive(false);
        StopCoroutine(_attackCoro);
        OnZombieDeath?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     isWalkable = false;
        //     AnimationUpdate_Walk(false);
        //     Attack();
        // }

        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
        {
            isWalkable = false;
            AnimationUpdate_Walk(false);
            _attackCoro = StartCoroutine(Attack_Coro(target));
        }
    }

    // private void Attack(ref IDamageable target)
    // {
    //     StartCoroutine(Attack_Coro(target));
    // }


    IEnumerator Attack_Coro(IDamageable target)
    {
        while (true)
        {
            AnimationUpdate_Attack();
            yield return _zombieAttackDelay;
            target.Damage(_data.Attack_Damage);
            yield return _zombieAttackCooldown;
        }
    }

    public void AfterDeath()
    {
        GameManager.Instance.Ref_PoolManager.BackToPool_Zombie(this, _data.Type);
    }

    #region ZOmbie Animation

    private void AnimationUpdate_Hit()
    {
        Zombie_Animator.SetTrigger(Hit_AnimationHash);
    }

    private void AnimationUpdate_Walk(bool status)
    {
        Zombie_Animator.SetBool(Walk_AnimationHash, status);
    }

    private void AnimationUpdate_Death()
    {
        Zombie_Animator.SetBool(Death_AnimationHash, true);

    }

    private void AnimationUpdate_Attack()
    {
        Zombie_Animator.SetTrigger(Attack_AnimationHash);
    }
    #endregion


}