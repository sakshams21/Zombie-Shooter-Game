using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace ZombieShooter
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private WeaponManager Ref_WeaponManager;


        private Controls _playerControls;

        private WaitForSeconds _bulletShootWait;

        private Coroutine _shootCoroutine;
        private void Awake()
        {
            _playerControls = new Controls();
        }

        private void OnEnable()
        {
            _playerControls.Player.Attack.performed += Shoot;
            _playerControls.Player.Attack.canceled += StopShoot;
            _playerControls.Player.Attack.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Player.Attack.performed -= Shoot;
            _playerControls.Player.Attack.canceled += StopShoot;
            _playerControls.Disable();
        }


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

        private void Shoot(InputAction.CallbackContext context)
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

        private void StopShoot(InputAction.CallbackContext context)
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
}