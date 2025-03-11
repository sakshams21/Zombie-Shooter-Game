using System.Collections;
using UnityEngine;


/// <summary>
/// Handles shooting of bullets
/// Clip management
/// 
/// </summary>
public class Weapon : MonoBehaviour
{

    [SerializeField] private GunType CurrentGunType;
    [SerializeField] private GameObject MuzzleFlashLocation;
    private WaitForSeconds _waitForFlash = new WaitForSeconds(0.2f);


    private void Start()
    {
        MuzzleFlashLocation.SetActive(false);
    }

    public Vector3 GetBulletSpawnPoint()
    {
        return MuzzleFlashLocation.transform.position;
    }


    public void Flash()
    {
        StartCoroutine(Flash_Coro());
    }

    IEnumerator Flash_Coro()
    {
        MuzzleFlashLocation.SetActive(true);
        yield return _waitForFlash;
        MuzzleFlashLocation.SetActive(false);
    }
}
