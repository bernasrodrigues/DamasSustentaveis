using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIndicator : MonoBehaviour
{

    public GameObject moveIndicatorPrefab;
    private List<GameObject> moveIndicators = new List<GameObject>();
    public CheckersBoard board;




    public void PlaceIndicators()
    {
        ArrayList validMovesList = board.ValidMovesList;

        foreach (int[] item in validMovesList)
        {
            //Debug.Log(item[0] + " " + item[1]);
            GameObject go = Instantiate(moveIndicatorPrefab);
            moveIndicators.Add(go);
            board.MoveIndicator(go, item[0], item[1]);
        }


    }



    public void RemoveIndicators()
    {
        foreach (GameObject item in moveIndicators)
        {
            Destroy(item);
        }
    }



}
