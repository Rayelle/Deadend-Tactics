using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePrefab : MonoBehaviour
{
    [SerializeField]
    Image upgradeBackground;
    [SerializeField]
    TextMeshProUGUI upgradeName, upgradeDescription;
    /// <summary>
    /// displays upgrade information in upgrade-screen
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="backgroundColor"></param>
    public void setUpNewUpgrade(string name, string description,Color backgroundColor)
    {
        upgradeName.text = name;
        upgradeDescription.text = description;
        upgradeBackground.color = backgroundColor;
    }
}
