using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera instance;

    [SerializeField]
    Transform cameraTransform;
    [SerializeField]
    SpaceSelectorDirectionProcessor myCursor;
    [SerializeField]
    int safeZoneRange;
    [SerializeField]
    float lerpStep = 0.01f;
    List<Vector2Int> currentSafeZone;
    Vector3 currentCameraTarget;
    bool cameraMoving = false, cameraInit = false;
    float currentLerp = 0.0f;
    float threshhold = 0.011f;

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

    private void Update()
    {
        if (cameraInit)
        {
            if (cameraMoving)
            {
                //if camera reaches destination, it stops
                if ((cameraTransform.position - currentCameraTarget).sqrMagnitude<=threshhold)
                {
                    cameraMoving = false;
                }
                else
                {
                    //move towards destination
                    Vector3 newPosition = Vector3.Lerp(cameraTransform.position, currentCameraTarget, lerpStep);
                    newPosition.z = cameraTransform.position.z;
                    cameraTransform.position = newPosition;
                }
            }
            if (!currentSafeZone.Contains(myCursor.HighlightPos))
            {
                //if the cursor moves out of the safe zone, create a new safezone and move the camera towards the middle
                currentSafeZone = GetRectangleAround(myCursor.HighlightPos, safeZoneRange);
                currentCameraTarget = IsoGrid.instance.ToWorldSpace(myCursor.HighlightPos);
                currentCameraTarget.z = cameraTransform.position.z;
                cameraMoving = true;
            }
        }
    }
    public void InitCamera()
    {
        currentSafeZone = GetRectangleAround(myCursor.HighlightPos, safeZoneRange);
        cameraInit = true;
    }
    /// <summary>
    /// gets a new list containging all tiles in a rectangle around given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private List<Vector2Int> GetRectangleAround(Vector2Int position,int range)
    {
        List<Vector2Int> spacesInRectangle = new List<Vector2Int>();

        for (int i = -range; i < range; i++)
        {
            for (int j = -range; j < range; j++)
            {
                spacesInRectangle.Add(new Vector2Int(position.x + i, position.y + j));
            }
        }
        return spacesInRectangle;
    }
}
