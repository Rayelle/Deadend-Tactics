using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class controls events which enables classes to communicate
public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public event Action onEndPlayerTurn, onEndAITurn, onUnitDoneMoving,onPlayerWin,onPlayerLoss,onEndMap;
    public event Action<Unit> onSpawnUnit,onDestroyUnit, onEndActivation;

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
    public void PlayerLoss()
    {
        if (onPlayerLoss != null)
        {
            onPlayerLoss();
        }
    }
    public void EndPlayerTurn()
    {
        if (onEndPlayerTurn != null)
        {
            onEndPlayerTurn();
        }
    }
    public void EndAITurn()
    {
        if (onEndAITurn != null)
        {
            onEndAITurn();
        }
    }
    public void SpawnUnit(Unit newUnit )
    {
        if (onSpawnUnit != null)
        {
            onSpawnUnit(newUnit);
        }
    }
    public void DestroyUnit(Unit destroyedUnit)
    {
        if (onDestroyUnit != null)
        {
            onDestroyUnit(destroyedUnit);
        }
    }
    public void UnitDoneMoving()
    {
        if (onUnitDoneMoving != null)
        {
            onUnitDoneMoving();
        }
    }
    public void EndActivation(Unit activatedUnit)
    {
        if (onEndActivation != null)
        {
            onEndActivation(activatedUnit);
        }
    }
    public void EndMap()
    {
        if(onEndMap!= null)
        {
            onEndMap();
        }
    }
}
