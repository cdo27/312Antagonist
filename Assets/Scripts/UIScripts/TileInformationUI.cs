using TMPro;
using UnityEngine;

public class TileInformationUI : MonoBehaviour
{

    public TextMeshProUGUI tileInformationText; 

    // Start is called before the first frame update
    void Start()
    {
        tileInformationText.gameObject.SetActive(false);
    }

    public void ShowTileInformation(string tileType)
    {
        Debug.Log("showing information!");
        tileInformationText.text = tileType;
        tileInformationText.gameObject.SetActive(true); 
    }

    public void HideTileInformation()
    {
        tileInformationText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
