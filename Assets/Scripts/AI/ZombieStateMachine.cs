using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// keeps track of a zombies AIBehaviour
/// </summary>
public class ZombieStateMachine : MonoBehaviour
{
    [SerializeField]
    private AIBehaviour defaultState;
    private AIBehaviour currentState;
    [SerializeField]
    private Unit myZombie;

    private void Start()
    {
        currentState = defaultState;
    }
    /// <summary>
    /// run one cycle of the state machine
    /// one cycle equals one zombie activation
    /// </summary>
    public void Run()
    {
        currentState.MyStateMachine = this;
        currentState.Run(myZombie);
    }
    public void ChangeState(AIBehaviour newBehaviour)
    {
        currentState = newBehaviour;
        currentState.Init(myZombie);
    }
    public void ChangeState()
    {
        currentState = defaultState;
        currentState.Init(myZombie);
    }

}
