using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating units
public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    [SerializeField]
    private AIManager mainAIManager;



    [SerializeField]
    private GameObject tankPrefab,gunnerPrefab,medicPrefab,stdZombiePrefab,obstaclePrefab,spitterPrefab,grabberPrefab,spawnerPrefab,dogPrefab,bossPrefab;
    List<GameObject> currentMapObjects= new List<GameObject>();
    GameObject[] allSpecialZombies = new GameObject[4];

    private int turn=0;
    private bool playerTurn = true,firstUpdate=true;
    System.Random rnd = new System.Random();
    public bool PlayerTurn { get => playerTurn; }

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
    private void OnDisable()
    {
        GameEvents.instance.onEndPlayerTurn -= EndPlayerTurn;
        GameEvents.instance.onEndAITurn -= EndAITurn;
        //GameEvents.instance.onEndMap -= SetUpNewMap;
    }

    private void Start()
    {
        //generate a new map
        MapGenerator.instance.GenerateMap();
        allSpecialZombies[0] = dogPrefab;
        allSpecialZombies[1] = spitterPrefab;
        allSpecialZombies[2] = grabberPrefab;
        allSpecialZombies[3] = spawnerPrefab;

        GameEvents.instance.onEndPlayerTurn += EndPlayerTurn;
        GameEvents.instance.onEndAITurn += EndAITurn;

        SpaceSelectorDirectionProcessor.instance.StartHighlight(new Vector2Int(1, 1));


    }
    private void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            //all heroes are created in the first map which is then skipped
            CreateUnit(gunnerPrefab, new Vector2Int(4, 0));
            CreateUnit(tankPrefab, new Vector2Int(5, 0));
            CreateUnit(medicPrefab, new Vector2Int(3, 0));

            StartCoroutine(StartFirstCampaginMap());
            StartCoroutine(StartCamera());

        }
    }
    //destroy all objects under this map
    public void CleanUp()
    {
        foreach (GameObject current in currentMapObjects)
        {
            Destroy(current);
        }
    }
    /// <summary>
    /// Create a new Unit at a given position and register in map components
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="positionInGrid"></param>
    private void CreateUnit(GameObject prefab,Vector2Int positionInGrid)
    {
        Unit newUnit = (GameObject.Instantiate(prefab, IsoGrid.instance.ToWorldSpace(positionInGrid), this.transform.rotation,this.transform)).GetComponent<Unit>();
        newUnit.RegisterThisUnit(positionInGrid);
        newUnit.MoveUnitTo(positionInGrid);
        if(newUnit.MyUnitType==HeroEnums.UnitType.enemy||newUnit.MyUnitType==HeroEnums.UnitType.obstacle)
        currentMapObjects.Add(newUnit.gameObject);

    }
    public void CreateStandardEnemy(Vector2Int positionInGrid)
    {
        CreateUnit(stdZombiePrefab, positionInGrid);
    }
    public void CreateObstacle(Vector2Int positionInGrid)
    {
        CreateUnit(obstaclePrefab, positionInGrid);
    }
    public void CreateSpecialEnemy(Vector2Int positionInGrid)
    {
        CreateUnit(allSpecialZombies[rnd.Next(4)], positionInGrid);
    }
    public void CreateBossEnemy(Vector2Int positionInGrid)
    {
        CreateUnit(bossPrefab, positionInGrid);
    }
    private void EndPlayerTurn()
    {
        playerTurn = false;
        StartCoroutine(mainAIManager.RunAIAfterDelay());
    }
    private void EndAITurn()
    {
        turn++;
        playerTurn = true;

    }
    /// <summary>
    /// start camera after delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(0.05f);
        FollowCamera.instance.InitCamera();
    }    
    /// <summary>
    /// start round after delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartFirstCampaginMap()
    {
        yield return new WaitForSeconds(0.1f);
        GameEvents.instance.EndMap();
    }
}
