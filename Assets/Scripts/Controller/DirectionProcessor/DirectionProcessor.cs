using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class is responsible for processing directional input.
/// </summary>
public class DirectionProcessor : MonoBehaviour
{
    /// <summary>
    /// Initializes current selection-processor.
    /// </summary>
    /// <param name="highlightStartingPosition"></param>
    public virtual void StartHighlight(Vector2Int highlightStartingPosition)
    {
        
    }
    /// <summary>
    /// Deinitializes current selection-processor.
    /// </summary>
    public virtual void EndHighlight()
    {

    }
    /// <summary>
    /// moves directional input upwards.
    /// </summary>
    public virtual void MoveHighlightUp()
    {

    }
    /// <summary>
    /// moves directional input downwards.
    /// </summary>
    public virtual void MoveHighlightDown()
    {

    }
    /// <summary>
    /// moves directional input leftwards.
    /// </summary>
    public virtual void MoveHighlightLeft()
    {

    }
    /// <summary>
    /// moves directional input rightwards.
    /// </summary>
    public virtual void MoveHighlightRight()
    {

    }

}
