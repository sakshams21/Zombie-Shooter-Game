using System.Collections;
using UnityEngine;


/// <summary>
/// Manages Spawning of zombies
/// </summary>
public class ZombieManager : MonoBehaviour
{
    [SerializeField] private Transform ZombieSpawnPoint_Transform;
    private LevelData _currentLevelData;
    private WaitForSecondsRealtime zombieSpawnDelay = new WaitForSecondsRealtime(1.5f);
    public void Initialize()
    {
        StartCoroutine(SpawnZombie());
    }
    private IEnumerator SpawnZombie()
    {
        _currentLevelData = LevelManager.Instance.GetCurrentLevelData();
        for (int i = 0; i < _currentLevelData.NumberOfBigZombies; i++)
        {
            Zombie zombieInstance = GameManager.Instance.GetFromPool_Zombie(ZombieType.Big);
            zombieInstance.transform.position = ZombieSpawnPoint_Transform.position;
            zombieInstance.ZombieStart(GameManager.Instance.GetZombieData(ZombieType.Big));
            yield return zombieSpawnDelay;
        }

        for (int i = 0; i < _currentLevelData.NumberOfSmallZombies; i++)
        {
            Zombie zombieInstance = GameManager.Instance.GetFromPool_Zombie(ZombieType.Small);
            zombieInstance.transform.position = ZombieSpawnPoint_Transform.position;
            zombieInstance.ZombieStart(GameManager.Instance.GetZombieData(ZombieType.Small));
            yield return zombieSpawnDelay;
        }
    }
}


