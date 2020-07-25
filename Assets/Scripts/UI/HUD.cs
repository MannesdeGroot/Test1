using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Weapon Info")]
    private static GameObject weaponInfoPanel;
    private static Text ammoText;

    private void Start()
    {
        weaponInfoPanel = GameObject.Find("Weapon Info");
        ammoText = GameObject.Find("Ammo Text").GetComponent<Text>();
    }

    public static void SetAmmoText(int ammo, int clipSize)
    {
        //ammoText.text = $"{ammo}/{clipSize}";
    }
}
