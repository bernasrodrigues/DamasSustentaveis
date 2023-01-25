using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ResourceDisplay : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI tm;






    public void UpdateValues(int whiteResources, int blackResources , bool isWhiteTurn)
    {
        string playerTurn;
        if (isWhiteTurn)
        {
            playerTurn = "White";
        }
        else
            playerTurn = "black";


        tm.SetText("White resources: " + whiteResources + "\n" + "Black resources: " + blackResources + "\n" + playerTurn + " turn");



    }




    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
