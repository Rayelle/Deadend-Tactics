using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager instance;
    [SerializeField]
    private GameObject currentMap,mapPrefab;
    MapData nextMapData;
    [SerializeField]
    List<MapData> allMaps = new List<MapData>();
    int currentLevel = 0, stdZombieMinimumNumber=2;
    [SerializeField]
    int maximumLevels = 5;
    bool finalLevel = false;

    public MapData CurrentMapData { set => nextMapData = value; }
    public bool FinalLevel { get => finalLevel; }
    public int CurrentLevel { get => currentLevel;  }

    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        GameEvents.instance.onEndMap += StartNewRound;
    }
    private void OnDisable()
    {
        GameEvents.instance.onEndMap -= StartNewRound;
    }
    /// <summary>
    /// start a new round and loads a new map
    /// </summary>
    private void StartNewRound()
    {
        if (finalLevel) //only used if cheatin, usually the ai-manager ends the game
        {
            SceneManager.LoadScene(2);
            return;
        }
        currentLevel++;
        //change current music intensity to reflect on progress
        AudioManager.instance.ChangeMusicIntensity(currentLevel);
        //set up new random generator and pick a random map
        System.Random rnd = new System.Random((int)(Time.time*Time.time)*Time.frameCount);
        
        int mapIterator;
        mapIterator = rnd.Next(allMaps.Count);
        nextMapData = allMaps[mapIterator];
        allMaps.RemoveAt(mapIterator);
        //let each hero regain some health
        foreach (Hero current in HeroManager.instance.AllHeroes)
        {
            current.RestBetweenRounds();
        }
        //destroy old map
        RoundManager.instance.CleanUp();
        GameObject.Destroy(currentMap);
        //second part of round start is performed after a delay
        StartCoroutine(StartRoundAfterDelay());

    }
    /// <summary>
    /// starting a new round has a short delay
    /// delay is necesscary so that all events are done when round is started
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRoundAfterDelay()
    {
        
        yield return new WaitForSeconds(0.01f);
        //in the last round finalLevel becomes true
        if (currentLevel >= maximumLevels)
        {
            finalLevel = true;
        }

        //create a new map based on the mapdata selected
        currentMap = GameObject.Instantiate(mapPrefab);
        yield return new WaitForSeconds(0.01f);

        MapGenerator.instance.GenerateMap(nextMapData.GridWidth,nextMapData.GridHeight);

        Vector2Int[] heroStartingPositions = nextMapData.GetHeroSpawn();
        Unit[] allHeroes = HeroManager.instance.AllHeroes.ToArray();
        int i = 0;
        foreach (Unit currentHero in allHeroes)
        {
            currentHero.RegisterThisUnit(heroStartingPositions[i]);
            currentHero.MoveUnitTo(heroStartingPositions[i]);
            i++;
        }
        //number of std and special zombies is determined by level number, final level gets a special treatment
        int numberOfStdZombies = stdZombieMinimumNumber + currentLevel;
        if (finalLevel)
        {
            numberOfStdZombies--;
        }
        for (int j = 0; j < numberOfStdZombies; j++)
        {
            Vector2Int nextZombieSpawn = nextMapData.GetOneZombieSpawn();
            if (nextZombieSpawn == new Vector2Int(-1, -1))
            {
                break;
            }
            //spawn zombies
            RoundManager.instance.CreateStandardEnemy(nextZombieSpawn);
        }
        int numberOfSpecialZombies;
        if (finalLevel)
        {
            numberOfSpecialZombies = 0;
        }
        else
        {
             numberOfSpecialZombies = 1 + currentLevel / 2;
        }
        for (int j = 0; j < numberOfSpecialZombies; j++)
        {
            Vector2Int nextZombieSpawn = nextMapData.GetOneZombieSpawn();
            if (nextZombieSpawn == new Vector2Int(-1, -1))
            {
                break;
            }
            //spawn zombies

            RoundManager.instance.CreateSpecialEnemy(nextZombieSpawn);

        }
        if (finalLevel)
        {
            Vector2Int nextZombieSpawn = nextMapData.GetOneZombieSpawn();
            if (!(nextZombieSpawn == new Vector2Int(-1, -1)))
            {
                RoundManager.instance.CreateBossEnemy(nextZombieSpawn);
            }
        }
        foreach (Vector2Int position in nextMapData.AllObstaclePositions)
        {
            //create obstacles
            RoundManager.instance.CreateObstacle(position);
        }

        //start the new turn
        InputManager.instance.CurrentInputProcessor = HeroSelectionInputProcessor.instance;
        InputManager.instance.CurrentDirectionInputProcessor = SpaceSelectorDirectionProcessor.instance;
        SpaceSelectorDirectionProcessor.instance.StartHighlight(new Vector2Int(1, 1));
        HeroManager.instance.ReReadyHeroes();

        InputManager.instance.ActivatePlayerControl();
    }
}
