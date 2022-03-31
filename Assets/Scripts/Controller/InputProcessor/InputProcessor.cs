using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls what kind of function input-manager calls on accept-button or back-button.
/// Class should always be overriden by child-class.
/// </summary>
public class InputProcessor : MonoBehaviour
{
    //make start method to initialise all input-processors

    public virtual void Init()
    {

    }

    /// <summary>
    /// Happens when accept button is presed
    /// </summary>
    public virtual void Accept()
    {

    }
    /// <summary>
    /// Happens when back button is pressed
    /// </summary>
    public virtual void Refuse()
    {

    }
}
