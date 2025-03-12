using System;
using TMPro;
using UnityEngine;
using Microlight.MicroBar;

/// <summary>
/// Handles UI Interation
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private MicroBar PlayerHeath_Bar;
    [SerializeField] private Canvas Pause_Canvas;
    [SerializeField] private TextMeshProUGUI Ammo_Text;
    [SerializeField] private TextMeshProUGUI Objective_Text;

    [SerializeField] private Canvas FailScreen_Canvas;

    private void Start()
    {
        GameManager.Instance.OnHeathChange += HealthUpdate;
        GameManager.Instance.OnScoreChange += ObjectiveUpdate;
        WeaponManager.OnBulletShoot += AmmoUpdate;

        PlayerHeath_Bar.Initialize(100);
    }

    public void ZombieStartSpawning()
    {
        GameManager.Instance.ZombieInitialize();
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnHeathChange -= HealthUpdate;
        WeaponManager.OnBulletShoot -= AmmoUpdate;
        GameManager.Instance.OnScoreChange -= ObjectiveUpdate;
    }

    public void ObjectiveUpdate(int current, int max)
    {
        Objective_Text.text = $"Objective: {current}/{max}";
    }

    public void HealthUpdate(int currentHealth)
    {
        PlayerHeath_Bar.UpdateBar(currentHealth);
    }

    public void AmmoUpdate(int ammoCount, int maxCount)
    {
        Ammo_Text.text = $"Ammo: {ammoCount}/{maxCount}";
    }

    public void PauseScreen(bool status)
    {
        Pause_Canvas.enabled = status;
    }
    public void ShowFailScreen()
    {
        FailScreen_Canvas.enabled = true;
    }


    public void BackToMainMenu()
    {
        GameManager.Instance.TimeScaleChange(1);
        LevelManager.Instance.BackToMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
