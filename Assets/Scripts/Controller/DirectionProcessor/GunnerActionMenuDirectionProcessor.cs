using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEnums;
/// <summary>
/// return gunner-actions based on Menu-Highlight-Selection
/// MenuSelectionHighlight is the only Selection-Highlight that is not singleton.
/// </summary>
public class GunnerActionMenuDirectionProcessor : ActionMenuDirectionProcessor
{
    public override HeroActions returnAction()
    {
        if (currentSelection == 0)
        {
            return HeroActions.shoot;
        }
        if (currentSelection == 1)
        {
            return HeroActions.frag;
        }
        return HeroActions.wait;
    }
}
