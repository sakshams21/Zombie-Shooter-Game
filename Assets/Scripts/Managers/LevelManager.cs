using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Persistent across screen carries the weapon equip data and loads level
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GunData EquippedGunData;
    [SerializeField] private LevelDataList Level_Data;
    public int CurrentLevel { get; private set; }

    public int TotalScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LevelCleared()
    {
        CurrentLevel += 1;
        LoadLevel(CurrentLevel);
    }


    public void LoadLevel(int level)
    {
        CurrentLevel = level;
        PlayerPrefs.SetInt("LAST LEVEL", CurrentLevel);
        SceneManager.LoadSceneAsync(1);
    }


    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AssignGunData(GunData data)
    {
        EquippedGunData = data;
    }

    public LevelData GetCurrentLevelData()
    {
        return Level_Data.Data[CurrentLevel];
    }
}