using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{

    protected ZombieStateMachine myStateMachine=null;

    public ZombieStateMachine MyStateMachine { set => myStateMachine = value; }

    /// <summary>
    /// run unit activation.
    /// </summary>
    public abstract void Run(Unit myZombie);
    /// <summary>
    /// initialize unit.
    /// </summary>
    public abstract void Init(Unit myZombie);
}
