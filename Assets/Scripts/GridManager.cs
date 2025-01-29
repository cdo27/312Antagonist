using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public BaseTile[,] gridArray; // 2D array to hold the references to all the tiles
    public int gridSizeX = 8; // Number of tiles in the X direction
    public int gridSizeY = 8; // Number of tiles in the Y direction

    // ref to game objects
    public ScoutAnt scoutAnt;
    public QueenAnt queenAnt;
    public GameManager gameManager;
    public UIManager uiManager;

    public int selectedAnt; //Scout = 0, Builder = 1

    //QueenAnt path
    private List<Vector2Int> queenPath;
    private int currentPathIndex = 0; 

    void Start()
    {
        selectedAnt = -1;
        gridArray = new BaseTile[gridSizeX, gridSizeY];

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
                        if (scoutAnt != null && scoutAnt.gridPosition == new Vector2Int(baseTile.xIndex, baseTile.yIndex))
                        {
                            selectedAnt = 0;
                            Debug.Log("Scout Ant Selected!");
                        }

                        //Move on hightlighted tiles
                        if(selectedAnt == 0 && baseTile.isHighlighted){
                            MoveScoutAnt(baseTile);
                        }
                        
                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) //Right click to deselect everything?
            {
                selectedAnt = -1; //deselect ant
                uiManager.HideScoutAntUI();
                UnhighlightSurroundingTiles(scoutAnt.gridPosition);
                Debug.Log("Deselect everything!");
            }

            if(selectedAnt == 0){ //Scout Ant
                uiManager.ShowScoutAntUI();
                if(scoutAnt.hasMoved == false) HighlightSurroundingTiles(scoutAnt.gridPosition);
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

    // Highlight tiles around the Scout Ant
    void HighlightSurroundingTiles(Vector2Int antPosition)
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
                  
                    if (tile != null&& tile.isWalkable)
                    {
                    
                        tile.Highlight();
                    }
                }
            }
        }
    }

    void UnhighlightSurroundingTiles(Vector2Int antPosition)
    {
        // Loop through the grid to check all tiles within the radius (1 tile in all directions)
        for (int x = antPosition.x - 1; x <= antPosition.x + 1; x++)
        {
            for (int y = antPosition.y - 1; y <= antPosition.y + 1; y++)
            {
                // Check if the tile is within bounds
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                {
                    BaseTile tile = gridArray[x, y];

                    if (tile != null)
                    {
                        tile.Unhighlight();  // Unhighlight the tile
                    }
                }
            }
        }
    }

    void MoveScoutAnt(BaseTile targetTile)
    {
        // Unhighlight the previous tile
        UnhighlightSurroundingTiles(scoutAnt.gridPosition);

        // Move the ScoutAnt to the target tile
        scoutAnt.gridPosition = new Vector2Int(targetTile.xIndex, targetTile.yIndex);
        scoutAnt.transform.position = targetTile.transform.position;
        
        scoutAnt.hasMoved = true;

    }

    public void moveQueen(){
        if (queenPath != null && currentPathIndex < queenPath.Count)
        {
            // Get the target position from the path
            Vector2Int targetPosition = queenPath[currentPathIndex];

            // Find the BaseTile at the target position
            BaseTile targetTile = gridArray[targetPosition.x, targetPosition.y];

            if (targetTile != null)
            {
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

}
