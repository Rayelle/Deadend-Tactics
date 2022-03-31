using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// heals friendly heroes around the medic when activated.
/// </summary>
public class MedicHeal : MonoBehaviour
{
    public static MedicHeal instance;
    [SerializeField]
    Hero myMedic;
    private bool blockActive = false;
    [SerializeField]
    AudioSource healAudioSource;
    [SerializeField]
    float healDuration, timeBetweenHealSounds;
    [SerializeField]
    GameObject healCloudPrefab;
    //public Hero Medic { set => myMedic = value; }

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
    /// heals friendly heroes around the medic
    /// </summary>
    public void ActivateMedicHeal()
    {
        StartCoroutine(PlayHealAnimationAndSound());
    }
    /// <summary>
    /// returns a List of all spaces in given range around the medic
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    private List<Vector2Int> SurroundingSpaces(int range)
    {
        List<Vector2Int> spacesInRange = new List<Vector2Int>();
        spacesInRange.Add(myMedic.gridPosition);
        for (int j = 0; j < range; j++)
        {
            int numberOfLoops = spacesInRange.Count;
            for (int i = 0; i < numberOfLoops; i++)
            {
                Vector2Int[] adjacenstSpaces = new Vector2Int[4];
                adjacenstSpaces[0] = new Vector2Int(spacesInRange[i].x + 1, spacesInRange[i].y);
                adjacenstSpaces[1] = new Vector2Int(spacesInRange[i].x - 1, spacesInRange[i].y);
                adjacenstSpaces[2] = new Vector2Int(spacesInRange[i].x, spacesInRange[i].y + 1);
                adjacenstSpaces[3] = new Vector2Int(spacesInRange[i].x, spacesInRange[i].y - 1);
                foreach (Vector2Int position in adjacenstSpaces)
                {
                    if (IsoGrid.instance.IsInsideBounds(position))
                    {
                        spacesInRange.Add(position);
                    }
                }

            }
        }
        return spacesInRange;
    }
    /// <summary>
    /// Plays medics heal animation and sound effect, creates heal-smoke at other healed friends
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayHealAnimationAndSound()
    {
        InputManager.instance.DeactivatePlayerControl();
        //start animation
        if (myMedic.MyAnimator != null)
        {
            myMedic.MyAnimator.SetBool("healing", true);
        }
        yield return new WaitForSeconds(timeBetweenHealSounds);
        //play heal sound
        if (healAudioSource != null)
        {
            healAudioSource.Play();

        }
        yield return new WaitForSeconds(timeBetweenHealSounds);
        //play heal sound again
        if (healAudioSource != null)
        {
            healAudioSource.Play();
        }
        //look for heroes inside surrounding tiles and heal them while displaying a heal-smoke-cloud
        foreach (Vector2Int position in SurroundingSpaces(HeroStatistics.MedicHealRange))
        {
            if (MapContent.instance.Dictionary.ContainsKey(position))
            {
                if (MapContent.instance.SpaceContainsHero(position))
                {
                    MapContent.instance.Dictionary[position].GetHealed(HeroStatistics.MedicHealAmout);
                    if(myMedic.gridPosition!=position)
                        GameObject.Instantiate(healCloudPrefab, IsoGrid.instance.ToWorldSpace(position), Quaternion.identity ,this.transform);

                }
            }
        }
        yield return new WaitForSeconds(healDuration - timeBetweenHealSounds * 2);
        //stop animation
        if (myMedic.MyAnimator != null)
        {
            myMedic.MyAnimator.SetBool("healing", false);
        }
        InputManager.instance.ActivatePlayerControl();
    }
}
