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

    //QueenAnt path
    private List<Vector2Int> queenPath;
    private int currentPathIndex = 0; 

    //Ant's positions on grid
    private List<Vector2Int> otherAntPositions;

    //GroundTile for replacing
    public GroundTile groundTilePrefab;

    void Start()
    {
        selectedAnt = -1;
        gridArray = new BaseTile[gridSizeX, gridSizeY];

        //set up soldier ant throw phase
        soldierAnt.soldierThrowPhase = -1;

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
        // if player's turn, detect mouse clicks
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
                        //make sure only 1 ant get selected per round
                        if (selectedAnt == -1)
                        { 
                            if (scoutAnt != null && scoutAnt.hpCount >= 1 && scoutAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                            {
                                selectedAnt = 0;
                                Debug.Log("Scout Ant Selected!");
                                uiManager.ShowScoutAntUI();
                                uiManager.HideBuilderAntUI();
                                uiManager.HideSoldierAntUI();
                                uiManager.HideAntLionUI();
                                uiManager.HideQueenAntUI();
                                UnhighlightAllTiles();
                            }
                            else if (builderAnt != null && builderAnt.hpCount >= 1 && builderAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                            {
                                selectedAnt = 1;
                                Debug.Log("Builder Ant Selected!");
                                uiManager.ShowBuilderAntUI();
                                uiManager.HideScoutAntUI();
                                uiManager.HideSoldierAntUI();
                                uiManager.HideAntLionUI();
                                uiManager.HideQueenAntUI();
                                UnhighlightAllTiles();
                            }
                            else if (soldierAnt != null &&  soldierAnt.hpCount >= 1 && soldierAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                            {
                                selectedAnt = 2;
                                Debug.Log("Soldier Ant Selected!");
                                uiManager.ShowSoldierAntUI();
                                uiManager.HideScoutAntUI();
                                uiManager.HideBuilderAntUI();
                                uiManager.HideAntLionUI();
                                uiManager.HideQueenAntUI();
                                UnhighlightAllTiles();
                            }
                            else if (queenAnt != null && queenAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                            {
                                selectedAnt = -1;
                                Debug.Log("Queen Ant clicked!");
                                uiManager.ShowQueenAntUI();
                                uiManager.HideBuilderAntUI();
                                uiManager.HideScoutAntUI();
                                uiManager.HideSoldierAntUI();
                                uiManager.HideAntLionUI();
                                UnhighlightAllTiles();
                            }

                            else if (antLion != null && antLion.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                            {
                                selectedAnt = -1;
                                Debug.Log("Ant Lion clicked!");
                                uiManager.ShowAntLionUI();
                                uiManager.HideBuilderAntUI();
                                uiManager.HideScoutAntUI();
                                uiManager.HideSoldierAntUI();
                                uiManager.HideQueenAntUI();
                                UnhighlightAllTiles();
                            }
                        }
           

                        //Click on Highlight Move Tile ---------------------
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

                        //Click on Ability Tile ----------------------------
                        // Scout Ant Ability Detect
                        if(selectedAnt == 0 && baseTile.isAbilityTile){
                            RevealTrapTiles(scoutAnt.gridPosition);
                            Debug.Log("Revealed Trap Tiles!");
                            UnhighlightAllTiles();
                        }

                        // Builder Ant Ability Build
                        if(selectedAnt == 1 && baseTile.isAbilityTile){
                            BuildOnTile(baseTile);
                            UnhighlightAllTiles();
                        }
                        

                        //Ant Throw 
                        if (selectedAnt == 2 && baseTile.isAbilityTile && soldierAnt.soldierThrowPhase == 0)
                        {
                            Debug.Log("Start Throw Process");
                            ThrowSelectTargetCharacter(soldierAnt, baseTile);
                            
                        } else if (selectedAnt == 2 && baseTile.isAbilityTile && soldierAnt.soldierThrowPhase == 1)
                        {
                            Debug.Log("throwing character now");
                            ThrowCharacter(baseTile);

                        }

                    }
                }
            }

            //Right click to cancel action
            if (Input.GetMouseButtonDown(1)) 
            {
                Debug.Log("right clicked");

                //reset all variables
                selectedAnt = -1;
                soldierAnt.soldierThrowPhase = -1;
                soldierAnt.soldierThrowTarget = -1;

                //hide all ui
                uiManager.HideScoutAntUI();
                uiManager.HideBuilderAntUI();
                uiManager.HideSoldierAntUI();

                //unhighlight all ants
                UnhighlightAllTiles();

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
        List<Vector2Int> otherAntPositions = new List<Vector2Int>();

        if (scoutAnt != null) otherAntPositions.Add(scoutAnt.gridPosition);
        if (queenAnt != null) otherAntPositions.Add(queenAnt.gridPosition);
        if (builderAnt != null) otherAntPositions.Add(builderAnt.gridPosition);
        if (soldierAnt != null) otherAntPositions.Add(soldierAnt.gridPosition);
        if (antLion != null) otherAntPositions.Add(antLion.gridPosition);
        if (antLionSecond != null) otherAntPositions.Add(antLionSecond.gridPosition);



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

    public void UnhighlightAllTiles()
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

    //Builder Ability Detect
    public void OnBuildButtonPressed()
    {
        if (selectedAnt == 1) // Builder Ant
        {
            Debug.Log("Detect button pressed for Scout Ant!");
            if (!builderAnt.usedAbility)
            {
                UnhighlightAllTiles();
                ShowBuildTiles(builderAnt.gridPosition);
            }
        }
    }

    void ShowBuildTiles(Vector2Int antPosition){
        // Update list of all other ant positions, for preventing moving onto or using ability there
        List<Vector2Int> otherAntPositions = new List<Vector2Int>
        {
            scoutAnt.gridPosition,
            queenAnt.gridPosition,
            builderAnt.gridPosition,
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

                    if (tile != null)
                    {
                        tile.HighlightAbilityTile();
                    }
                }
            }
        }
    }

    public void BuildOnTile(BaseTile baseTile)
    {
        // Building ona  water tile (replace with ground tile?)
        if (baseTile.tileType == BaseTile.TileType.Water)
        {
            ((WaterTile)baseTile).BuildBridge();
            builderAnt.usedAbility = true;
            Debug.Log("Built a Bridge!");
        }
        // Building on a ground tile
        if (baseTile.tileType == BaseTile.TileType.Ground)
        {
            ((GroundTile)baseTile).buildObstacle();
            builderAnt.usedAbility = true;
            Debug.Log("Built an Obstacle!");
        }

        // Building on a trap tile (replace with ground tile)
        if (baseTile.tileType == BaseTile.TileType.Trap)
        {

            // grid pos
            Vector2Int gridPos = new Vector2Int(baseTile.xIndex, baseTile.yIndex);

            // Check if valid
            if (gridPos.x >= 0 && gridPos.x < gridSizeX && gridPos.y >= 0 && gridPos.y < gridSizeY)
            {

                // Replace tile
                GroundTile newGroundTile = Instantiate(groundTilePrefab, baseTile.transform.position, Quaternion.identity);

                // Set to the same as the original tile
                newGroundTile.transform.SetParent(baseTile.transform.parent);
                newGroundTile.name = baseTile.name;
                newGroundTile.xIndex = baseTile.xIndex;
                newGroundTile.yIndex = baseTile.yIndex;

                // Update the new tile type and sprite
                newGroundTile.tileType = BaseTile.TileType.Ground;
                newGroundTile.HideDeployableTiles();

                // Update the grid array with new 
                gridArray[gridPos.x, gridPos.y] = newGroundTile;

                // Destroy the old TrapTile
                Destroy(baseTile.gameObject);

                builderAnt.usedAbility = true;
                Debug.Log("Buried Trap and Replaced with Ground Tile!");
            }
        }

    }

    //----Soldier Ability Throw----

    //On throw button press, show all the tiles that the ant soldier can grab characters from.
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
        soldierAnt.soldierThrowPhase = 0;
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
                soldierAnt.soldierThrowTarget = 0;
                
                showTileToThrowTo();
                break;

            case var _ when targetPosition == builderAnt.gridPosition:
                soldierAnt.soldierThrowTarget = 1;
                showTileToThrowTo();
                break;

            case var _ when targetPosition == antLion.gridPosition:

                soldierAnt.soldierThrowTarget = 2;
                showTileToThrowTo();
                break;

            case var _ when targetPosition == antLionSecond.gridPosition:
                soldierAnt.soldierThrowTarget = 3;
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
        soldierAnt.soldierThrowPhase = 1;
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
        switch (soldierAnt.soldierThrowTarget)
        {
            case 0:
                scoutAnt.GettingThrown(targetTile);
                break;

            case 1:
                builderAnt.GettingThrown(targetTile);
                break;

            case 2:
                antLion.GettingThrown(targetTile);
                break;

            case 3:
                antLionSecond.GettingThrown(targetTile);
                break;
        }

        //reset variables
        soldierAnt.soldierThrowPhase = -1;
        soldierAnt.soldierThrowTarget = -1;

        soldierAnt.usedAbility = true;
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
        playerAnt.MoveToTile(targetTile);

        playerAnt.hasMoved = true;

        //target tile is a trap tile, hurt ant
        if (targetTile is TrapTile){
            playerAnt.loseHP();
        }
        
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

                // Update Queen's grid position
                queenAnt.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
                // Move Queen to the new position
                queenAnt.MoveToTile(targetTile);



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
    moveAntLionIndividual(antLion);
    moveAntLionIndividual(antLionSecond);
}

private void moveAntLionIndividual(AntLion antLion)
{
    // Check if an attackable ant is within 1 tile before moving
    NPCAnt queenAntTarget = CheckForAttackableQueenAnt(antLion.gridPosition);
    if (queenAntTarget != null)
    {
        AttackAnt(antLion, queenAntTarget);
        return; // End turn after attacking
    }

    PlayerAnt playerAntTarget = CheckForAttackablePlayerAnt(antLion.gridPosition);
    if (playerAntTarget != null)
    {
        AttackAnt(antLion, playerAntTarget);
        return; // End turn after attacking
    }

    // If no attackable ant is found, proceed with movement
    Vector2Int targetPosition;
    Vector2Int queenPosition = queenAnt.gridPosition;
    Vector2Int antLionPosition = antLion.gridPosition;

    // Move towards QueenAnt first
    int dx = Mathf.Clamp(queenPosition.x - antLionPosition.x, -1, 1);
    int dy = Mathf.Clamp(queenPosition.y - antLionPosition.y, -1, 1);
    Vector2Int nextPositionTowardsQueen = new Vector2Int(antLionPosition.x + dx, antLionPosition.y + dy);

    if (isMoveViable(nextPositionTowardsQueen, queenPosition))
    {
        targetPosition = nextPositionTowardsQueen;
    }
    else
    {
        // Move towards the closest player ant
        Vector2Int closestPlayerAntPosition = findClosestPlayerAntPosition(antLionPosition);
        dx = Mathf.Clamp(closestPlayerAntPosition.x - antLionPosition.x, -1, 1);
        dy = Mathf.Clamp(closestPlayerAntPosition.y - antLionPosition.y, -1, 1);
        Vector2Int nextStepTowardsPlayerAnt = new Vector2Int(antLionPosition.x + dx, antLionPosition.y + dy);

        if (isMoveViable(nextStepTowardsPlayerAnt, queenPosition))
        {
            targetPosition = nextStepTowardsPlayerAnt;
        }
        else
        {
            targetPosition = antLionPosition; // Stay in place if no move is possible
        }
    }

    // Move AntLion to the determined position
    if (targetPosition != antLionPosition)
    {
        BaseTile targetTile = gridArray[targetPosition.x, targetPosition.y];
        if (targetTile != null && targetTile.isWalkable)
        {
            antLion.transform.position = targetTile.transform.position;
            antLion.gridPosition = targetPosition;
            Debug.Log($"AntLion moved to ({targetPosition.x}, {targetPosition.y})");
        }
    }

    // After moving, check for attackable ants again
    queenAntTarget = CheckForAttackableQueenAnt(antLion.gridPosition);
    if (queenAntTarget != null)
    {
        AttackAnt(antLion, queenAntTarget);
        return;
    }

    playerAntTarget = CheckForAttackablePlayerAnt(antLion.gridPosition);
    if (playerAntTarget != null)
    {
        AttackAnt(antLion, playerAntTarget);
    }
}
private NPCAnt CheckForAttackableQueenAnt(Vector2Int antLionPosition)
{
    Vector2Int[] directions = {
        new Vector2Int(0, 1), new Vector2Int(0, -1), 
        new Vector2Int(1, 0), new Vector2Int(-1, 0)
    };

    foreach (Vector2Int dir in directions)
    {
        Vector2Int checkPos = antLionPosition + dir;
        if (queenAnt != null && queenAnt.gridPosition == checkPos)
        {
            return queenAnt; // Found QueenAnt in attack range
        }
    }

    return null;
}
private PlayerAnt CheckForAttackablePlayerAnt(Vector2Int antLionPosition)
{
    List<PlayerAnt> playerAnts = new List<PlayerAnt> { scoutAnt, builderAnt, soldierAnt };
    PlayerAnt closestAnt = null;
    float minDistance = float.MaxValue;

    Vector2Int[] directions = {
        new Vector2Int(0, 1), new Vector2Int(0, -1), 
        new Vector2Int(1, 0), new Vector2Int(-1, 0)
    };

    foreach (PlayerAnt ant in playerAnts)
    {
        if (ant == null) continue;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = antLionPosition + dir;
            if (ant.gridPosition == checkPos)
            {
                float distance = Vector2Int.Distance(antLionPosition, ant.gridPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestAnt = ant;
                }
            }
        }
    }

    return closestAnt; // Return the closest player ant in attack range
}
private void AttackAnt(AntLion antLion, NPCAnt targetAnt)
{
    if (targetAnt == null || targetAnt.isDead) return;

    Debug.Log($"{antLion.name} attacks {targetAnt.name}!");
    targetAnt.hpCount -= 1; // Reduce HP by 1

    //targetAnt.checkHealth();

    // If the attacked ant is QueenAnt, notify GameManager
    if (targetAnt == queenAnt)
        {
            gameManager.HandleQueenAntDefeat();
        }
}

