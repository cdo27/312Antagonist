using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileInformationUI : MonoBehaviour
{

    public TextMeshProUGUI tileInformationText;
    public Image tileInformationImage;
    public TextMeshProUGUI tileInformationContent;

    //Tile Library
    public Sprite groundTile;
    public Sprite waterTile;
    public Sprite trapTile;
    public Sprite deployTile;
    public Sprite noTile;

    // Start is called before the first frame update
    void Start()
    {
        tileInformationText.gameObject.SetActive(false);
        tileInformationImage.gameObject.SetActive(false);
        tileInformationContent.gameObject.SetActive(false);
    }

    public void ShowTileInformation(string tileType)
    {
        //Debug.Log("Tile Information Box: showing information!");

        //for text
        tileInformationText.text = tileType;
        tileInformationText.gameObject.SetActive(true);

        //For image
        //change png depending on which typeType is returned
        if (tileType == null)
        {
            //just setting an initial to prevent reference error.
            tileInformationImage.sprite = noTile;
        }
        else if (tileInformationImage != null)
        {
            // Set the sprite based on the tile type
            switch (tileType)
            {
                case "Ground":
                    tileInformationImage.sprite = groundTile;
                    break;
                case "Water":
                    tileInformationImage.sprite = waterTile;
                    break;
                case "Trap":
                    tileInformationImage.sprite = trapTile;
                    break;
                case "Deploy":
                    tileInformationImage.sprite = deployTile;
                    break;
                default:
                    tileInformationImage.sprite = noTile; // Default image
                    break;
            }
        }

        tileInformationImage.gameObject.SetActive(true);

        //For content
        switch (tileType)
        {
            case "Ground":
                tileInformationContent.text = "This tile has no effect";
                break;
            case "Water":
                tileInformationContent.text = "Cannot be walked on, all entities die in water";
                break;
            case "Trap":
                tileInformationContent.text = "Take 1 damage when entities walk on this tile";
                break;
            case "Deploy":
                tileInformationContent.text = "You can deploy units on this tile";
                break;
            default:
                tileInformationContent.text = "default text";
                break;
        }
        tileInformationContent.gameObject.SetActive(true);


    }

    public void HideTileInformation()
    {
        tileInformationText.gameObject.SetActive(false);
        tileInformationImage.gameObject.SetActive(false);
        tileInformationContent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
