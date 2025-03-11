using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public ZombieType Type;
    public int Health;
    public float Movement_Speed;
    public float Attack_Cooldown;
    public int Attack_Damage;
}
