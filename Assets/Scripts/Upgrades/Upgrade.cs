using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    private string upgradeName, upgradeDescription;
    [SerializeField]
    private Color upgradeBackgroundColor;
    public string UpgradeName { get => upgradeName; }
    public string UpgradeDescription { get => upgradeDescription; }
    public Color UpgradeBackgroundColor { get => upgradeBackgroundColor; }

    // Start is called before the first frame update

    public virtual void ApplyUpgrade()
    {

    }
}
