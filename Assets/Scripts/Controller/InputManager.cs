using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    KeyCode up = KeyCode.W;
    KeyCode down = KeyCode.S;
    KeyCode left = KeyCode.A;
    KeyCode right = KeyCode.D;
    KeyCode accept = KeyCode.J;
    KeyCode refuse = KeyCode.K;
    


    [SerializeField]
    InputProcessor currentInputProcessor;
    [SerializeField]
    DirectionProcessor currentUpDownLeftRightProcessor;
    private float rapidFireStartingDelay=0.3f,rapidFireCooldown=0.1f,rapidFireTimer=0.0f;
    private float upTimer=0.0f, downTimer = 0.0f, rightTimer = 0.0f, leftTimer = 0.0f;

    private bool playerControl = true;

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

    public InputProcessor CurrentInputProcessor { set => currentInputProcessor = value; }
    public DirectionProcessor CurrentDirectionInputProcessor { get => currentUpDownLeftRightProcessor; set => currentUpDownLeftRightProcessor = value; }

    public void DeactivatePlayerControl()
    {
        playerControl = false;
    }
    public void ActivatePlayerControl()
    {
        playerControl = true;
    }

    private void Update()
    {
        if (playerControl)
        {
            CheckForKeyDown();
            CheckForKeyStay();
        }

        //Debugging keys:
        CheckForDebuggingKeys();
    }
    /// <summary>
    /// check for key presses and call functions based on current input-processors
    /// </summary>
    private void CheckForKeyDown()
    {
        if (Input.GetKeyDown(up))
        {
            currentUpDownLeftRightProcessor.MoveHighlightUp();
        }
        if (Input.GetKeyDown(down))
        {
            currentUpDownLeftRightProcessor.MoveHighlightDown();
        }
        if (Input.GetKeyDown(left))
        {
            currentUpDownLeftRightProcessor.MoveHighlightLeft();
        }
        if (Input.GetKeyDown(right))
        {
            currentUpDownLeftRightProcessor.MoveHighlightRight();
        }
        if (Input.GetKeyDown(accept))
        {
            currentInputProcessor.Accept();
        }
        if (Input.GetKeyDown(refuse))
        {
            currentInputProcessor.Refuse();
        }
    }
    /// <summary>
    /// check for held keys and call functions based on current input-processors
    /// </summary>
    private void CheckForKeyStay()
    {
        if (Input.GetKey(up))
        {
            upTimer += Time.deltaTime;
            if (upTimer >= rapidFireStartingDelay)
            {
                rapidFireTimer += Time.deltaTime;
                if (rapidFireTimer >= rapidFireCooldown)
                {
                    currentUpDownLeftRightProcessor.MoveHighlightUp();
                    rapidFireTimer = 0.0f;
                }
            }
        }
        else
        {
            upTimer = 0.0f;
        }
        if (Input.GetKey(down))
        {
            downTimer += Time.deltaTime;
            if (downTimer >= rapidFireStartingDelay)
            {
                rapidFireTimer += Time.deltaTime;
                if (rapidFireTimer >= rapidFireCooldown)
                {
                    currentUpDownLeftRightProcessor.MoveHighlightDown();
                    rapidFireTimer = 0.0f;
                }
            }
        }
        else
        {
            downTimer = 0.0f;
        }
        if (Input.GetKey(left))
        {
            leftTimer += Time.deltaTime;
            if (leftTimer >= rapidFireStartingDelay)
            {
                rapidFireTimer += Time.deltaTime;
                if (rapidFireTimer >= rapidFireCooldown)
                {
                    currentUpDownLeftRightProcessor.MoveHighlightLeft();
                    rapidFireTimer = 0.0f;
                }
            }
        }
        else
        {
            leftTimer = 0.0f;
        }
        if (Input.GetKey(right))
        {
            rightTimer += Time.deltaTime;
            if (rightTimer >= rapidFireStartingDelay)
            {
                rapidFireTimer += Time.deltaTime;
                if (rapidFireTimer >= rapidFireCooldown)
                {
                    currentUpDownLeftRightProcessor.MoveHighlightRight();
                    rapidFireTimer = 0.0f;
                }
            }
        }
        else
        {
            rightTimer = 0.0f;
        }
    }
    private void CheckForDebuggingKeys()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpgradeManager.instance.StartUpgradeScreen();
        }
    }
    public void ReturnToHeroSelection()
    {
        currentInputProcessor = HeroSelectionInputProcessor.instance;
        currentUpDownLeftRightProcessor = SpaceSelectorDirectionProcessor.instance;
    }
}
