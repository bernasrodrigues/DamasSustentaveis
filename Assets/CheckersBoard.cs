using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoard : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];           // represents board
    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;

    private Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private Vector2 mouseOver;
    private Piece selectedPiece;
    private Piece pieceToEat;

    private Vector2 startDrag;      // starting position of the selected piece
    private Vector2 endDrag;        // end position of the selected piece


    public ArrayList ValidMovesList;
    public MoveIndicator moveIndicator;


    private bool canMove;
    public int resourcesWhite = 15;
    public int resourcesBlack = 15;

    public bool isWhiteTurn;

    public ResourceDisplay resourceDisplay;


    /*
     board
   y  (0,7)      (7,7)
        ^
        |
        |
        |
        |
        ----------->
     (0,0)      (7,0)   x
     */




    private void Start()
    {
        GenerateBoard();
        resourceDisplay.UpdateValues(resourcesWhite, resourcesBlack , isWhiteTurn);
    }


    private void Update()
    {
        UpdateMouseOver();

        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;




        if (Input.GetMouseButtonDown(0))
        {
            print("Clicked: " + mouseOver);
            //if (selectedPiece == null || pieces[x,y] != null)
            if (selectedPiece == null)
            {

                SelectPiece(x, y);
            }

            else if (selectedPiece != null)
            {
                if (CanMove((int)startDrag.x, (int)startDrag.y, x, y))
                {

                    // TODO CHECK IF ATE PIECE
                    CheckIfAtePiece((int)startDrag.x, (int)startDrag.y, x, y);




                    // TODO CHECK IF ALLOWED TO EAT MORE





                    CalculateResourcesSpent((int)startDrag.x , x , selectedPiece.isKing);


                    // Perform move
                    pieces[(int)startDrag.x, (int)startDrag.y] = null;
                    pieces[x, y] = selectedPiece;
                    MovePiece(selectedPiece, x, y);
                    selectedPiece.selected = false;
                    selectedPiece = null;


                    ChangeTurn();

                }
                moveIndicator.RemoveIndicators();
            }

            resourceDisplay.UpdateValues(resourcesWhite, resourcesBlack , isWhiteTurn);


        }


        if (Input.GetKeyDown("space"))
        {
            if (selectedPiece != null) {

                if (selectedPiece.isWhite)
                {
                    resourcesWhite += 2;
                }
                else resourcesBlack += 2;

                Destroy(selectedPiece.gameObject);
                resourceDisplay.UpdateValues(resourcesWhite, resourcesBlack, isWhiteTurn);
                moveIndicator.RemoveIndicators();
            }

        }

        if (Input.GetKeyDown("return"))
        {
            ChangeTurn();
            resourceDisplay.UpdateValues(resourcesWhite, resourcesBlack, isWhiteTurn);

        }

    }

    private void CalculateResourcesSpent( int initialX , int finalX , bool isKing)
    {
        int costMultiplier = 2;
        if (isKing) costMultiplier = 1;

        int deltaMove = Mathf.Abs(initialX - finalX);

        if (isWhiteTurn)
            resourcesWhite -= (deltaMove * costMultiplier);
        else
            resourcesBlack -= (deltaMove * costMultiplier);
    }

    private void ChangeTurn()
    {
        // Check for promotions
        for (int x = 0; x < pieces.GetLength(0); x++)
        {
            if(pieces[x , 0] != null && !pieces[x , 0].isWhite)     // promote pieces on the first row of the board (black pieces)
            {
                pieces[x, 0].PromotePiece();
            }
            if (pieces[x, pieces.GetLength(1) - 1] != null && pieces[x, pieces.GetLength(1) - 1].isWhite)       // promote pieces on the last row of the board (white pieces)
            {
                pieces[x, pieces.GetLength(1) - 1].PromotePiece();
            }
        }





        // Change turn player
        if (isWhiteTurn)
            isWhiteTurn = false;
        else
            isWhiteTurn = true;
    }


    // Check if can move piece
    private bool CanMove(int x1, int y1, int x2, int y2)
    {
        // Check out of bounds
        if (x2 < 0 || x2 >= pieces.Length || y2 < 0 ||  y2 >= pieces.Length)
        {
            print("Forbiden move");
            selectedPiece.selected = false;
            selectedPiece = null;
            return false;
        }

        if (pieces[x2,y2] != null)
        {
            print("Ocupied space");
            selectedPiece.selected = false;
            selectedPiece = null;
            return false;
        }


        bool inValidMoves = false;
        foreach (int[] position in ValidMovesList)
        {
            if (position[0] == x2 && position[1] == y2)
            {
                inValidMoves = true;
            }
        }
        if (!inValidMoves)
        {
            print("Not a valid move");
            selectedPiece.selected = false;
            selectedPiece = null;
            return false;
        }


        
        /*
        // Perform move
        pieces[x1, y1] = null;
        pieces[x2, y2] = selectedPiece;
        MovePiece(selectedPiece, x2, y2);
        selectedPiece.selected = false;
        selectedPiece = null;
        */

        
        return true;

    }



    public void CheckIfAtePiece(int initialX , int initialY , int finalX , int finalY)
    {
        int x = initialX;
        if (initialY < finalY) // up move
        {
            if (initialX < finalX) //right move
            {
                for (int y = initialY + 1; y < finalY; y++)
                {
                    x++;
                    if (pieces[x, y] != null)
                    {
                        pieceToEat = pieces[x, y];
                    }
                }
            }
            else // left move
            {
                for (int y = initialY + 1; y < finalY; y++)
                {
                    x--;
                    if (pieces[x, y] != null)
                    {
                        pieceToEat = pieces[x, y];
                    }
                }

            }

        }
        else            // down move
        {
            if (initialX < finalX) //right move
            {
                for (int y = initialY - 1; y > finalY; y--)
                {
                    x++;
                    if (pieces[x, y] != null)
                    {
                        pieceToEat = pieces[x, y];
                    }
                }
            }
            else // left move
            {
                for (int y = initialY - 1; y > finalY; y--)
                {
                    x--;
                    if (pieces[x, y] != null)
                    {
                        pieceToEat = pieces[x,y];
                    }
                }

            }
        }


        if(pieceToEat != null)
        {
            CalculatePieceEaten(pieceToEat);
        }


    }

    // Calculate Resources 
    private void CalculatePieceEaten(Piece pieceToEat)
    {
        int resourcesToGain = (pieceToEat.isKing) ? 1 : 2;


        if (isWhiteTurn)
            resourcesWhite += resourcesToGain;

        else
            resourcesBlack += resourcesToGain;

        Destroy(pieceToEat.gameObject);


    }

    private void UpdateMouseOver()
    {
        if (!Camera.main)
        {
            Debug.LogError("Unable to find Camera");
            return;
        }


        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int) (hit.point.x - boardOffset.x);
            mouseOver.y = (int) (hit.point.z - boardOffset.z);

           
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }

    }


    private void SelectPiece(int x, int y)
    {
        // Check bounds
        if (x < 0 || x > pieces.Length || y < 0 || y >= pieces.Length)
            return;

        Piece p = pieces[x, y];

        if (p != null)
        {

            if ((isWhiteTurn && p.isWhite) || (!isWhiteTurn && !p.isWhite))
            {
                selectedPiece = p;
                p.selected = true;
                startDrag = mouseOver;

                ValidMovesList = selectedPiece.validMoves(pieces, x, y);
                moveIndicator.PlaceIndicators();
            }
        }



    }


    #region Board and Pieces Generation
    private void GenerateBoard()
    {
        // x = rows
        // y = column
        
        // For white team
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0);     // check if in an odd row (if y not divisible by 2 » odd row)
            for (int x = 0; x < 8; x += 2)
            {
                GeneratePiece(  ((oddRow) ? x : x +1)  ,  y , true);
            }
        }

        // For black team
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);     // check if in an odd row (if y not divisible by 2 » odd row)
            for (int x = 0; x < 8; x += 2)
            {
                GeneratePiece(((oddRow) ? x : x + 1), y , false);
            }
        }
    }

    private void GeneratePiece(int x, int y , bool isWhite)
    {
        GameObject go = Instantiate((isWhite)?WhitePiecePrefab:BlackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece piece = go.GetComponent<Piece>();
        pieces[x, y] = piece;
        MovePiece(piece, x, y);       // Adjust pieces position

    }

    #endregion
   
    private void MovePiece(Piece piece, int x, int y)
    {
        piece.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        

        // Update matrix position
    }



    public void MoveIndicator(GameObject Indicator, int x, int y)
    {
        Indicator.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

}
