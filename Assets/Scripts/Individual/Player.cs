using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private WeaponManager Ref_WeaponManager;

    private WaitForSeconds _bulletShootWait;

    private Coroutine _shootCoroutine;


    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            if (Ref_WeaponManager.CanShoot())
            {
                Ref_WeaponManager.ShootBullet();
            }
            yield return _bulletShootWait;
        }
    }

    public void Shoot(ref InputAction.CallbackContext context)
    {
        _bulletShootWait = new WaitForSeconds(Ref_WeaponManager.GetFireRate());
        if (context.interaction is HoldInteraction)
        {
            if (_shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(ShootContinuously());
            }
        }
        else if (context.interaction is TapInteraction)
        {
            Ref_WeaponManager.ShootBullet();
        }
    }

    public void StopShoot(ref InputAction.CallbackContext context)
    {
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }
    }


    public void Damage(int damage)
    {
        GameManager.Instance.HealthManager(damage);
    }
}
