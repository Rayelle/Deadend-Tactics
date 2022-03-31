using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrowls : MonoBehaviour
{
    [SerializeField]
    AudioSource myAS;
    [SerializeField]
    AudioClip[] allGroans;
    System.Random rnd;
    [SerializeField]
    float averageDelay, maxDelayDispersion;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random(Time.frameCount * Time.frameCount);
        StartCoroutine(PlayGroaningSounds());
    }
    /// <summary>
    /// plays random groaning sounds forever
    /// coroutines are stopped if onDisable is called
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGroaningSounds()
    {
        while (true)
        {
            if (rnd.NextDouble() >= 0.5d)
            {
                yield return new WaitForSeconds(averageDelay + (float)rnd.NextDouble() * maxDelayDispersion);
            }
            else
            {
                yield return new WaitForSeconds(averageDelay - (float)rnd.NextDouble() * maxDelayDispersion);
            }
            if (!myAS.isPlaying)
            {
                myAS.clip = allGroans[rnd.Next(allGroans.Length)];
                myAS.Play();
            }
        }
    }
}
