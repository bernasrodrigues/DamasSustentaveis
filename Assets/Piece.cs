using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    public bool selected;
    public bool isWhite;
    public bool isKing;


    public GameObject SelectedLight;
    public GameObject Coroa;




    void Start()
    {
        
    }


    //NOT USED
    public bool validMove(Piece[,] board, int x1 , int y1 , int x2 , int y2)
    {
        if (board[x2,y2] != null)       // check if free space (redundant)
        {
            return false; 
        }
        
        int deltaMove = (int) (x1 - x2);
        int deltaMoveY = (int) (y1 - y2);


        if (Mathf.Abs(deltaMove )!= Mathf.Abs(deltaMoveY))        // check if the move is diagonal
        {
            return false;
        }


        int piecesInMovePath = 0;
        int x = x1;
        int y = y1;


        int XMove = (x2 > x1) ? 1 : -1;
        



        if (isWhite || isKing)
        {
            if (!isKing && deltaMoveY > 0)      // prevent white non-king pieces from moving backards
            {
                return false;
            }




           



            while (x != x2)         // check for pieces in move path
            {
                y++;
                x += XMove;


                if (piecesInMovePath > 0 && x != x2)        // if detected a piece and trying to move further than beyond piece
                {
                    return false;
                }





                if (board[x, y] != null)
                {
                    piecesInMovePath++;

                    if (piecesInMovePath > 1)
                    {
                        return false;
                    }
                }


            }
        }


        if (!isWhite || isKing)
        {
            if (!isKing && deltaMoveY < 0)      // prevent black non-king pieces from moving backards
            {
                return false;
            }




            


            while (x != x2)
            {
                y--;
                x += XMove;


                if (piecesInMovePath > 0 && x != x2)
                {
                    return false;
                }



                if (board[x, y] != null)
                {
                    piecesInMovePath++;
                }


            }
        }






        return true;

    }




    public ArrayList validMoves(Piece[,] board , int x1, int y1)
    {
        
        // x1 = piece x
        // y1 = piece y
        ArrayList validMovesArray = new ArrayList();
        int x_neg = x1;         // go through spaces left of the piece (move left)
        int x_pos = x1;         // go through spaces right of the piece (move right)
        int y = y1;             // go through vertical spaces


        // For white pieces or King
        if (isWhite || isKing)
        {
            x_neg = x1;
            x_pos = x1;



            // right moves
            y = y1 + 1;
            for (int x = x1 + 1; x < board.GetLength(0); x++)
            {
                if (x > board.GetLength(0) -1 || y > board.GetLength(1) -1) break;            //prevent edge cases on first and last row


                if (board[x, y] == null)        // Check for empty positions
                    validMovesArray.Add(new int[] { x, y });

                ////else if (!board[x, y].isWhite)                               // check if black piece in the way and then calculate if can eat
                 else if (CheckIfCanEat(board[x, y]))                           // check if black piece in the way and then calculate if can eat
                {
                    if (x + 1 < board.GetLength(0) && y + 1 < board.GetLength(1) && board[x + 1, y + 1] == null)
                        validMovesArray.Add(new int[] { x + 1, y + 1 });

                    break;
                }

                else break;       // if white piece in path -> break

                y++;
            }




            // left moves
            y = y1 + 1;
            for (int x = x1 - 1; x >= 0; x--)
            {
                if (x < 0 || y > board.GetLength(1) -1) break;                     //prevent edge cases on first and last row


                if (board[x, y] == null)        // Check for empty positions
                    validMovesArray.Add(new int[] { x, y });

                ////else if (!board[x, y].isWhite)                              // check if black piece in the way and then calculate if can eat
                else if (CheckIfCanEat(board[x, y]))                           // check if black piece in the way and then calculate if can eat
                {
                    if (x - 1 >= 0 && y + 1 <= board.GetLength(1) && board[x - 1, y + 1] == null)
                        validMovesArray.Add(new int[] { x - 1, y + 1 });
                    break;
                }
                else break;        // if white piece in path -> break

                y++;
            }
        }


        // Same for black pieces or King
        if (!isWhite || isKing)
        {
            x_neg = x1;
            x_pos = x1;



            // right moves
            y = y1 - 1;
            for (int x = x1 + 1; x < board.GetLength(0); x++)
            {
                if (x > board.GetLength(0)-1 || y < 0) break;                     //prevent edge cases on first and last row



                if (board[x, y] == null)        // Check for empty positions
                    validMovesArray.Add(new int[] { x, y });

                ////else if (board[x, y].isWhite)                               // check if white piece in the way and then calculate if can eat
                else if (CheckIfCanEat(board[x, y]))                           // check if black piece in the way and then calculate if can eat
                {
                    if (x + 1 < board.GetLength(0) && y - 1 > 0 && board[x + 1, y - 1] == null)
                        validMovesArray.Add(new int[] { x + 1, y - 1 });

                    break;
                }

                else break;       // if black piece in path -> break

                y--;
            }




            // left moves
            y = y1 - 1;
            for (int x = x1 - 1; x >= 0; x--)
            {
                if (x < 0 || y < 0) break;                     //prevent edge cases on first and last row

                if (board[x, y] == null)        // Check for empty positions
                    validMovesArray.Add(new int[] { x, y });

                ////else if (board[x, y].isWhite)                              // check if white piece in the way and then calculate if can eat
                else if (CheckIfCanEat(board[x, y]))                           // check if black piece in the way and then calculate if can eat
                {
                    if (x - 1 >= 0 && y - 1 > 0 && board[x - 1, y - 1] == null)
                        validMovesArray.Add(new int[] { x - 1, y - 1 });
                    break;
                }
                else break;        // if white piece in path -> break

                y--;
            }

        }







        return validMovesArray;
    }




    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            SelectedLight.SetActive(true);


            if(Input.GetKeyDown("space"))
        {
                Destroy(gameObject);
            }

        }
        else
        {
            SelectedLight.SetActive(false);
        }
    }



    bool CheckIfCanEat(Piece p) {


        if (p.isWhite && !isWhite)           // if piece on board is white and current piece is black
            return true;


        if (!p.isWhite && isWhite)            // if piece on board is black and current piece is white
            return true;




        return false;
    }


    public void PromotePiece()
    {
        isKing = true;
        Coroa.SetActive(true);
    }





    #region Random Methods

    void PrintArray(ArrayList a)
    {
        foreach (var item in a)
        {
            Debug.Log(item);
        }


    }



    #endregion
}


/*

                // Check left possible moves
                if ((x_neg >= 0 && x_neg <= board.GetLength(0)))      // check if within board
                {
                    if (board[x_neg,y] == null)
                    {
                        validMovesArray.Add( (x_neg, y) );
                    }
                }


                // Check right possible moves
                if ((x_pos >= 0 && x_pos <= board.GetLength(0)))      // check if within board
                {
                    if (board[x_pos, y] == null)
                    {
                        validMovesArray.Add((x_pos, y));
                    }
                }

                x_neg--;
                x_pos++;
                */
