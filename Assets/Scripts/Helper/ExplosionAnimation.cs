using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    [SerializeField]
    AudioSource myExplosion;
    [SerializeField]
    float explosionDelay,animationDuration;
    [SerializeField]
    SpriteRenderer mySpriteRenderer;
    private void Start()
    {
        Destroy(this.gameObject, 10f);
        StartCoroutine(PlaySoundAfterDelay());
        StartCoroutine(DisappearAfterDelay());
    }
    /// <summary>
    /// Plays soundeffect after given delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySoundAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        myExplosion.Play();
    }    
    /// <summary>
    /// disables explosion sprite after animation has finished
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(animationDuration);
        mySpriteRenderer.enabled = false;
    }

}
