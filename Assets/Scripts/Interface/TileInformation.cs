using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInformation : MonoBehaviour
{
    [SerializeField]
    string title, description;
    [SerializeField]
    Color background;
    [SerializeField]
    Sprite picture;
    [SerializeField]
    Unit describedUnit;
    /// <summary>
    /// applies serialized inforamtion to the hud
    /// </summary>
    public void ApplyTileInformation()
    {
        if (picture == null)
        {
            TileInformationSetup.instance.ApplyGUIDescription(title,description,background,describedUnit);
        }
        else
        {
            TileInformationSetup.instance.ApplyGUIDescription(picture, title, description, background,describedUnit);
        }
    }
}
