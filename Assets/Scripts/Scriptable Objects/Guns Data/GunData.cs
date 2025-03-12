using UnityEngine;
using ZombiesShooter.MainMenu;


[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public Weapon Weapon_Prefab;
    public WeaponSelect WeaponSelect_Prefab;
    public int Attack_Damage;
    public float Reload_Duration;

    /// <summary>
    /// Number of bullets per second
    /// </summary>
    public float FireRate;

    public int Magazine_Size;
    public float Bullet_Speed;
}
