using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class responsible for managing heroes health bars
public class HeroHealthBarManager : MonoBehaviour
{
    public static HeroHealthBarManager instance;
    [SerializeField]
    private HealthBar[] heroHealthBars;
    private int healthIterator=0;
    [SerializeField]
    Image[] allHealthBarImages;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        GameEvents.instance.onSpawnUnit += CheckNewUnit;
        GameEvents.instance.onEndMap += DisplayHealthBars;
    }
    private void OnDisable()
    {
        GameEvents.instance.onSpawnUnit -= CheckNewUnit;
        GameEvents.instance.onEndMap -= DisplayHealthBars;
    }

    /// <summary>
    /// connect new Hero with one free healthbar
    /// only works for up to 3 heroes
    /// </summary>
    /// <param name="newUnit"></param>
    private void CheckNewUnit(Unit newUnit)
    {
        if (newUnit.MyUnitType==HeroEnums.UnitType.gunner|| newUnit.MyUnitType == HeroEnums.UnitType.medic|| newUnit.MyUnitType == HeroEnums.UnitType.tank)
        {
            if (healthIterator <= 2)
            {
                Hero newUnitHero = newUnit.GetComponent<Hero>();
                heroHealthBars[healthIterator].gameObject.SetActive(true);
                newUnitHero.MyHealthBar = heroHealthBars[healthIterator];
                newUnitHero.MyHealthBar.SetMaxHealth(newUnit.MaxHealth);
                newUnitHero.MyHealthBar.SetHealth(newUnit.MaxHealth);
                healthIterator++;
            }
        }
    }
    /// <summary>
    /// Hide image of the heroes health bars
    /// </summary>
    public void HideHealthBars()
    {
        for (int i = 0; i < allHealthBarImages.Length; i++)
        {
            Color noAlpha = allHealthBarImages[i].color;
            noAlpha.a = 0;
            allHealthBarImages[i].color = noAlpha;
        }
    }
    /// <summary>
    /// Display the heroes health bars
    /// </summary>
    private void DisplayHealthBars()
    {
        for (int i = 0; i < allHealthBarImages.Length; i++)
        {
            Color noAlpha = allHealthBarImages[i].color;
            noAlpha.a = 255;
            allHealthBarImages[i].color = noAlpha;
        }
    }
}
