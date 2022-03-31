using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickerInputProcessor : InputProcessor
{
    public static UpgradePickerInputProcessor instance;
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
    /// pick currently selected upgrade
    /// </summary>
    public override void Accept()
    {
        UpgradePickerDirectionProcessor.instance.PickCurrentUpgrade();
        AudioManager.instance.PlayMenuAcceptSound();
    }
    /// <summary>
    /// do nothing
    /// </summary>
    public override void Refuse()
    {

    }
}