private void AttackAnt(AntLion antLion, PlayerAnt targetAnt)
{
    if (targetAnt == null || targetAnt.isDead) return;

    Debug.Log($"{antLion.name} attacks {targetAnt.name}!");
    targetAnt.loseHP(); // Reduce HP by 1

        targetAnt.checkHealth();
}

    /**
private void RemovePlayerAnt(PlayerAnt targetAnt)
{
    if (targetAnt == scoutAnt) scoutAnt = null;
    else if (targetAnt == builderAnt) builderAnt = null;
    else if (targetAnt == soldierAnt) soldierAnt = null;

    Destroy(targetAnt.gameObject); // Remove from scene
}

*/

private bool isMoveViable(Vector2Int position, Vector2Int queenPosition)
{
    return position.x >= 0 && position.x < gridSizeX && position.y >= 0 && position.y < gridSizeY
        && gridArray[position.x, position.y].isWalkable && !IsTileOccupiedByOtherAnts(position, queenPosition);
}

private Vector2Int findClosestPlayerAntPosition(Vector2Int antLionPosition)
{
    List<PlayerAnt> playerAnts = new List<PlayerAnt> { scoutAnt, builderAnt, soldierAnt };
    PlayerAnt closestAnt = null;
    float minDistance = float.MaxValue;

    foreach (PlayerAnt ant in playerAnts)
    {
        if (ant != null)
        {
            float distance = Vector2.Distance(antLionPosition, ant.gridPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestAnt = ant;
            }
        }
    }

    return closestAnt != null ? closestAnt.gridPosition : antLionPosition; // Return current position if no ant found
}



    public void ResetPlayer()
    {
        selectedAnt = -1;
    }

    private bool IsTileOccupiedByOtherAnts(Vector2Int position, Vector2Int queenPosition)
{
    return (scoutAnt != null && scoutAnt.gridPosition == position && position != queenPosition) ||
           (builderAnt != null && builderAnt.gridPosition == position && position != queenPosition) ||
           (antLionSecond != null && antLionSecond.gridPosition == position && position != queenPosition);
}


}

