using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

//Will handle clip mangement and reloading
public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform GunSpawnPoint_Transform;
    [SerializeField] private Image Reload_Image;


    private GunData _currentGunData;
    private Weapon _currentGun;
    private Vector3 _bulletSpawnPoint;



    private float _nextFireTime;
    private bool _canShoot = true;
    private bool _startTimer;
    private bool _isReloading;
    private int _currentMag = 0;


    private void Start()
    {
        _currentGunData = GameManager.Instance.GetGunData();
        _currentGun = Instantiate(_currentGunData.Weapon_Prefab, GunSpawnPoint_Transform);
        _currentGun.transform.localPosition = Vector3.zero;
        _bulletSpawnPoint = _currentGun.GetBulletSpawnPoint();
        _currentMag = _currentGunData.Magazine_Size;
    }

    private void Update()
    {
        if (_startTimer && Time.time >= _nextFireTime && !_isReloading) // Left mouse or controller button
        {
            _canShoot = true;
            _startTimer = false;
            _nextFireTime = Time.time + _currentGunData.FireRate; // Update next allowed fire time
        }
    }

    public bool CanShoot()
    {
        return _canShoot;
    }

    public float GetFireRate()
    {
        return _currentGunData.FireRate;
    }

    public void ShootBullet()
    {
        if (!_canShoot || _isReloading) return;
        _canShoot = false;
        Bullet bullet = GameManager.Instance.Ref_PoolManager.GetBullet();
        bullet.transform.position = _bulletSpawnPoint;
        bullet.gameObject.SetActive(true);
        bullet.StartBullet(_currentGunData.Bullet_Speed, _currentGunData.Attack_Damage);
        _currentGun.Flash();

        _startTimer = true;


        _currentMag--;
        if (_currentMag <= 0)
        {
            ReloadGun();
        }
    }

    private void ReloadGun()
    {
        _isReloading = true;
        Reload_Image.fillAmount = 1;
        Reload_Image.gameObject.SetActive(true);
        Reload_Image.DOFillAmount(0, _currentGunData.Reload_Duration).OnComplete(() =>
        {
            _isReloading = false;
            _currentMag = _currentGunData.Magazine_Size;
            Reload_Image.gameObject.SetActive(false);
        });
    }
}
