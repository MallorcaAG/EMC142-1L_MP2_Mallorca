using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.VFX;

public class gameAI : MonoBehaviour
{
   /* public enum levels {lvl1,lvl2,lvl3};
    public levels LevelDifficulty;*/

    public GameManager manager;

    public int level = 0;
        
    private short myTurn = 2;
    private int moves = 1;
    private bool markedtopLcor = false, markedbotRcor = false, markedoppcor = false;
    private bool ITakeTopMid = false, ITakeMidLeft = false, ITakeMidRight = false, ITakeBotMid = false;
    private bool blockcor1 = false, blockcor2 = false, blockcor3 = false, blockcor4 = false;


    //AI is player2
    private bool OppTookCenter = false;
    private bool RisingCornerPlayed = false;
    private bool FallingCornerPlayed = false;
    private bool OppTookASide = false;
    private bool blockSide = false;
    private bool forceDraw1 = false;
    private bool tile0mine = false;
    private bool tile1mine = false;
    private bool tile2mine = false;
    private bool tile3mine = false;
    private bool tile4mine = false;
    private bool tile5mine = false;
    private bool tile6mine = false;
    private bool tile7mine = false;
    private bool tile8mine = false;
    private bool trapSet = false;
    private bool adjacent = false;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        if (manager.AIControlled)
        {
            if(manager.HumanIsPlayer2)
            {
                myTurn = 1;
            }
            else
            {
                myTurn = 2;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
        getLevel();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(manager.gameState==myTurn)
        {
            lockCursor();
            if (level == 2)
            {
                Lvl2();
            }
            else if (level == 1)
            {
                Lvl1();
            } 
            else if (level == 3)
            {
                Lvl3();
            }
            //endTurn();
            unlockCursor();
            
        }
    }

    void getLevel()
    {
        level = manager.levelNum;
    }




    void Lvl1()
    {
        for (int num = Random.Range(0, manager.tiles.Length); num <= manager.tiles.Length; num = Random.Range(0, manager.tiles.Length))
        {
            if(manager.tiles[num].GetComponent<tileState>().state == 2)
            {
                manager.mark(manager.tiles[num]);
                
                break;
            }
        }
    }

    void Lvl2()
    {
        int heads = Random.Range(-2, 2);
        Debug.Log(heads);
        Debug.Log("NOTE: negative number calls Level 1 AI, 0 and positive calls Level 3");
        if (heads<0)
        {
            level = 1;
        }
        else
        {
            level = 3;
        }
    }



    void Lvl3()
    {
        //Spagetti time
        short center = 4;

        

        


        if(moves ==1)
        {
            if (myTurn == 1)//if I go first
            {
                //Go to corner
                manager.mark(manager.tiles[center + 2]);
            }
            else //if I go second, play defensively
            {

                if (manager.tiles[4].GetComponent<tileState>().state == 2) //If center is unoccupied
                {
                    //Go to center
                    manager.mark(manager.tiles[center]);
                    tile4mine = true;

                    if((manager.tiles[1].GetComponent<tileState>().state == 2) && //Opponent is at the corner
                       (manager.tiles[3].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[5].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[7].GetComponent<tileState>().state == 2))
                    {
                        OppTookASide = false;
                    }
                    else
                    {
                        OppTookASide=true;
                    }
                }
                else
                {
                    manager.mark(manager.tiles[0]);
                    OppTookCenter = true;
                    tile0mine= true;
                }
            }
            moves = 2;

        }
        else if(moves ==2)
        {
            if (myTurn == 1)//If I went first
            {
                if (manager.tiles[4].GetComponent<tileState>().state == 1) //if opponent took center
                {
                    //Play opposite corner
                    manager.mark(manager.tiles[center - 2]);
                    markedoppcor = true;
                }
                else if ((manager.tiles[0].GetComponent<tileState>().state == 1) ||
                         (manager.tiles[2].GetComponent<tileState>().state == 1)) //if opponent is in [0,2,3,5]
                {
                    manager.mark(manager.tiles[center + 4]);
                    markedbotRcor = true;
                    //opponent will block [center+3]
                }
                else if ((manager.tiles[8].GetComponent<tileState>().state == 1)) //if opponent is in [1,7,8]
                {
                    manager.mark(manager.tiles[center - 4]);
                    markedtopLcor = true;
                    //opponent will block [center-1]
                }
                else if (manager.tiles[3].GetComponent<tileState>().state == 1)
                {
                    manager.mark(manager.tiles[center + 4]);
                    markedbotRcor = true;
                    ITakeMidLeft = true;

                }
                else if (manager.tiles[5].GetComponent<tileState>().state == 1)
                {
                    manager.mark(manager.tiles[center + 4]);
                    markedbotRcor = true;
                    ITakeMidRight = true;
                }
                else if (manager.tiles[1].GetComponent<tileState>().state == 1)
                {
                    manager.mark(manager.tiles[center - 4]);
                    markedtopLcor = true;
                    ITakeTopMid = true;
                }
                else if (manager.tiles[7].GetComponent<tileState>().state == 1)
                {
                    manager.mark(manager.tiles[center - 4]);
                    markedtopLcor = true;
                    ITakeBotMid = true;
                }
            }
            else //If I went second
            {

                if (OppTookCenter)
                {
                    
                    if ((manager.tiles[1].GetComponent<tileState>().state == 2) && //If this is empty, opponent took corner
                       (manager.tiles[3].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[5].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[7].GetComponent<tileState>().state == 2))
                    {
                        if (manager.tiles[6].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[6]);

                            //Win if Marked 3
                            ITakeMidLeft = true;
                        }
                        else
                        {
                            manager.mark(manager.tiles[2]);

                            //Win if Marked 1
                            ITakeTopMid = true;
                        }
                    }
                    else
                    {
                        int x = manager.tiles[1].GetComponent<tileState>().state != 2 ? 7 :
                                manager.tiles[3].GetComponent<tileState>().state != 2 ? 5 :
                                manager.tiles[5].GetComponent<tileState>().state != 2 ? 3 : 1;

                        manager.mark(manager.tiles[x]);
                        blockSide = true;
                        
                    }
                }
                else                    //We have center
                {
                    if(((manager.tiles[0].GetComponent<tileState>().state != 2)&&(manager.tiles[8].GetComponent<tileState>().state != 2)))
                    {
                        FallingCornerPlayed = true;
                        manager.mark(manager.tiles[7]);
                    }
                    else if((manager.tiles[2].GetComponent<tileState>().state != 2) && (manager.tiles[6].GetComponent<tileState>().state != 2))
                    {
                        RisingCornerPlayed = true;
                        manager.mark(manager.tiles[7]);
                    }
                    else if (!((manager.tiles[1].GetComponent<tileState>().state == 2) && //Opponent is at the sides
                       (manager.tiles[3].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[5].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[7].GetComponent<tileState>().state == 2)))
                    {
                        OppTookASide = true;
                    }
                    if (!OppTookASide && !(FallingCornerPlayed||RisingCornerPlayed)) //Opponent is in corner
                    {
                        if((manager.tiles[0].GetComponent<tileState>().state == 0) && (manager.tiles[2].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[1]);
                            //ITakeTopMid = true;

                            //Take tile 3 next turn
                            tile3mine = true;
                        }
                        else if ((manager.tiles[0].GetComponent<tileState>().state == 0) && (manager.tiles[6].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[3]);
                            //ITakeMidLeft = true;

                            //Take tile 1 next turn
                            tile1mine = true;
                        }
                        else if((manager.tiles[2].GetComponent<tileState>().state == 0) && (manager.tiles[8].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[5]);
                            //ITakeMidRight = true;

                            //Take tile 1 next turn
                            tile1mine=true;
                        }
                        else if ((manager.tiles[6].GetComponent<tileState>().state == 0) && (manager.tiles[8].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[7]);
                            //ITakeBotMid = true;

                            //Take tile 3 next turn
                            tile3mine=true;
                        }
                        
                    }
                    else if (OppTookASide)   //If opponent took side
                    {   
                        //TOP
                        if ((manager.tiles[0].GetComponent<tileState>().state == 0) && (manager.tiles[1].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);

                            //Opponent must block corner 3
                            blockcor3 = true;

                            ITakeMidLeft = true;
                        }
                        else if ((manager.tiles[1].GetComponent<tileState>().state == 0) && (manager.tiles[2].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[0]);

                            //Opponent must block corner 4
                            blockcor4 = true;

                            ITakeMidRight = true;
                        }
                        //LEFT
                        else if ((manager.tiles[0].GetComponent<tileState>().state == 0) && (manager.tiles[3].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);

                            //Opponent must block corner 2
                            blockcor2 = true;

                            ITakeTopMid = true;
                        }
                        else if ((manager.tiles[3].GetComponent<tileState>().state == 0) && (manager.tiles[6].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[0]);

                            //Opponent must block corner 4
                            blockcor4 = true;

                            ITakeBotMid = true;
                        }
                        //RIGHT
                        else if ((manager.tiles[2].GetComponent<tileState>().state == 0) && (manager.tiles[5].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[8]);

                            //Opponent must block corner 1
                            blockcor1 = true;

                            ITakeTopMid = true;
                        }
                        else if ((manager.tiles[5].GetComponent<tileState>().state == 0) && (manager.tiles[8].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);

                            //Opponent must block corner 3
                            blockcor3 = true;

                            ITakeBotMid = true;
                        }
                        //BOTTOM
                        else if ((manager.tiles[6].GetComponent<tileState>().state == 0) && (manager.tiles[7].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[8]);
                            //Opponent must block corner 1
                            blockcor1 = true;

                            ITakeMidLeft = true;
                        }
                        else if ((manager.tiles[7].GetComponent<tileState>().state == 0) && (manager.tiles[8].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);
                            //Opponent must block corner 2
                            blockcor2 = true;

                            ITakeMidRight = true;
                        }
                        else if ((manager.tiles[1].GetComponent<tileState>().state == 0) && (manager.tiles[3].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[0]);
                            //Opponent must block corner 4
                            blockcor4 = true;

                            adjacent = true;
                        }
                        else if ((manager.tiles[1].GetComponent<tileState>().state == 0) && (manager.tiles[5].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);
                            //Opponent must block corner 3
                            blockcor3 = true;

                            adjacent = true;
                        }
                        else if ((manager.tiles[3].GetComponent<tileState>().state == 0) && (manager.tiles[7].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);
                            //Opponent must block corner 2
                            blockcor2 = true;

                            adjacent = true;
                        }
                        else if ((manager.tiles[5].GetComponent<tileState>().state == 0) && (manager.tiles[7].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[8]);
                            //Opponent must block corner 1
                            blockcor1 = true;

                            adjacent = true;
                        }

                        //if opponent takes far corner after playing side
                        else 
                        {
                            //TOP
                            if ((manager.tiles[1].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[8].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[5]);

                                //Take tile 1 next turn
                                tile0mine = true;
                            }
                            else if ((manager.tiles[1].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[6].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[3]);

                                tile2mine = true;
                            }
                            //LEFT
                            else if ((manager.tiles[3].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[2].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[1]);

                                tile6mine = true;
                            }
                            else if ((manager.tiles[3].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[8].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[7]);

                                tile0mine = true;
                            }
                            //RIGHT
                            else if ((manager.tiles[5].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[0].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[1]);

                                tile8mine = true;
                            }
                            else if ((manager.tiles[5].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[6].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[7]);

                                tile2mine = true;
                            }
                            //BOTTOM
                            else if ((manager.tiles[0].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[7].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[3]);

                                tile8mine = true;
                            }
                            else if ((manager.tiles[2].GetComponent<tileState>().state == 0) &&
                                 (manager.tiles[7].GetComponent<tileState>().state == 0))
                            {
                                manager.mark(manager.tiles[5]);

                                tile6mine = true;
                            }
                        }
                    }
                    
                }
            }

            moves = 3;

        }
        else if(moves==3)
        {
            
            if (myTurn == 1)//If I went first
            {
                //Debug.Log("OUTPUT MESSAGE");        //CODE ERROR  //ERROR RESOLVED
                //Opponent played center and blunders
                if (markedoppcor && (manager.tiles[0].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[8]);
                    markedbotRcor = true;
                }
                else if (markedoppcor && (manager.tiles[8].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[0]);
                    markedtopLcor = true;
                }
                //Opponent played center and forces draw
                else if (markedoppcor && (manager.tiles[1].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[7]);
                    ITakeBotMid = true;
                }
                else if (markedoppcor && (manager.tiles[3].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[5]);
                    ITakeMidRight = true;
                }
                else if (markedoppcor && (manager.tiles[5].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[3]);
                    ITakeMidLeft = true;
                }
                else if (markedoppcor && (manager.tiles[7].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[1]);
                    ITakeTopMid = true;
                }
                //opponent did not block
                else if (markedbotRcor && (manager.tiles[center + 3].GetComponent<tileState>().state == 2))
                {
                    manager.mark(manager.tiles[center + 3]);    //WIN

                }
                else if (markedbotRcor && !ITakeMidRight && manager.tiles[center - 2].GetComponent<tileState>().state == 2)
                {
                    manager.mark(manager.tiles[center - 2]);
                    markedoppcor = true;

                }
                else if (manager.tiles[0].GetComponent<tileState>().state == 2)
                {
                    manager.mark(manager.tiles[0]);
                    markedtopLcor = true;

                }
                else if (markedtopLcor && (manager.tiles[center - 1].GetComponent<tileState>().state == 2))
                {
                    manager.mark(manager.tiles[center - 1]);    //WIN
                }
                else if (ITakeTopMid && markedtopLcor)
                {
                    manager.mark(manager.tiles[center + 4]);
                    markedbotRcor = true;

                }
                else if (markedtopLcor && !ITakeTopMid && (manager.tiles[center - 1].GetComponent<tileState>().state == 1))
                {
                    manager.mark(manager.tiles[center - 2]);
                    markedoppcor = true;
                }
                else if (ITakeBotMid && manager.tiles[center - 2].GetComponent<tileState>().state == 2)
                {
                    manager.mark(manager.tiles[center - 2]);
                    markedoppcor = true;
                }
                else if (manager.tiles[0].GetComponent<tileState>().state == 2)
                {
                    manager.mark(manager.tiles[0]);
                    markedtopLcor = true;

                }


            }
            else //If I went second,
            {
                if(OppTookCenter)
                {
                    ///Wining states
                    if(ITakeMidLeft && (manager.tiles[3].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[3]);
                    }
                    else if (ITakeMidLeft && !(manager.tiles[3].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[5]);

                        tile1mine = true;
                    }
                    else if (ITakeTopMid && (manager.tiles[1].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[1]);
                    }
                    else if (ITakeTopMid && !(manager.tiles[1].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[7]);

                        tile3mine = true;
                    }

                    //Opponent block sides
                    else if (blockSide && (manager.tiles[3].GetComponent<tileState>().state == 1)) //If we blocked tile3
                    {
                        if((manager.tiles[6].GetComponent<tileState>().state == 2))
                        {//Win
                            manager.mark(manager.tiles[6]);
                        }
                        else if ((manager.tiles[6].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);

                            //Take tile 1 next turn
                            tile1mine = true;
                        }                      
                    }
                    else if (blockSide && (manager.tiles[1].GetComponent<tileState>().state == 1)) //If we blocked tile1
                    {
                        if ((manager.tiles[2].GetComponent<tileState>().state == 2))
                        {
                            //Win
                            manager.mark(manager.tiles[2]);
                        }
                        else if ((manager.tiles[2].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);

                            //Take tile 3 next turn
                            tile3mine = true;
                        }
                    }
                    else if (blockSide && (manager.tiles[7].GetComponent<tileState>().state == 1)) //If we blocked tile7
                    {
                        if ((manager.tiles[2].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);

                            //Go tile8 or tile3 to win
                            tile8mine = true;

                            trapSet = true;
                        }
                        else if ((manager.tiles[3].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[5]);

                            //Take tile 2 next turn
                            tile2mine=true;
                        }
                        else if ((manager.tiles[5].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[3]);

                            //Take tile 6 next turn
                            tile6mine = true;
                        }
                        else if ((manager.tiles[6].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);
                            tile5mine = true;

                        }
                    }
                    else if (blockSide && (manager.tiles[5].GetComponent<tileState>().state == 1)) //If we blocked tile5
                    {
                        if ((manager.tiles[1].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[7]);

                            tile2mine = true;
                        }
                        else if ((manager.tiles[7].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[1]);

                            tile2mine=(true);
                        }
                        else if ((manager.tiles[2].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[6]);

                            tile1mine = true;
                        }
                        else if ((manager.tiles[6].GetComponent<tileState>().state == 0))
                        {
                            manager.mark(manager.tiles[2]);
                            tile1mine=(true);

                            trapSet = true;
                        }
                    }






                }
                else           //WE HAVE CENTER
                {
                    //WINNING STATES
                    if((manager.tiles[1].GetComponent<tileState>().state == 2) &&
                       (manager.tiles[7].GetComponent<tileState>().state == 1))
                    {
                        manager.mark(manager.tiles[1]);
                    }
                    else if ((manager.tiles[7].GetComponent<tileState>().state == 2) &&
                             (manager.tiles[1].GetComponent<tileState>().state == 1))
                    {
                        manager.mark(manager.tiles[7]);
                    }
                    else if ((manager.tiles[3].GetComponent<tileState>().state == 2) &&
                             (manager.tiles[5].GetComponent<tileState>().state == 1))
                    {
                        manager.mark(manager.tiles[3]);
                    }
                    else if ((manager.tiles[5].GetComponent<tileState>().state == 2) &&
                             (manager.tiles[3].GetComponent<tileState>().state == 1))
                    {
                        manager.mark(manager.tiles[5]);
                    }

                    else if(blockcor1)
                    {
                        if((manager.tiles[0].GetComponent<tileState>().state == 2))
                        {
                            //WIN
                            manager.mark(manager.tiles[0]);
                        }
                        else if (adjacent)
                        {
                            manager.mark(manager.tiles[6]);
                            tile2mine = true;

                        }
                        else if(ITakeTopMid)
                        {
                            manager.mark(manager.tiles[1]);

                            forceDraw1 = true;
                            tile7mine = true;
                        }
                        else if (ITakeMidLeft)
                        {
                            manager.mark(manager.tiles[3]);

                            forceDraw1 = true;
                            tile5mine = true;
                        }
                        else if (ITakeMidRight)
                        {
                            manager.mark(manager.tiles[5]);

                            forceDraw1 = true;
                            tile3mine = true;
                        }
                        else if (ITakeBotMid)
                        {
                            manager.mark(manager.tiles[7]);

                            forceDraw1 = true;
                            tile1mine = true;
                        }
                    }
                    else if (blockcor2)
                    {
                        if ((manager.tiles[2].GetComponent<tileState>().state == 2))
                        {
                            //WIN
                            manager.mark(manager.tiles[2]);
                        }
                        else if (adjacent)
                        {
                            manager.mark(manager.tiles[8]);
                            tile0mine = true;

                        }
                        else if (ITakeTopMid)
                        {
                            manager.mark(manager.tiles[1]);

                            forceDraw1 = true;
                            tile7mine = true;
                        }
                        else if (ITakeMidLeft)
                        {
                            manager.mark(manager.tiles[3]);

                            forceDraw1 = true;
                            tile5mine = true;
                        }
                        else if (ITakeMidRight)
                        {
                            manager.mark(manager.tiles[5]);

                            forceDraw1 = true;
                            tile3mine = true;
                        }
                        else if (ITakeBotMid)
                        {
                            manager.mark(manager.tiles[7]);

                            forceDraw1 = true;
                            tile1mine = true;
                        }
                    }
                    else if (blockcor3)
                    {
                        if ((manager.tiles[6].GetComponent<tileState>().state == 2))
                        {
                            //WIN
                            manager.mark(manager.tiles[6]);
                        }
                        else if (adjacent)
                        {
                            manager.mark(manager.tiles[8]);

                            //Take tile 0 next turn
                            tile0mine = true;

                        }
                        else if (ITakeTopMid)
                        {
                            manager.mark(manager.tiles[1]);

                            forceDraw1 = true;
                            tile7mine = true;
                        }
                        else if (ITakeMidLeft)
                        {
                            manager.mark(manager.tiles[3]);

                            forceDraw1 = true;
                            tile5mine = true;
                        }
                        else if (ITakeMidRight)
                        {
                            manager.mark(manager.tiles[5]);

                            forceDraw1 = true;
                            tile3mine = true;
                        }
                        else if (ITakeBotMid)
                        {
                            manager.mark(manager.tiles[7]);

                            forceDraw1 = true;
                            tile1mine = true;
                        }
                    }
                    else if(blockcor4)
                    {
                        if((manager.tiles[8].GetComponent<tileState>().state == 2))
                        {
                            //WIN
                            manager.mark(manager.tiles[8]);
                        }
                        else if(adjacent)
                        {
                            manager.mark(manager.tiles[6]);

                            //Take tile 2 next turn
                            tile2mine = true;

                        }
                        else if(ITakeTopMid)
                        {
                            manager.mark(manager.tiles[1]);

                            forceDraw1 = true;
                            tile7mine = true;
                        }
                        else if (ITakeMidLeft)
                        {
                            manager.mark(manager.tiles[3]);

                            forceDraw1 = true;
                            tile5mine = true;
                        }
                        else if (ITakeMidRight)
                        {
                            manager.mark(manager.tiles[5]);

                            forceDraw1 = true;
                            tile3mine = true;
                        }
                        else if (ITakeBotMid)
                        {
                            manager.mark(manager.tiles[7]);

                            forceDraw1 = true;
                            tile1mine = true;
                        }
                    }



                    //BLOCK
                    else if (tile0mine && (manager.tiles[0].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[0]);
                    }
                    else if (tile1mine && (manager.tiles[1].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[1]);

                        ITakeBotMid = true;
                    }
                    else if(tile2mine && (manager.tiles[2].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[2]);
                    }
                    else if(tile3mine && (manager.tiles[3].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[3]);

                        if((manager.tiles[6].GetComponent<tileState>().state == 2)||
                           (manager.tiles[7].GetComponent<tileState>().state == 2)||
                           (manager.tiles[8].GetComponent<tileState>().state == 2))
                        {
                            ITakeBotMid = true;
                        }
                        else
                        {
                            blockcor2 = true;
                        }
                    }
                    else if(tile4mine && (manager.tiles[4].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[4]);
                    }
                    else if(tile5mine && (manager.tiles[5].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[5]);
                    }
                    else if(tile6mine && (manager.tiles[6].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[6]);
                    }
                    else if(tile7mine && (manager.tiles[7].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[7]);
                    }
                    else if(tile8mine && (manager.tiles[8].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[8]);
                    }

                    if(RisingCornerPlayed || FallingCornerPlayed)
                    {
                        if(manager.tiles[1].GetComponent<tileState>().state == 2)
                        {
                            //WIN
                            manager.mark(manager.tiles[1]);
                        }
                        else if(RisingCornerPlayed)
                        {
                            manager.mark(manager.tiles[0]);

                            //Opponent will block corner 3
                            blockcor4 = true;
                        }
                        else if (FallingCornerPlayed)
                        {
                            manager.mark(manager.tiles[2]);

                            //Opponent will block corner 4
                            blockcor3 = true;
                        }
                    }

                }










                /*if(!OppTookCenter && !CornerPlayed)
                {
                    //WIN
                    if (blockcor1 && (manager.tiles[0].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[0]);
                    }
                    else if (blockcor2 && (manager.tiles[2].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[2]);
                    }
                    else if (blockcor3 && (manager.tiles[6].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[6]);
                    }
                    else if (blockcor4 && (manager.tiles[8].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[8]);
                    }
                    else if(ITakeTopMid && (manager.tiles[7].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[7]);
                    }
                    else if (ITakeBotMid &&(manager.tiles[1].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[1]);
                    }
                    else if (ITakeMidLeft && (manager.tiles[5].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[5]);
                    }
                    else if (ITakeMidRight && (manager.tiles[3].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[3]);
                    }
                    //BLOCK
                    else if (blockcor1)
                    {
                        int x = manager.tiles[1].GetComponent<tileState>().state == 0 ? 2 :
                                manager.tiles[2].GetComponent<tileState>().state == 0 ? 1 :
                                manager.tiles[3].GetComponent<tileState>().state == 0 ? 6 : 3;

                        manager.mark(manager.tiles[x]);
                    }
                    else if (blockcor2)
                    {
                        int x = manager.tiles[1].GetComponent<tileState>().state == 0 ? 0 :
                                manager.tiles[0].GetComponent<tileState>().state == 0 ? 1 :
                                manager.tiles[8].GetComponent<tileState>().state == 0 ? 5 : 8;

                        manager.mark(manager.tiles[x]);
                    }
                    else if (blockcor3)
                    {
                        int x = manager.tiles[0].GetComponent<tileState>().state == 0 ? 3 :
                                manager.tiles[3].GetComponent<tileState>().state == 0 ? 0 :
                                manager.tiles[8].GetComponent<tileState>().state == 0 ? 7 : 8;

                        manager.mark(manager.tiles[x]);
                    }
                    else if (blockcor4)
                    {
                        int x = manager.tiles[5].GetComponent<tileState>().state == 0 ? 2 :
                                manager.tiles[2].GetComponent<tileState>().state == 0 ? 5 :
                                manager.tiles[6].GetComponent<tileState>().state == 0 ? 7 : 6;

                        manager.mark(manager.tiles[x]);
                    }
                    *//*else if (ITakeBotMid)
                    {
                        int x = manager.tiles[1].GetComponent<tileState>().state == 0 ? 0 :
                                manager.tiles[0].GetComponent<tileState>().state == 0 ? 1 :
                                manager.tiles[8].GetComponent<tileState>().state == 0 ? 5 : 8;

                        manager.mark(manager.tiles[x]);
                    }*//*
                    else if (ITakeMidLeft || ITakeMidRight)
                    {
                        manager.mark(manager.tiles[7]);
                        ITakeBotMid = true;
                    }
                    *//*else if (ITakeMidRight)
                    {
                        int x = manager.tiles[5].GetComponent<tileState>().state == 0 ? 2 :
                                manager.tiles[2].GetComponent<tileState>().state == 0 ? 5 :
                                manager.tiles[6].GetComponent<tileState>().state == 0 ? 7 : 6;

                        manager.mark(manager.tiles[x]);
                    }*//*
                    else if (ITakeTopMid || ITakeBotMid)
                    {
                        manager.mark(manager.tiles[5]);
                        ITakeMidRight = true;
                    }
                    if(markedtopLcor)
                    {
                        manager.mark(manager.tiles[0]);

                        //My win condition is corner 4 next turn
                    }
                    if (markedoppcor)
                    {
                        manager.mark(manager.tiles[2]);
                    }

                }
                else if(CornerPlayed)
                {
                    if (markedtopLcor)
                    {
                        manager.mark(manager.tiles[0]);

                        //My win condition is corner 4 next turn
                    }
                    if (markedoppcor)
                    {
                        manager.mark(manager.tiles[2]);
                    }
                }
                else if(OppTookCenter)
                {
                    if(ITakeTopMid && manager.tiles[1].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[1]);
                    }
                    else if (!ITakeTopMid || (manager.tiles[1].GetComponent<tileState>().state == 0))
                    {
                        ITakeTopMid = false;
                        manager.mark(manager.tiles[8]);
                        
                    }
                    if(ITakeMidLeft && manager.tiles[3].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[3]);
                    }
                    else if(!ITakeMidLeft || (manager.tiles[3].GetComponent<tileState>().state == 0))
                    {
                        ITakeMidLeft = false;
                        manager.mark(manager.tiles[5]);
                        forceDraw1 = true;
                    }*/



                //}
                
            }

            moves = 4;

        }
        else if(moves ==4)
        {
            if (myTurn == 1)//If I went first
            {
                
                //Opponent played center and blunders
                if (markedoppcor && markedbotRcor)
                {
                    if (manager.tiles[center+1].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center + 1]);
                    }
                    else if(manager.tiles[center + 3].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center + 3]);
                    }
                    else
                    {
                        manager.mark(manager.tiles[center]);
                    }
                }
                if (markedoppcor && markedtopLcor)
                {
                    if (manager.tiles[center - 1].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center - 1]);
                    }
                    else if (manager.tiles[center - 3].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center - 3]);
                    }
                    else
                    {
                        manager.mark(manager.tiles[center]);
                    }
                }
                if(markedbotRcor && markedtopLcor)
                {
                    if (manager.tiles[center - 1].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center - 1]);
                    }
                    else if (manager.tiles[center + 3].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[center + 3]);
                    }
                    else
                    {
                        manager.mark(manager.tiles[center]);
                    }
                }


                //Opponent played center and forces draw
                if(ITakeTopMid)
                {
                    if(manager.tiles[0].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[0]);
                    }
                    else
                    {
                        //Block corner 8
                        manager.mark(manager.tiles[8]);
                        blockcor4 = true;
                    }
                }
                else if(ITakeBotMid)
                {
                    if(manager.tiles[8].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[8]);
                    }
                    else
                    {
                        manager.mark(manager.tiles[0]);
                        blockcor1 = true;
                    }
                }
                else if(ITakeMidRight)
                {
                    if (manager.tiles[8].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[8]);
                    }
                    else
                    {
                        //Block corner 8
                        manager.mark(manager.tiles[0]);
                        blockcor4 = true;
                    }
                }
                else if(ITakeMidLeft)
                {
                    if (manager.tiles[0].GetComponent<tileState>().state == 2)
                    {
                        manager.mark(manager.tiles[0]);
                    }
                    else
                    {
                        manager.mark(manager.tiles[8]);
                        blockcor1 = true;
                    }
                }


            }
            else //If I went second,
            {
                if(OppTookCenter)
                {
                    if(tile1mine)
                    {
                        if(manager.tiles[1].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[1]);
                        }
                        else if(trapSet)
                        {
                            manager.mark(manager.tiles[8]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[7]);
                        }
                    }
                    else if (tile2mine)
                    {
                        if (manager.tiles[2].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[2]);
                        }
                        else 
                        {
                            manager.mark(manager.tiles[6]);
                        }
                    }
                    else if (tile3mine)
                    {
                        if (manager.tiles[3].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[3]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[5]);
                        }
                    }
                    else if (tile5mine)
                    {
                        if (manager.tiles[5].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[5]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[3]);
                        }
                    }
                    else if (tile6mine)
                    {
                        if (manager.tiles[6].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[6]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[2]);
                        }
                    }
                    else if (tile8mine)
                    {
                        if (manager.tiles[8].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[8]);
                        }
                        else if (trapSet)
                        {
                            manager.mark(manager.tiles[3]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[5]);
                        }
                    }

                }
                else if(RisingCornerPlayed || FallingCornerPlayed)
                {
                    if(blockcor3)
                    {
                        if(manager.tiles[6].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[6]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[3]);
                        }
                    }
                    if(blockcor4)
                    {
                        if (manager.tiles[8].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[8]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[5]);
                        }
                    }
                }
                else
                {
                    if(forceDraw1)
                    {
                        Lvl1();
                    }    
                    else if(adjacent)
                    {
                        if(tile0mine && manager.tiles[0].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[0]);
                        }
                        else if(tile0mine && manager.tiles[0].GetComponent<tileState>().state != 2)
                        {
                            if(blockcor3)
                            {
                                manager.mark(manager.tiles[3]);
                            }
                            else
                            {
                                manager.mark(manager.tiles[1]);
                            }
                            
                        }
                        else if (tile2mine && manager.tiles[2].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[2]);
                        }
                        else if (tile2mine && manager.tiles[2].GetComponent<tileState>().state != 2)
                        {
                            if (blockcor4)
                            {
                                manager.mark(manager.tiles[5]);
                            }
                            else
                            {
                                manager.mark(manager.tiles[1]);
                            }

                        }

                    }
                    else if (ITakeBotMid && (manager.tiles[7].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[7]);
                    }
                    else if (ITakeBotMid && (manager.tiles[5].GetComponent<tileState>().state == 2))
                    {
                        manager.mark(manager.tiles[5]);
                    }
                    else if (ITakeBotMid&&(manager.tiles[7].GetComponent<tileState>().state ==0))
                    {
                        int x = manager.tiles[8].GetComponent<tileState>().state == 2 ? 8 : 6;

                        manager.mark(manager.tiles[x]);
                    }
                    else if(blockcor2)
                    {
                        if (manager.tiles[5].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[5]);
                        }
                        else if (manager.tiles[1].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[1]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[2]);
                        }
                    }
                }


                /*if(!OppTookCenter)
                {
                    if(markedtopLcor)
                    {
                        if((manager.tiles[8].GetComponent<tileState>().state == 2))
                        {
                            manager.mark(manager.tiles[8]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[5]);
                        }
                    }
                    else if (markedoppcor)
                    {
                        if ((manager.tiles[6].GetComponent<tileState>().state == 2))
                        {
                            manager.mark(manager.tiles[6]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[3]);
                        }
                    }
                    else if(ITakeTopMid && ITakeMidRight)
                    {
                        manager.mark(manager.tiles[6]);
                    }
                    else if(ITakeBotMid && ITakeMidRight)
                    {
                        manager.mark(manager.tiles[0]);
                    }
                    else if (ITakeBotMid && ITakeMidLeft)
                    {
                        manager.mark(manager.tiles[2]);
                    }
                }
                else if(OppTookCenter)
                {
                    if(forceDraw1)
                    {
                        if(manager.tiles[1].GetComponent<tileState>().state == 2)
                        {
                            manager.mark(manager.tiles[1]);
                        }
                        else
                        {
                            manager.mark(manager.tiles[7]);
                        }
                    }
                    
                        
                }*/
            }

            moves = 5;


        }
        else if(moves ==5)
        {
            if(myTurn == 1)
            {
                Lvl1();
            }
            /*if(blockcor1||blockcor4) 
            {
                Lvl1();
            }*/
        }
    }

    void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
