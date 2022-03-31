using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileInformationSetup : MonoBehaviour
{
    public static TileInformationSetup instance;
    [SerializeField]
    GameObject TileInformationParent;
    [SerializeField]
    Image backgroundGUI, pictureGUI;
    [SerializeField]
    TextMeshProUGUI titleGUI, descriptionGUI,statsGUI;
    private void Awake()
    {
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
        GameEvents.instance.onEndPlayerTurn += HideGUIDescription;
    }
    private void OnDisable()
    {
        GameEvents.instance.onEndPlayerTurn -= HideGUIDescription;
    }
    /// <summary>
    /// apply information to hud
    /// </summary>
    /// <param name="picture"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="background"></param>
    /// <param name="describedUnit"></param>
    public void ApplyGUIDescription(Sprite picture, string title, string description, Color background, Unit describedUnit)
    {
        TileInformationParent.SetActive(true);
        backgroundGUI.color = background;
        descriptionGUI.text = description;
        titleGUI.text = title;
        pictureGUI.sprite = picture;
        statsGUI.text = UnitStatsToString(describedUnit);
    }
    /// <summary>
    /// apply information to hud
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="background"></param>
    /// <param name="describedUnit"></param>
    public void ApplyGUIDescription(string title, string description, Color background, Unit describedUnit)
    {
        TileInformationParent.SetActive(true);
        backgroundGUI.color = background;
        descriptionGUI.text = description;
        titleGUI.text = title;
        statsGUI.text = UnitStatsToString(describedUnit);
    }
    /// <summary>
    /// hide information from hud
    /// </summary>
    public void HideGUIDescription()
    {
        if(TileInformationParent!=null)
            TileInformationParent.SetActive(false);
    }
    private string UnitStatsToString(Unit unit)
    {
        //each unit has its own statistics it wants to display
        return unit.getStats();
    }
}
