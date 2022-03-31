using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//quick and dirty class for a small ending animation
public class ExplosionSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab,deathCloudPrefab;
    [SerializeField]
    Vector3 downLeft;
    System.Random rnd;
    [SerializeField]
    GameObject[] allTargets;

    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random(Time.frameCount*Time.frameCount);
        StartCoroutine(spawnRandomExplosions());
        StartCoroutine(destroyZombies(2.7f));
        Destroy(this.gameObject, 2.8f);
    }
    /// <summary>
    /// spawns explosions around upwards and rightwards of vector downLeft
    /// </summary>
    /// <returns></returns>
    private IEnumerator spawnRandomExplosions()
    {
        while (true)
        {
        yield return new WaitForSeconds(0.3f);
        Vector3 newPosition = new Vector3(downLeft.x + (float)(rnd.NextDouble() * 5d), downLeft.y +(float)(rnd.NextDouble() * 2.5d));
        GameObject.Instantiate(explosionPrefab, newPosition, Quaternion.identity);
        }
    }
    /// <summary>
    /// removes all zombies in the collection and replaces them with death somke clouds
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator destroyZombies(float delay)
    {
        yield return new WaitForSeconds((delay -0.01f) - (0.1f * allTargets.Length));
        for (int i = 0; i < allTargets.Length; i++)
        {

            GameObject.Destroy(GameObject.Instantiate(deathCloudPrefab, allTargets[i].transform.position, Quaternion.identity),1f);
            Destroy(allTargets[i]);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
