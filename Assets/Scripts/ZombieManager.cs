using System.Collections;
using UnityEngine;


/// <summary>
/// Manages Spawning and despawning of zombies
/// </summary>
public class ZombieManager : MonoBehaviour
{
    [SerializeField] private Transform ZombieSpawnPoint_Transform;
    private LevelData _currentLevelData;
    private WaitForSeconds zombieSpawnDelay = new WaitForSeconds(0.5f);
    public IEnumerator Initialize()
    {
        yield return StartCoroutine(SpawnZombie());
    }
    private IEnumerator SpawnZombie()
    {
        _currentLevelData = GameManager.Instance.GetLEvelData();
        for (int i = 0; i < _currentLevelData.NumberOfBigZombies; i++)
        {
            Zombie zombieInstance = GameManager.Instance.Ref_PoolManager.GetZombie(ZombieType.Big);
            zombieInstance.transform.position = ZombieSpawnPoint_Transform.position;
            zombieInstance.ZombieStart(GameManager.Instance.GetZombieData(ZombieType.Big));
            yield return zombieSpawnDelay;
        }

        for (int i = 0; i < _currentLevelData.NumberOfSmallZombies; i++)
        {
            Zombie zombieInstance = GameManager.Instance.Ref_PoolManager.GetZombie(ZombieType.Small);
            zombieInstance.transform.position = ZombieSpawnPoint_Transform.position;
            zombieInstance.ZombieStart(GameManager.Instance.GetZombieData(ZombieType.Small));
            yield return zombieSpawnDelay;
        }
    }
}


