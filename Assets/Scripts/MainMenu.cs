using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using ZombiesShooter.MainMenu;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button Resume_Btn;
    [SerializeField] private GunData[] Guns;
    [SerializeField] private Transform GunSpawn_Transform;
    [SerializeField] private CanvasGroup Loading_CanvasGroup;

    private List<WeaponSelect> GunUI_List = new();
    private int lastGunSelected = 0;
    private int lastLevelSelected = 0;
    private void Awake()
    {
        lastGunSelected = PlayerPrefs.GetInt("GUN SELECTED", 0);
        lastLevelSelected = PlayerPrefs.GetInt("LAST LEVEL", 0);

    }
    private void Start()
    {
        for (int i = 0; i < Guns.Length; i++)
        {
            var gunUI = Instantiate(Guns[i].WeaponSelect_Prefab, GunSpawn_Transform);
            gunUI.index = i;
            GunUI_List.Add(gunUI);
        }

        WeaponChange(lastGunSelected);

        WeaponSelect.OnWeaponSelect += WeaponChange;

        Resume_Btn.gameObject.SetActive(lastLevelSelected > 0);
    }
    private void OnDestroy()
    {
        WeaponSelect.OnWeaponSelect -= WeaponChange;
    }

    private void WeaponChange(int index)
    {
        LevelManager.Instance.AssignGunData(Guns[index]);
        PlayerPrefs.SetInt("GUN SELECTED", index);

        for (int i = 0; i < GunUI_List.Count; i++)
        {
            GunUI_List[i].Select(i == index);
        }
    }


    public void PlayBtn(bool isnew)
    {
        Loading_CanvasGroup.alpha = 0;
        Loading_CanvasGroup.gameObject.SetActive(true);
        Loading_CanvasGroup.DOFade(1, 2f).OnComplete(() =>
        {
            LevelManager.Instance.LoadLevel(isnew ? 0 : lastLevelSelected);
        });

    }

    public void QuitGame()
    {
        LevelManager.Instance.QuitGame();
    }
}
