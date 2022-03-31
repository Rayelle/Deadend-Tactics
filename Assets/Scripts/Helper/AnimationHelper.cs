using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public static AnimationHelper instance;
    [SerializeField]
    float fadeStrength=0.01f, fadeInterval=0.01f,smokeCloudAnimationDuration=1.0f,smokeCloudYOffset=0.55f;
    [SerializeField]
    GameObject smokeCloudPrefab;
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
    /// <summary>
    /// flash an image red then return to normal
    /// </summary>
    /// <param name="flashing"></param>
    public void FlashImageRed(SpriteRenderer flashing)
    {
        StartCoroutine(RedFlash(flashing));
    }
    /// <summary>
    /// fade out green and blue of a given sprite renderer then fade it back in
    /// </summary>
    /// <param name="flashing"></param>
    /// <returns></returns>
    private IEnumerator RedFlash(SpriteRenderer flashing)
    {
        while (flashing.color.g > 0.01f||flashing.color.b > 0.01f)
        {
            Color currentColor = flashing.color;
            currentColor.g -= fadeStrength;
            currentColor.b -= fadeStrength;
            flashing.color = currentColor;
            yield return new WaitForSeconds(fadeInterval);

        }
        Color finalColor = flashing.color;
        finalColor.g = 0;
        finalColor.b = 0;
        flashing.color = finalColor;

        while (flashing.color.g < 0.99f || flashing.color.b < 0.99f)
        {
            Color currentColor = flashing.color;
            currentColor.g += fadeStrength;
            currentColor.b += fadeStrength;
            flashing.color = currentColor;
            yield return new WaitForSeconds(fadeInterval);

        }
        finalColor = flashing.color;
        finalColor.g = 1;
        finalColor.b = 1;
        flashing.color = finalColor;
    }
    public void CreateSmokeCloud(Vector3 position)
    {
        GameObject newSmokeCloud = GameObject.Instantiate(smokeCloudPrefab, new Vector3(position.x,position.y+smokeCloudYOffset), Quaternion.identity, this.transform);
        Destroy(newSmokeCloud, smokeCloudAnimationDuration);
    }
}
