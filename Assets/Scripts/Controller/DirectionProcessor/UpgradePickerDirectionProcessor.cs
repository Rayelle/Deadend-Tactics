using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickerDirectionProcessor : DirectionProcessor
{
    public static UpgradePickerDirectionProcessor instance;
    [SerializeField]
    GameObject myUpgradeMenu;
    //[SerializeField]
    //GameObject[] myUpgradeChoiceSpawnPositions;
    [SerializeField]
    GameObject[] allArrows;
    short currentUpgradeSelection = 0; //goes from zero to three
    GameObject[] currentUpgradesGameObjects = new GameObject[3];
    Upgrade[] currentUpgradeOptions;
    [SerializeField]
    private UpgradePrefab[] allUpgradePositions;


    private void Awake()
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
    /// <summary>
    /// Displays Upgrade-menu with a random selection of three upgrades.
    /// </summary>
    /// <param name="upgradeOptions"></param>
    public void StartUpgradeMenu(Upgrade[] upgradeOptions)
    {
        if (myUpgradeMenu == null)
        {
            return;
        }
        myUpgradeMenu.SetActive(true);
        currentUpgradeOptions = upgradeOptions;
        for (int i = 0; i < allUpgradePositions.Length; i++)
        {
            allUpgradePositions[i].setUpNewUpgrade(upgradeOptions[i].UpgradeName, upgradeOptions[i].UpgradeDescription, upgradeOptions[i].UpgradeBackgroundColor);
        }

        currentUpgradeSelection = 0;
        allArrows[currentUpgradeSelection].SetActive(true);
    }
    /// <summary>
    /// stop displaying upgrade menu.
    /// </summary>
    public void EndUpgradeMenu()
    {
        foreach (GameObject item in currentUpgradesGameObjects)
        {
            Destroy(item);
        }
        allArrows[currentUpgradeSelection].SetActive(false);
        myUpgradeMenu.SetActive(false);
    }
    public override void EndHighlight()
    {
        
    }
    /// <summary>
    /// activate currently selected update and fire EndMap-event.
    /// </summary>
    public void PickCurrentUpgrade()
    {
        UpgradeManager.instance.AddUpgrade(currentUpgradeOptions[currentUpgradeSelection]);
        EndUpgradeMenu();
        GameEvents.instance.EndMap();
    }
    /// <summary>
    /// change current upgrade-selection and upgrade-selection-arrow
    /// </summary>
    public override void MoveHighlightLeft()
    {
        if (!(currentUpgradeSelection - 1 < 0))
        {
            allArrows[currentUpgradeSelection].SetActive(false);
            currentUpgradeSelection--;
            allArrows[currentUpgradeSelection].SetActive(true);
            AudioManager.instance.PlayMenuMoveSound();
        }
    }
    /// <summary>
    /// change current upgrade-selection and upgrade-selection-arrow
    /// </summary>
    public override void MoveHighlightRight()
    {
        if (!(currentUpgradeSelection + 1 >= allArrows.Length))
        {
            allArrows[currentUpgradeSelection].SetActive(false);
            currentUpgradeSelection++;
            allArrows[currentUpgradeSelection].SetActive(true);
            AudioManager.instance.PlayMenuMoveSound();

        }
    }

}
