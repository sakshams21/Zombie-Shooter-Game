using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PoolManager Ref_PoolManager;
    public ZombieManager Ref_ZombieManager;
    [SerializeField] private GunData CurrentGunData;
    [SerializeField] private ZombieData[] ZombieData;
    [SerializeField] private LevelDataList Level_Data;

    public event Action<int> OnHeathChange;

    private int _health;

    public int Health => _health;

    public float _score;

    public float Score => _score;

    private int _currentLevel;

    public event Action OnPlayerDead;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        _health = 100;
    }

    private IEnumerator Start()
    {
        yield return Ref_PoolManager.Initialize();

        yield return Ref_ZombieManager.Initialize();

        Zombie.OnZombieDeath += ScoreUpdate;
    }

    private void ScoreUpdate()
    {
        _score++;
        if (_score >= Level_Data.Data[_currentLevel].ObjectKill)
        {

            //load next level
        }
    }

    public LevelData GetLEvelData()
    {
        return Level_Data.Data[_currentLevel];
    }

    public GunData GetGunData()
    {
        return CurrentGunData;
    }

    public ZombieData GetZombieData(ZombieType type)
    {
        return ZombieData[(int)type];
    }

    public void HealthManager(int damage)
    {
        _health -= damage;
        OnHeathChange?.Invoke(_health);

        if (_health < 0)
        {
            _health = 0;
            OnPlayerDead?.Invoke();
        }
    }

}

#region ENUMS

public enum GunType
{
    Rifle, SMG, Pistol
}
public enum ZombieType
{
    Small, Big
}
#endregion