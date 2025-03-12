using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Script References")]
    [SerializeField] private PoolManager Ref_PoolManager;
    [SerializeField] private ZombieManager Ref_ZombieManager;
    [SerializeField] private UIManager Ref_UIManager;
    [SerializeField] private Player Ref_Player;

    [Space(20f)]
    [Header("UI References")]
    [SerializeField] private Canvas Game_Canvas;
    [SerializeField] private Canvas Loading_Canvas;
    [SerializeField] private CanvasGroup Loading_CanvasGroup;
    [SerializeField] private GameObject Tutorial_Go;

    [Space(20f)]
    [Header("Data")]
    [SerializeField] private ZombieData[] ZombieData;

    public event Action<int> OnHeathChange;
    public event Action<int, int> OnScoreChange;


    private int _health;

    public int Health => _health;

    public int _score;

    public int Score => _score;

    private bool isPaused;
    public event Action OnPlayerDead;

    private Controls _playerControls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        _playerControls = new Controls();
        Instance = this;
        _health = 100;
    }



    private IEnumerator Start()
    {

        Tutorial_Go.SetActive(LevelManager.Instance.CurrentLevel == 0);
        Game_Canvas.enabled = false;
        Loading_Canvas.enabled = true;
        Loading_Canvas.gameObject.SetActive(true);
        Loading_CanvasGroup.alpha = 1;

        yield return Ref_PoolManager.Initialize();

        Game_Canvas.enabled = true;
        Loading_CanvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            Loading_Canvas.enabled = false;
        });

        if (LevelManager.Instance.CurrentLevel != 0)
        {
            ZombieInitialize();
        }
        else
        {
            _playerControls.Player.Attack.Disable();
        }

        Zombie.OnZombieDeath += ScoreUpdate;
        UI_Value_Initialize();

    }


    #region INPUT
    private void OnEnable()
    {
        //_playerControls.Player.pau
        _playerControls.Player.Pause.performed += Pause;
        _playerControls.Player.Pause.Enable();
        _playerControls.Player.Attack.performed += Shoot;
        _playerControls.Player.Attack.canceled += StopShoot;
        _playerControls.Player.Attack.Enable();
    }

    private void Pause(InputAction.CallbackContext context)
    {
        ActionOnPause();
    }

    public void ActionOnPause()
    {
        isPaused = !isPaused;
        TimeScaleChange(isPaused ? 0 : 1);
        if (isPaused)
        {
            _playerControls.Player.Pause.Disable();
        }
        else
        {
            _playerControls.Player.Attack.Enable();
        }
        Ref_UIManager.PauseScreen(isPaused);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Ref_Player.Shoot(ref context);
    }
    private void StopShoot(InputAction.CallbackContext context)
    {
        Ref_Player.StopShoot(ref context);
    }



    private void OnDisable()
    {
        _playerControls.Player.Pause.performed += Pause;
        _playerControls.Player.Pause.Disable();
        _playerControls.Player.Attack.performed -= Shoot;
        _playerControls.Player.Attack.canceled += StopShoot;
        _playerControls.Player.Attack.Disable();
        Zombie.OnZombieDeath -= ScoreUpdate;
    }
    #endregion

    //Set ammo text, set object text
    private void UI_Value_Initialize()
    {
        int magSize = LevelManager.Instance.EquippedGunData.Magazine_Size;
        Ref_UIManager.AmmoUpdate(magSize, magSize);
        Ref_UIManager.HealthUpdate(100);
        Ref_UIManager.ObjectiveUpdate(0, LevelManager.Instance.GetCurrentLevelData().ObjectKill);
    }

    public void ZombieInitialize()
    {
        _playerControls.Player.Attack.Enable();
        Ref_ZombieManager.Initialize();
    }

    public void TimeScaleChange(float value)
    {
        Time.timeScale = value;
    }


    #region POOL MANAGER

    public Bullet GetFromPool_Bullet()
    {
        return Ref_PoolManager.GetBullet();
    }

    public void BackToPool_Bullet(Bullet bullet)
    {
        Ref_PoolManager.BackToPool_Bullet(bullet);
    }

    public Zombie GetFromPool_Zombie(ZombieType zombie_type)
    {
        return Ref_PoolManager.GetZombie(zombie_type);
    }

    public void BackToPool_Zombie(Zombie zombie, ZombieType type)
    {
        Ref_PoolManager.BackToPool_Zombie(zombie, type);
    }

    #endregion
    public GunData GetGunData()
    {
        return LevelManager.Instance.EquippedGunData;
    }

    public ZombieData GetZombieData(ZombieType type)
    {
        return ZombieData[(int)type];
    }


    private void ScoreUpdate()
    {
        _score++;
        LevelManager.Instance.TotalScore++;
        int objectiveKill = LevelManager.Instance.GetCurrentLevelData().ObjectKill;
        OnScoreChange?.Invoke(_score, objectiveKill);

        if (_score >= objectiveKill)
        {
            LevelManager.Instance.LevelCleared();
        }
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