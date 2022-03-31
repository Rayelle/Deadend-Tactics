using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroEnums;
/// <summary>
/// Describes a hero or enemy-unit
/// </summary>
public class Unit : MonoBehaviour
{
    [SerializeField]
    Animator myAnimator;
    [SerializeField]
    float attackDuration, attackSoundEffectDelay;
    [SerializeField]
    private UnitType myUnitType;
    [SerializeField]
    protected float armor, damage;
    protected float health;
    [SerializeField]
    protected int moveRange, attackRange;
    protected Vector2Int myCurrentPosition;
    [SerializeField]
    protected bool isHero, isObstacle;
    [SerializeField]
    protected SpriteRenderer mySpriteRenderer;
    [SerializeField]
    TileInformation myTileInformation;
    [SerializeField]
    private AudioSource attackAudio, stepAudio;

    int unitLayerStartingPoint = 0;

    [SerializeField]
    protected float maxHealth;
    private bool stunned = false;
    protected float  poisonCounter = 0.0f;
    private bool ready = true;

    public Vector2Int gridPosition { get => myCurrentPosition; set => myCurrentPosition = value; }
    public virtual int MoveRange { get => moveRange; }
    public bool IsHero { get => isHero; }
    public bool IsObstacle { get => isObstacle; }
    public int AttackRange { get => attackRange; }
    public float Health { get => health; }
    public float MaxHealth { get => maxHealth; }
    public UnitType MyUnitType { get => myUnitType; }
    public bool Stunned { get => stunned; set => stunned = value; }
    public bool Ready { get => ready; set => ready = value; }
    public TileInformation MyTileInformation { get => myTileInformation; }
    public Animator MyAnimator { get => myAnimator; }
    public float AttackDuration { get => attackDuration; }
    public AudioSource AttackAudio { get => attackAudio; }
    public float AttackSoundEffectDelay { get => attackSoundEffectDelay; }

    /// <summary>
    /// take given amount of damage
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakeDamage(float amount)
    {
        if (health - (amount - armor) <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            AnimationHelper.instance.FlashImageRed(mySpriteRenderer);
            health -=  (amount - armor);
        }
    }
    /// <summary>
    /// increase poison counter by given amount
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakePoisonDamage(float amount)
    {
        poisonCounter += amount;
    }
    /// <summary>
    /// get healed by given amount
    /// </summary>
    /// <param name="amount"></param>
    public virtual void GetHealed(float amount)
    {
        //heal cannot exceed maximum health
        if (health + amount >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            if (health > 0.0f)
            {
                health += amount;
            }
        }
    }
    /// <summary>
    /// return damage value
    /// </summary>
    /// <returns></returns>
    public virtual float GetAttackDamage()
    {
        return damage;
    }
    public virtual int GetAttackRange()
    {
        return attackRange;
    }
    //public virtual int GetMoveRange()
    //{
    //    return moveRange;
    //}
    public virtual string getStats()
    {
        return "";
    }
    /// <summary>
    /// Destroy Unit and gameObject
    /// </summary>
    public virtual void Die()
    {
        AnimationHelper.instance.CreateSmokeCloud(IsoGrid.instance.ToWorldSpace(gridPosition));
        Destroy(this.gameObject);
    }
    /// <summary>
    /// set units current position and register it trough event
    /// </summary>
    /// <param name="unitPositionInGrid"></param>
    public void RegisterThisUnit(Vector2Int unitPositionInGrid)
    {
        myCurrentPosition = unitPositionInGrid;
        GameEvents.instance.SpawnUnit(this);
    }

    private void OnDestroy()
    {
        //unregister this unit before being destroyed
        GameEvents.instance.DestroyUnit(this);
    }

    protected virtual void Start()
    {
        health = maxHealth;
    }
    /// <summary>
    /// take poison damage and reduce poisoncounter by one
    /// </summary>
    public virtual void UpdatePoison()
    {
        if (poisonCounter > 0)
        {
            TakeDamage(poisonCounter);
            poisonCounter--;
        }
        
    }
    public virtual void ResetUnit()
    {
        health = maxHealth;
    }
    /// <summary>
    /// moves a unit from one space to another. 
    /// changes transform.position and position in mapContent
    /// </summary>
    /// <param name="destination"></param>
    public virtual void MoveUnitTo(Vector2Int destination)
    {
        if (!(myCurrentPosition == destination))
        {
            MapContent.instance.MoveFromTo(myCurrentPosition, destination);
            transform.position = IsoGrid.instance.ToWorldSpace(destination);
            myCurrentPosition = destination;
            if(stepAudio!=null)
                stepAudio.Play();
        }
        else
        {
            transform.position = IsoGrid.instance.ToWorldSpace(destination);
        }
        if (mySpriteRenderer != null)
            mySpriteRenderer.sortingOrder = unitLayerStartingPoint - destination.y + destination.x;
    }
}
