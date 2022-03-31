using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//using tutorial: https://www.youtube.com/watch?v=gx0Lt4tCDE0


public class AIManager : MonoBehaviour
{
    private List<ZombieStateMachine> allAI = new List<ZombieStateMachine>();

    private int currentAI;

    private float waitingTime=0.1f;
    //bool levelZero=true;

    private void Start()
    {
        GameEvents.instance.onSpawnUnit += RegisterNewUnit;
        GameEvents.instance.onUnitDoneMoving += MoveNextUnit;
        GameEvents.instance.onDestroyUnit += RemoveUnit;
    }

    private void OnDisable()
    {
        GameEvents.instance.onSpawnUnit -= RegisterNewUnit;
        GameEvents.instance.onUnitDoneMoving -= MoveNextUnit;
        GameEvents.instance.onDestroyUnit -= RemoveUnit;
    }
    /// <summary>
    /// Start running each AI-behaviour after 0.1 seconds delay.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunAIAfterDelay()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (allAI != null && allAI.Count != 0) //if- and else-clause might be unnesscescary
        {
            currentAI = 0;
            while (allAI[currentAI] == null)
            {
                currentAI++;
                if (currentAI >= allAI.Count)
                {
                    EndCurrentTurn();
                }
            }
            allAI[currentAI].Run();
        }
        else
        {
            //if AI is not set up properly
            //try running AI again
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void MoveNextUnit()
    {
        currentAI++;
        if (currentAI >= allAI.Count)
        {
            EndCurrentTurn();
        }
        else
        {

            while (allAI[currentAI] == null)
            {
                currentAI++;
                if (currentAI >= allAI.Count)
                {
                    EndCurrentTurn();
                }
            }
            //run next ai
            allAI[currentAI].Run();
        }
    }
    /// <summary>
    /// end turn through event and update poison on all zombies.
    /// </summary>
    private void EndCurrentTurn()
    {
        GameEvents.instance.EndAITurn();
        foreach (ZombieStateMachine currentZombie in allAI)
        {
            Enemy toUpdate;
            if (currentZombie.TryGetComponent<Enemy>(out toUpdate))
            {
                
                toUpdate.UpdatePoison();
            }
        }
    }
    /// <summary>
    /// Registers new enemy in AiManager
    /// </summary>
    /// <param name="newUnit"></param>
    private void RegisterNewUnit(Unit newUnit)
    {
        if(newUnit.MyUnitType==HeroEnums.UnitType.enemy)
            allAI.Add(newUnit.GetComponent<ZombieStateMachine>());
    }
    /// <summary>
    /// Unregisters each unit from AIManager, responsible for ending the Round 
    /// </summary>
    /// <param name="unitToRemove"></param>
    private void RemoveUnit(Unit unitToRemove)
    {
        ZombieStateMachine stateMachineToRemove;
        if(unitToRemove.gameObject.TryGetComponent<ZombieStateMachine>(out stateMachineToRemove))
        {
            if (allAI.Contains(stateMachineToRemove))
            {
                allAI.Remove(stateMachineToRemove);
                if (allAI.Count == 0)
                {
                    //if all enemies in final level are deafeated, load endScene
                    if (CampaignManager.instance.FinalLevel)
                    {
                        SceneManager.LoadScene(2);
                        return;
                    }

                        UpgradeManager.instance.StartUpgradeScreen();

                }
            }
        }
    }
}
