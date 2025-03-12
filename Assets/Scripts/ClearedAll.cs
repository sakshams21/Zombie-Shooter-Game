using UnityEngine;

public class ClearedAll : MonoBehaviour
{
    [SerializeField] private GameObject Loading_Go;
    public void BackToMainMenu()
    {
        Loading_Go.SetActive(true);
        LevelManager.Instance.BackToMainMenu();
    }
    public void QuitGame()
    {
        LevelManager.Instance.QuitGame();
    }
}
