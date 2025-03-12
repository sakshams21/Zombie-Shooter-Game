using System;
using UnityEngine;

/// <summary>
/// part of 
/// </summary>
namespace ZombiesShooter.MainMenu
{
    public class WeaponSelect : MonoBehaviour
    {
        [SerializeField] private GameObject Equipped_Go;
        [SerializeField] private GameObject Equip_Go;

        public int index;

        public static event Action<int> OnWeaponSelect;
        public void WeaponSelectMethod()
        {
            OnWeaponSelect?.Invoke(index);
        }

        public void Select(bool status)
        {
            Equip_Go.SetActive(!status);
            Equipped_Go.SetActive(status);
        }
    }
}