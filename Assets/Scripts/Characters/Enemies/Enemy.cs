using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField]
    HealthBar myHealthBar;
    [SerializeField]
    TextMeshProUGUI poisonCounterText;

    /// <summary>
    /// take damage and set new healthbar-value
    /// </summary>
    /// <param name="amount"></param>
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        myHealthBar.SetHealth(Health);
    }
    /// <summary>
    /// take poison damage and update poison counter
    /// </summary>
    /// <param name="amount"></param>
    public override void TakePoisonDamage(float amount)
    {
        poisonCounter += amount;
        poisonCounterText.enabled = true;
        poisonCounterText.text = poisonCounter.ToString();
    }
    protected override void Start()
    {
        base.Start();
        myHealthBar.SetMaxHealth(MaxHealth);
    }
    /// <summary>
    /// take damage amount based on poison-counter, reduce poison-counter by one
    /// </summary>
    public override void UpdatePoison()
    {
        if (poisonCounter > 0)
        {
            TakeDamage(poisonCounter);
            poisonCounter--;
            if (poisonCounterText != null)
            {
                if (poisonCounter == 0) 
                {
                    poisonCounterText.enabled = false;
                }
                else
                {
                    poisonCounterText.enabled = true;
                    poisonCounterText.text = poisonCounter.ToString();
                }
            }
        }
    }
    public override string getStats()
    {
        return $"HP: { health}/{ maxHealth}\nMovementRange: { moveRange}";
    }
}
