using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Microlight.MicroBar.MicroBar PlayerHeath_Bar;



    private void Start()
    {
        GameManager.Instance.OnHeathChange += HealthUpdate;
        PlayerHeath_Bar.Initialize(100);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHeathChange -= HealthUpdate;
    }

    private void HealthUpdate(int currentHealth)
    {
        PlayerHeath_Bar.UpdateBar(currentHealth);
    }
}
