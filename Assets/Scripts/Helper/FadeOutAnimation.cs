using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAnimation : MonoBehaviour
{
    [SerializeField]
    float disapearanceStrength=0.05f, disapearanceInterval=0.01f,delay=0;
    [SerializeField]
    SpriteRenderer mySpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, 5f);
        StartCoroutine(Disapear());
    }
    /// <summary>
    /// fades out image in sprite renderer
    /// </summary>
    /// <returns></returns>
    private IEnumerator Disapear()
    {
        yield return new WaitForSeconds(delay);
        while (mySpriteRenderer.color.a > 0.01f)
        {
            yield return new WaitForSeconds(disapearanceInterval);
            Color fading = mySpriteRenderer.color;
            fading.a -= disapearanceStrength;
            mySpriteRenderer.color = fading;
        }
        Color final = mySpriteRenderer.color;
        final.a -= 0;
        mySpriteRenderer.color = final;
    }
}
