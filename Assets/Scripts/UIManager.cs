using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gameStateText;
    public GameObject scoutAntUI;
    public GameObject builderAntUI;
    public TileInformationUI tileInfoUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScoutAntUI(){
        scoutAntUI.SetActive(true);
    }

    public void HideScoutAntUI(){
        scoutAntUI.SetActive(false);
    }

    public void ShowBuilderAntUI(){
        builderAntUI.SetActive(true);
    }

    public void HideBuilderAntUI(){
        builderAntUI.SetActive(false);
    }

    public void ShowTileInformation(string tileInfo)
    {
        if (tileInfoUI != null)
        {
            tileInfoUI.ShowTileInformation(tileInfo); 
        }
    }

    public void HideTileInformation()
    {
        if (tileInfoUI != null)
        {
            tileInfoUI.HideTileInformation(); 
        }
    }
}
