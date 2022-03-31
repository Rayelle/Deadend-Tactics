using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private List<Upgrade> activeUpgrades = new List<Upgrade>();
    private List<Upgrade> inactiveUpgrades = new List<Upgrade>();
    [SerializeField]
    private Upgrade[] allUpgrades;

    private Upgrade[] currentRandomUpgrades;

    void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {

        foreach (Upgrade current in allUpgrades)
        {
            inactiveUpgrades.Add(current);
        }
    }
    /// <summary>
    /// Starts the upgrade screen and chooses new upgrade to display
    /// </summary>
    public void StartUpgradeScreen()
    {
        HeroHealthBarManager.instance.HideHealthBars();

        AudioManager.instance.FadeInDroneSound();
        currentRandomUpgrades = getThreeRandomUpgrades();

        
        InputManager.instance.CurrentInputProcessor = UpgradePickerInputProcessor.instance;
        InputManager.instance.CurrentDirectionInputProcessor = UpgradePickerDirectionProcessor.instance;
        UpgradePickerDirectionProcessor.instance.StartUpgradeMenu(currentRandomUpgrades);

        
        TileInformationSetup.instance.HideGUIDescription();

    }
    /// <summary>
    /// give thre different random upgrades
    /// </summary>
    /// <returns></returns>
    public Upgrade[] getThreeRandomUpgrades()
    {
        Upgrade[] randomUpgrades = new Upgrade[3];
        System.Random rnd = new System.Random((int)(System.DateTime.Now.Millisecond * Time.time % DateTime.Now.Hour));
        int remainingUpgrades = inactiveUpgrades.Count;

        HashSet<Upgrade> partOfSelection = new HashSet<Upgrade>();

        for (int j = 0; j < 3; j++)
        {
            if (remainingUpgrades > 0)
            {

                int i = rnd.Next(remainingUpgrades);
                randomUpgrades[j] = inactiveUpgrades[i];
                
                remainingUpgrades--;
            }
        }
        return randomUpgrades;
    }
    /// <summary>
    /// add an upgrade to the active upgrades
    /// </summary>
    /// <param name="newUpgrade"></param>
    public void AddUpgrade(Upgrade newUpgrade)
    {
        if (!activeUpgrades.Contains(newUpgrade))
        {
            activeUpgrades.Add(newUpgrade);
            inactiveUpgrades.Remove(newUpgrade);
            newUpgrade.ApplyUpgrade();
        }
    }
}
