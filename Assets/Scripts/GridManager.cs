using UnityEngine;

public class GridManager : MonoBehaviour
{
    public BaseTile[,] gridArray; // 2D array to hold the references to all the tiles
    public int gridSizeX = 8; // Number of tiles in the X direction
    public int gridSizeY = 8; // Number of tiles in the Y direction

    public GameManager gameManager;

    void Start()
    {
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
                    }
                }
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

}
