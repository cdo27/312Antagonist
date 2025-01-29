using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI gameStateText;
    public GameObject scoutAntUI;

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
}
