using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public BaseTile[,] gridArray; // 2D array to hold the references to all the tiles
    public int gridSizeX = 8; // Number of tiles in the X direction
    public int gridSizeY = 8; // Number of tiles in the Y direction

    // ref to game objects
    public ScoutAnt scoutAnt;
    public SoldierAnt soldierAnt;
    public QueenAnt queenAnt;
    public BuilderAnt builderAnt;
    public AntLion antLion;
    public AntLion antLionSecond;

    public GameManager gameManager;
    public UIManager uiManager;

    public int selectedAnt; //No Selection = -1 Scout = 0, Builder = 1, Soldier = 2

    //Soldier Ant Throw Actions
    private int soldierThrowPhase; //not throwing = -1, selecting target = 0, selecting tile to throw to = 1
    private int soldierThrowTarget; //no target = -1, scout = 0, builder = 1, antlion(1) = 2, antlion(2) = 3

    //QueenAnt path
    private List<Vector2Int> queenPath;
    private int currentPathIndex = 0; 

    //Ant's positions on grid
    private List<Vector2Int> otherAntPositions;

    void Start()
    {
        selectedAnt = -1;
        gridArray = new BaseTile[gridSizeX, gridSizeY];

        //set up soldier ant throw phase
        soldierThrowPhase = -1;

        // Loop through all child objects under the grid parent
        foreach (Transform child in transform)
        {
            // Check if the child has a BaseTile component
            BaseTile baseTile = child.GetComponent<BaseTile>();

            // If BaseTile, parse the name and store it in the array
            if (baseTile != null)
            {
                // indices from the tile's name
                Vector2Int gridPos = ParseTileName(child.name);

                // position is within bounds
                if (gridPos.x >= 0 && gridPos.x < gridSizeX && gridPos.y >= 0 && gridPos.y < gridSizeY)
                {
                    // Store the tile in the gridArray 
                    gridArray[gridPos.x, gridPos.y] = baseTile;

                    // assign the tile's grid index (X, Y) to the BaseTile
                    baseTile.xIndex = gridPos.x;
                    baseTile.yIndex = gridPos.y;

                    // Log the tile
                    //Debug.Log($"Tile {child.name} placed at Grid Position: ({gridPos.x}, {gridPos.y})");
                }
                else
                {
                    //Debug.LogWarning($"Tile {child.name} is out of bounds ({gridPos.x}, {gridPos.y})");
                }
            }
        }

        InitializeQueenPath();

        // log the grid after population
        Debug.Log("Grid Population Complete!");
    }

    void Update()
    {
        // if player's turn, detect mosue clicks
        if(gameManager.gameState == GameManager.GameState.Player || gameManager.gameState == GameManager.GameState.Deploy){


            if (Input.GetMouseButtonDown(0))
            {
                // Create a ray from the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Perform the raycast to check for hits
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                // If the raycast hits something
                if (hit.collider != null)
                {
                    // Get the BaseTile component from the hit object
                    BaseTile baseTile = hit.collider.GetComponent<BaseTile>();

                    // If a BaseTile was hit
                    if (baseTile != null)
                    {
                        // Print the tile's index from the grid
                        Debug.Log($"Tile clicked at ({baseTile.xIndex}, {baseTile.yIndex}) with type: {baseTile.tileType}");
                       
                        //Check clicked tile for ants, show UI, highlight tiles
                        // Check if the clicked tile contains the ScoutAnt
                         if (selectedAnt == -1 && scoutAnt != null && scoutAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                        {
                            selectedAnt = 0;
                            Debug.Log("Scout Ant Selected!");
                            uiManager.ShowScoutAntUI();
                            uiManager.HideBuilderAntUI();
                            uiManager.HideSoldierAntUI();
                            UnhighlightAllTiles();
                        }

                        else if (selectedAnt == -1 &&  builderAnt != null && builderAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                        {
                            selectedAnt = 1;
                            Debug.Log("Builder Ant Selected!");
                            uiManager.ShowBuilderAntUI();
                            uiManager.HideScoutAntUI();
                            uiManager.HideSoldierAntUI();
                            UnhighlightAllTiles();
                        }

                        else if (selectedAnt == -1 && soldierAnt != null && soldierAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                        {
                            selectedAnt = 2;
                            Debug.Log("Soldier Ant Selected!");
                            uiManager.ShowSoldierAntUI();
                            uiManager.HideScoutAntUI();
                            uiManager.HideBuilderAntUI();
                            UnhighlightAllTiles();
                        }

              

                        //Click on Highlight Tile
                        if (selectedAnt == 0 && baseTile.isHighlighted){
                            MovePlayerAnt(scoutAnt, baseTile);
                            UnhighlightAllTiles();
                        }

                        if(selectedAnt == 1 && baseTile.isHighlighted){
                            MovePlayerAnt(builderAnt, baseTile);
                            UnhighlightAllTiles();
                        }

                        if (selectedAnt == 2 && baseTile.isHighlighted)
                        {
                            MovePlayerAnt(soldierAnt, baseTile);
                            UnhighlightAllTiles();
                        }

                        //Click on Ability Tile
                        if (selectedAnt == 0 && baseTile.isAbilityTile){
                            RevealTrapTiles(scoutAnt.gridPosition);
                            Debug.Log("Revealed Trap Tiles!");
                            UnhighlightAllTiles();
                        }

                        //Ant Throw 
                        if (selectedAnt == 2 && baseTile.isAbilityTile && soldierThrowPhase == -1)
                        {
                            Debug.Log("Start Throw Process");
                            ThrowSelectTargetCharacter(soldierAnt, baseTile);
                            
                        } else if (selectedAnt == 2 && baseTile.isAbilityTile && soldierThrowPhase == 0)
                        {
                            Debug.Log("throwing character now");
                            ThrowCharacter(baseTile);

                        }

                    }
                }
            }
            if (Input.GetMouseButtonDown(1) || selectedAnt == -1) //Right click to deselect everything?
            {
                selectedAnt = -1; //deselect ant
                //hide all ui
                uiManager.HideScoutAntUI();
                uiManager.HideBuilderAntUI();
                uiManager.HideSoldierAntUI();
                //unhighlight all ants
                UnhighlightAllTiles();

                //Debug.Log("Deselect everything!");
            }


        }
    }

    // tile's name to get the grid position
    Vector2Int ParseTileName(string tileName)
    {
        // name format is "Tile_x_y"
        string[] parts = tileName.Split('_');

        if (parts.Length == 3 && parts[0] == "Tile")
        {
            int x = int.Parse(parts[1]); 
            int y = int.Parse(parts[2]);

            return new Vector2Int(x, y);
        }

        //if the name format is invalid
        Debug.LogError($"Invalid tile name format: {tileName}");
        return new Vector2Int(-1, -1);  // Invalid index
    }

    // Initialize the Queen's predetermined path
    void InitializeQueenPath()
    {
        // Define the path for QueenAnt
        queenPath = new List<Vector2Int>()
        {
            new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(0, 3),
            new Vector2Int(1, 3), new Vector2Int(2, 3), new Vector2Int(3, 3), new Vector2Int(4, 3),
            new Vector2Int(5, 3), new Vector2Int(6, 3),  new Vector2Int(7, 3)
        };
    }

    // Highlight tiles around the Player Ant, for move function
    void HighlightSurroundingTiles(Vector2Int antPosition)
    {
        // Update list of all other ant positions, for preventing moving onto or using ability there
        List<Vector2Int> otherAntPositions = new List<Vector2Int>
        {
            scoutAnt.gridPosition,
            queenAnt.gridPosition,
            builderAnt.gridPosition,
            soldierAnt.gridPosition,
            antLion.gridPosition,
            antLionSecond.gridPosition
        };

        // Loop through the grid to check all tiles within the radius
        for (int x = antPosition.x - 1; x <= antPosition.x + 1; x++)
        {
            for (int y = antPosition.y - 1; y <= antPosition.y + 1; y++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);

                // Check if the tile is within bounds and not occupied by another ant
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && !otherAntPositions.Contains(currentPos))
                {
                    BaseTile tile = gridArray[x, y];

                    if (tile != null && tile.isWalkable)
                    {
                        tile.Highlight();
                    }
                }
            }
        }
    }

    void UnhighlightAllTiles()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                BaseTile tile = gridArray[x, y];
                if (tile != null)
                {
                    tile.Unhighlight();
                    tile.UnhighlightAbilityTile();
                }
            }
        }
    }

     //----Ability Functions----------------------------------------------------------------------

    //Scout Ability Detect
    public void OnDetectButtonPressed()
    {
        if (selectedAnt == 0) // Scout Ant
        {
            Debug.Log("Detect button pressed for Scout Ant!");
            if (!scoutAnt.usedAbility)
            {
                UnhighlightAllTiles();
                ShowDetectTiles(scoutAnt.gridPosition);
            }
        }
    }

    void ShowDetectTiles(Vector2Int antPosition)
    {
        // Loop through the grid to check all tiles within the radius
        for (int x = antPosition.x - 1; x <= antPosition.x + 1; x++)
        {
            for (int y = antPosition.y - 1; y <= antPosition.y + 1; y++)
            {
                // Check if the tile is within bounds
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    BaseTile tile = gridArray[x, y];

                    if (tile != null) //doesnt have to be walkable
                    {
                        tile.HighlightAbilityTile();
                    }
                }
            }
        }

        // Extend one more tile in the four cardinal directions
        int[,] extraTiles = {
            {antPosition.x, antPosition.y + 2}, // Up
            {antPosition.x, antPosition.y - 2}, // Down
            {antPosition.x + 2, antPosition.y}, // Right
            {antPosition.x - 2, antPosition.y}  // Left
        };

        for (int i = 0; i < extraTiles.GetLength(0); i++)
        {
            int x = extraTiles[i, 0];
            int y = extraTiles[i, 1];

            if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
            {
                BaseTile tile = gridArray[x, y];
                if (tile != null)
                {
                    tile.HighlightAbilityTile();
                }
            }
        }
    }

    void RevealTrapTiles(Vector2Int antPosition)
    {
        // Loop through the grid to check all tiles within the radius
        for (int x = antPosition.x - 1; x <= antPosition.x + 1; x++)
        {
            for (int y = antPosition.y - 1; y <= antPosition.y + 1; y++)
            {
                // Check if the tile is within bounds
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    BaseTile tile = gridArray[x, y];

                    if (tile != null && tile is TrapTile) // Only reveal trap tiles
                    {
                        ((TrapTile)tile).RevealTrap();  // Call RevealTrap on trap tiles
                    }
                }
            }
        }

        // Extend one more tile
        int[,] extraTiles = {
            {antPosition.x, antPosition.y + 2}, // Up
            {antPosition.x, antPosition.y - 2}, // Down
            {antPosition.x + 2, antPosition.y}, // Right
            {antPosition.x - 2, antPosition.y}  // Left
        };

        for (int i = 0; i < extraTiles.GetLength(0); i++)
        {
            int x = extraTiles[i, 0];
            int y = extraTiles[i, 1];

            if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
            {
                BaseTile tile = gridArray[x, y];
                if (tile != null && tile is TrapTile)  // Only reveal trap tiles
                {
                    ((TrapTile)tile).RevealTrap();  // Call RevealTrap on trap tiles
                }
            }
        }

        scoutAnt.usedAbility = true;
    }

    void ShowBuildTiles(Vector2Int antPosition){
        
    }

    //----Soldier Ability Throw----

    //first, on throw button press:
    public void OnThrowButtonPressed()
    {
        if (selectedAnt == 2) // soldier ant
        {
            Debug.Log("Throw button pressed for Soldier Ant!");
            if (!soldierAnt.usedAbility)
            {
                UnhighlightAllTiles();
                ShowThrowOptionTiles();
            }
        }
    }

    //Second, highlight the entire area around soldier ant (all character in hightlighted areas are options for soldier ant to throw
    void ShowThrowOptionTiles()
    {
        // Loop through the grid to check all tiles within the radius (adjacent and diagonal)
        for (int x = soldierAnt.gridPosition.x - 1; x <= soldierAnt.gridPosition.x + 1; x++)
        {
            for (int y = soldierAnt.gridPosition.y - 1; y <= soldierAnt.gridPosition.y + 1; y++)
            {
                // Check if the tile is within bounds
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    BaseTile tile = gridArray[x, y];

                    if (tile != null) // Doesn't have to be walkable
                    {
                        tile.HighlightAbilityTile(); // Highlight the surrounding tiles
                    }
                }
            }
        }
    }

    //Third, Click on a tile with a character on it to proceed to the actual throw action
    //for now, if you click on a tile with nothing on it, it just cancels the highlight.
    void ThrowSelectTargetCharacter(SoldierAnt soldierAnt, BaseTile targetTile)
    {
        UnhighlightAllTiles();
        Vector2Int targetPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);

        

        switch (targetPosition)
        {
            case var _ when targetPosition == scoutAnt.gridPosition:
                soldierThrowTarget = 0;
                soldierThrowPhase = 0;
                showTileToThrowTo();
                break;

            case var _ when targetPosition == builderAnt.gridPosition:
                soldierThrowTarget = 1;
                soldierThrowPhase = 0;
                showTileToThrowTo();
                break;

            case var _ when targetPosition == antLion.gridPosition:
                
                soldierThrowTarget = 2;
                soldierThrowPhase = 0;
                showTileToThrowTo();
                break;

            case var _ when targetPosition == antLionSecond.gridPosition:
                soldierThrowTarget = 3;
                soldierThrowPhase = 0;
                showTileToThrowTo();
                break;

            default:
                // Handle the case where targetPosition does not match any known ant's position
                Debug.Log("no viable target selected");
                break;
        }

    }

    //Fourth, with the target selected, choose the tile you want to throw that target to.
    void showTileToThrowTo()
    {
        // Define the four cardinal directions with one-step and two-step distances
        int[,] targetTiles = {
            {soldierAnt.gridPosition.x, soldierAnt.gridPosition.y + 1}, // Up 1 step
            {soldierAnt.gridPosition.x, soldierAnt.gridPosition.y + 2}, // Up 2 steps
            {soldierAnt.gridPosition.x, soldierAnt.gridPosition.y - 1}, // Down 1 step
            {soldierAnt.gridPosition.x, soldierAnt.gridPosition.y - 2}, // Down 2 steps
            {soldierAnt.gridPosition.x + 1, soldierAnt.gridPosition.y}, // Right 1 step
            {soldierAnt.gridPosition.x + 2, soldierAnt.gridPosition.y}, // Right 2 steps
            {soldierAnt.gridPosition.x - 1, soldierAnt.gridPosition.y}, // Left 1 step
            {soldierAnt.gridPosition.x - 2, soldierAnt.gridPosition.y}  // Left 2 steps
        };

        // Loop through the defined target positions
        for (int i = 0; i < targetTiles.GetLength(0); i++)
        {
            int x = targetTiles[i, 0];
            int y = targetTiles[i, 1];

            // Check if the tile is within bounds
            if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
            {
                BaseTile tile = gridArray[x, y];
                if (tile != null)
                {
                    tile.HighlightAbilityTile(); // Highlight valid tiles
                }
            }
        }
    }

    //Fifth, actually throw the target
    void ThrowCharacter(BaseTile targetTile)
    {
        switch (soldierThrowTarget)
        {
            case 0:
                scoutAnt.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
                scoutAnt.transform.position = targetTile.transform.position;
                break;

            case 1:
                builderAnt.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
                builderAnt.transform.position = targetTile.transform.position;
                break;

            case 2:
                antLion.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
                antLion.transform.position = targetTile.transform.position;
                break;

            case 3:
                antLionSecond.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
                antLionSecond.transform.position = targetTile.transform.position;
                break;
        }

        //reset variables
        soldierThrowPhase = -1;
        soldierThrowTarget = -1;
        UnhighlightAllTiles();

    }


    //----Move Functions----------------------------------------------------------------------

    //Move Button pressed Highlight Tiles, applied to OnCLick()
    public void OnMoveButtonPressed()
    {
        if (selectedAnt == 0) // Scout Ant
        {
            Debug.Log("Move button pressed for Scout Ant!");
            UnhighlightAllTiles();
            if (!scoutAnt.hasMoved)
            {
                HighlightSurroundingTiles(scoutAnt.gridPosition);
            }
        }
        else if (selectedAnt == 1) // Builder Ant
        {
            Debug.Log("Move button pressed for Builder Ant!");
            UnhighlightAllTiles();
            if (!builderAnt.hasMoved)
            {
                HighlightSurroundingTiles(builderAnt.gridPosition);
            }
        }
        else if (selectedAnt == 2) // Soldier Ant
        {
            Debug.Log("Move button pressed for Solider Ant!");
            UnhighlightAllTiles();
            if (!soldierAnt.hasMoved)
            {
                HighlightSurroundingTiles(soldierAnt.gridPosition);
            }
        }
    }

    //Move specified player ant to target tile
    void MovePlayerAnt(PlayerAnt playerAnt, BaseTile targetTile)
    {
        UnhighlightAllTiles();
        // Move the ant to the target tile
        playerAnt.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
        playerAnt.transform.position = targetTile.transform.position;
        
        playerAnt.hasMoved = true;
    
        
    }

    public void moveQueen(){
        if (queenPath != null && currentPathIndex < queenPath.Count)
        {
            // Get the target position from the path
            Vector2Int targetPosition = queenPath[currentPathIndex];

            // Find the BaseTile at the target position
            BaseTile targetTile = gridArray[targetPosition.x, targetPosition.y];

            if (targetTile != null && targetTile.isWalkable)
            {
                // Check if an ant is already occupying the target position
                if ((scoutAnt != null && scoutAnt.gridPosition == targetPosition) ||
                    (builderAnt != null && builderAnt.gridPosition == targetPosition) ||
                    (soldierAnt != null && soldierAnt.gridPosition == targetPosition))
                {
                    Debug.Log($"Queen cannot move to ({targetPosition.x}, {targetPosition.y}) - an ant is already there!");
                    return; // Stop movement if another ant is present
                }
                // Move Queen to the new position
                queenAnt.transform.position = targetTile.transform.position;

                // Update Queen's grid position
                queenAnt.gridPosition = targetPosition;

                // Log the movement
                Debug.Log($"Queen moved to ({targetPosition.x}, {targetPosition.y})");

                // Check if the Queen has reached the winning position
                if (targetPosition.x == 7 && targetPosition.y == 3)
                {
                    // Trigger the game win logic
                    gameManager.WinGame();
                }
                // Increment the path index to move to the next tile in the next call
                currentPathIndex++;
            }
        }
        else
        {
            // Log if the Queen has completed the path
            //Debug.Log("Queen has completed her path!");
        }

    }

    public void moveAntLion()
    {
        // Get the Queen's current position
        Vector2Int queenPosition = queenAnt.gridPosition;

        // Get the current AntLion position
        Vector2Int antLionPosition = antLion.gridPosition;

        // Calculate the direction toward the Queen's position
        Vector2Int direction = new Vector2Int(
            (int)Mathf.Sign(queenPosition.x - antLionPosition.x),
            (int)Mathf.Sign(queenPosition.y - antLionPosition.y)
        );

        // Calculate the next position
        Vector2Int nextPosition = new Vector2Int(antLionPosition.x + direction.x, antLionPosition.y + direction.y);

        // find basetile at target pos
        BaseTile targetTile = gridArray[nextPosition.x, nextPosition.y];

        if (targetTile != null && targetTile.isWalkable) {
            // check if any ants are on target position
            if ((scoutAnt != null && scoutAnt.gridPosition == nextPosition) ||
                (builderAnt != null && builderAnt.gridPosition == nextPosition) ||
                (soldierAnt != null && soldierAnt.gridPosition == nextPosition)) {
                Debug.Log($"AntLion cannot move to ({nextPosition.x}, {nextPosition.y}) - an ant is already there!");
                return;
            }

            // Move AntLion to the new position
            antLion.transform.position = targetTile.transform.position;

            // Update AntLion's grid position
            antLion.gridPosition = nextPosition;

            Debug.Log($"AntLion moved to ({nextPosition.x}, {nextPosition.y})");

            //add logic for reaching queen position, attack

        } else {
            Debug.Log($"AntLion cannot move to ({nextPosition.x}, {nextPosition.y}) - it's not walkable.");
        }
    }

}
