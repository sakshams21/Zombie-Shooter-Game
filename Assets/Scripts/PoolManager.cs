using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZombieShooter;

public class PoolManager : MonoBehaviour
{
    // public static PoolManager Instance { get; private set; }
    [SerializeField] private Bullet Bullet_Prefab;
    [SerializeField] private int Max_Bullet_Spawn;
    [SerializeField] private Transform BulletSpawnPoint_Transform;
    [SerializeField] private Queue<Bullet> Bullet_Pool = new();

    [Space(10)]
    [SerializeField] private Zombie BigZombie_Prefab;
    [SerializeField] private int Max_BigZombie_Spawn;
    [SerializeField] private Queue<Zombie> BigZombie_Pool = new();

    [Space(10)]
    [SerializeField] private Zombie SmallZombie_Prefab;
    [SerializeField] private int Max_SmallZombie_Spawn;
    [SerializeField] private Queue<Zombie> SmallZombie_Pool = new();
    [Space(10)]
    [SerializeField] private Transform ZombieSpawnPoint_Transform;
    private WaitForSeconds _spawnWaitTime = new WaitForSeconds(0.1f);

    public IEnumerator Initialize()
    {
        yield return StartCoroutine(SpawnPool());
    }

    IEnumerator SpawnPool()
    {
        for (int i = 0; i < Max_Bullet_Spawn; i++)
        {
            Bullet bullet = Instantiate(Bullet_Prefab, BulletSpawnPoint_Transform);
            bullet.gameObject.SetActive(false);
            Bullet_Pool.Enqueue(bullet);
            yield return _spawnWaitTime;
        }

        for (int i = 0; i < Max_BigZombie_Spawn; i++)
        {
            Zombie zombie_intance = Instantiate(BigZombie_Prefab, ZombieSpawnPoint_Transform);
            zombie_intance.gameObject.SetActive(false);
            BigZombie_Pool.Enqueue(zombie_intance);
            yield return _spawnWaitTime;
        }

        for (int i = 0; i < Max_SmallZombie_Spawn; i++)
        {
            Zombie zombie_intance = Instantiate(SmallZombie_Prefab, ZombieSpawnPoint_Transform);
            zombie_intance.gameObject.SetActive(false);
            SmallZombie_Pool.Enqueue(zombie_intance);
            yield return _spawnWaitTime;
        }

    }

    #region Bullet pooling
    public Bullet GetBullet()
    {
        Bullet bulletInstance = (Bullet_Pool.Count > 0) ? Bullet_Pool.Dequeue() : Instantiate(Bullet_Prefab, BulletSpawnPoint_Transform);
        bulletInstance.gameObject.SetActive(true);
        return bulletInstance;
    }

    public void BackToPool_Bullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        Bullet_Pool.Enqueue(bullet);

    }
    #endregion


    #region Zombie pooling

    public Zombie GetZombie(ZombieType zombie_type)
    {
        Zombie zombie;
        switch (zombie_type)
        {
            case ZombieType.Small:
                zombie = SmallZombie_Pool.Count > 0 ? SmallZombie_Pool.Dequeue() : Instantiate(SmallZombie_Prefab, ZombieSpawnPoint_Transform);
                break;

            case ZombieType.Big:
                zombie = BigZombie_Pool.Count > 0 ? BigZombie_Pool.Dequeue() : Instantiate(BigZombie_Prefab, ZombieSpawnPoint_Transform);
                break;

            default:
                zombie = BigZombie_Pool.Count > 0 ? BigZombie_Pool.Dequeue() : Instantiate(BigZombie_Prefab, ZombieSpawnPoint_Transform);
                break;
        }
        zombie.gameObject.SetActive(true);
        return zombie;
    }

    public void BackToPool_Zombie(Zombie zombie, ZombieType type)
    {
        zombie.gameObject.SetActive(false);
        switch (type)
        {
            case ZombieType.Small:
                SmallZombie_Pool.Enqueue(zombie);
                break;
            case ZombieType.Big:
                BigZombie_Pool.Enqueue(zombie);
                break;

            default:
                BigZombie_Pool.Enqueue(zombie);
                break;
        }
    }
    #endregion
}
