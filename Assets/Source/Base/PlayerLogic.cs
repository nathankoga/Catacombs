using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    /*
     * This class is in charge of the player logic.
     * It listens to the manager update to decide if it is in a state that can move,
     * and if it is in a movable state, it will respond to any WASD/arrow key movements.
     * 
     * It will need to interact with the DungeonManager to figure out where valid tiles are.
     * When the player moves onto a tile with an enemy, it will need to tell the
     * BattleManager about that and what that enemy is, and the BattleManager will engage combat.
     */

    public delegate void UserInputAction();
    public static event UserInputAction MoveUpdate;
    
    public Vector2 tilePosition = Vector2.zero;
    public Vector3 worldOffset;

    public InputAction moveAction;
    public AudioSource moveSound;

    // storing player data into this class as well
    private int lives = 9;
    private int balance = 0;
    private int ferocity = 1;
    private int stubbornness = 1;
    private int precision = 1;
    private int grace = 1;
    private int gems = 0;


    // new abilities may be difficult to implement in a timely manner 
    // private Amulet[] Amulets = Array.Empty<Amulet>();
    // private Ability[] Abilities = Array.Empty<Ability>();

    private void Start()
    {
        LerpToWorldPos(1.0f);
        moveSound = GetComponent<AudioSource>();
    }

    /*
     * Update stuffs
     */

    private void Update()
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON || true)
        {
            // Listen for inputs.
            if (Input.GetKeyDown(KeyCode.W)) OnMove(new Vector2(0.0f, 1.0f));
            if (Input.GetKeyDown(KeyCode.S)) OnMove(new Vector2(0.0f, -1.0f));
            if (Input.GetKeyDown(KeyCode.A)) OnMove(new Vector2(-1.0f, 0.0f));
            if (Input.GetKeyDown(KeyCode.D)) OnMove(new Vector2(1.0f, 0.0f));
        }
    }

    private void FixedUpdate()
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON || true)
        {
            // Move to world position.
            LerpToWorldPos(0.25f);
        }
    }

    /*
     * Positioning
     */

    private void LerpToWorldPos(float t)
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetWorldPos(), t);
    }

    public Vector3 GetTargetWorldPos()
    {
        return DungeonManager.GetWorldTilePosition(tilePosition) + worldOffset;
    }

    public void SetPosition(Vector2 pos)
    {
        this.tilePosition = pos;
        LerpToWorldPos(1.0f);
    }

    /*
     * Input Actions
     */

    private void OnMove(Vector2 movementVector)
    {
        if (GameState.GetManagerType() == ManagerType.DUNGEON)
        {
            
            // MoveUpdate is observed by DungeonGUI, which leads to RemoveTutorialText to be called
            MoveUpdate();
            // print(movementVector);

            // What is our new tile position?
            Vector2 newPos = tilePosition + movementVector;
            MapTile newTile = DungeonManager.GetTileAtPos(newPos);
            if (newTile != null)
            {
                if (newTile.CanMoveOnto()) {
                    // before colliding into an enemy, initiate combat
                    // if (newTile.hasAnEnemy()){
                    //     newTile.OnStep(this);
                    // }
                    // else{
                    moveSound.Play();
                    tilePosition = newPos;
                    newTile.OnStep(this);
                    // }
                }

            }
        }
    }

}
