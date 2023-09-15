using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("AI Settings")]
    public bool AIControlled = false;
    public enum levels { lvl1 = 1, lvl2 = 2, lvl3 = 3};
    public levels LevelDifficulty = levels.lvl1;
    public int levelNum = 1;
    public bool HumanIsPlayer2 = false;

    [Header("Game Settings")]
    public short gameState = 0; //0 = Main Menu, 1 = Player1 Turn, 2 = Player2 Turn, 3 = P1 Win, 4 = P2 Win, 5 = DRRAW
    public GameObject AI;
    public GameObject[] tiles;

    [Header("UI Settings")]
    public GameObject[] Panels;

    private bool drawable = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<Text>().text = " ";
        }
    }


    // Update is called once per frame
    void Update()
    {
        //checkWinner();
        
    }



    public void mark(GameObject tile)
    {
        
        if(gameState==1)
        {
            if(tile.GetComponent<Text>().text== " ")
            {
                tile.GetComponent<Text>().text = "O";
                tile.GetComponent<tileState>().state = 0;
                gameState = 2;
            }
            
        }
        else if(gameState==2)
        {
            if (tile.GetComponent<Text>().text == " ")
            {
                tile.GetComponent<Text>().text = "X";
                tile.GetComponent<tileState>().state = 1;
                gameState = 1;
            }
        }


        checkWinner();
        
    }

    void checkWinner()
    {
        topWin();
        botWin();
        centerHorizontalWin ();
        centerVerticalWin ();
        leftWin();
        rightWin();
        diagonalWin1 ();
        diagonalWin2 ();

        if (drawable)
        {
            checkDraw();
        }
    }


    void topWin()
    {
        if ((tiles[0].GetComponent<tileState>().state == 0)&& (tiles[1].GetComponent<tileState>().state == 0)&& (tiles[2].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[0].GetComponent<tileState>().state == 1) && (tiles[1].GetComponent<tileState>().state ==1) && (tiles[2].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }
    void centerHorizontalWin()
    {
        if ((tiles[3].GetComponent<tileState>().state == 0) && (tiles[4].GetComponent<tileState>().state == 0) && (tiles[5].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[3].GetComponent<tileState>().state == 1) && (tiles[4].GetComponent<tileState>().state == 1) && (tiles[5].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }
    void botWin()
    {
        if ((tiles[6].GetComponent<tileState>().state == 0) && (tiles[7].GetComponent<tileState>().state == 0) && (tiles[8].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[6].GetComponent<tileState>().state == 1) && (tiles[7].GetComponent<tileState>().state == 1) && (tiles[8].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }

    void leftWin()
    {
        if ((tiles[0].GetComponent<tileState>().state == 0) && (tiles[3].GetComponent<tileState>().state == 0) && (tiles[6].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[0].GetComponent<tileState>().state == 1) && (tiles[3].GetComponent<tileState>().state == 1) && (tiles[6].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }
    void centerVerticalWin()
    {
        if ((tiles[1].GetComponent<tileState>().state == 0) && (tiles[4].GetComponent<tileState>().state == 0) && (tiles[7].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[1].GetComponent<tileState>().state == 1) && (tiles[4].GetComponent<tileState>().state == 1) && (tiles[7].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }
    void rightWin()
    {
        if ((tiles[2].GetComponent<tileState>().state == 0) && (tiles[5].GetComponent<tileState>().state == 0) && (tiles[8].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[2].GetComponent<tileState>().state == 1) && (tiles[5].GetComponent<tileState>().state == 1) && (tiles[8].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }

    void diagonalWin1()
    {
        if ((tiles[0].GetComponent<tileState>().state == 0) && (tiles[4].GetComponent<tileState>().state == 0) && (tiles[8].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[0].GetComponent<tileState>().state == 1) && (tiles[4].GetComponent<tileState>().state == 1) && (tiles[8].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }
    void diagonalWin2()
    {
        if ((tiles[2].GetComponent<tileState>().state == 0) && (tiles[4].GetComponent<tileState>().state == 0) && (tiles[6].GetComponent<tileState>().state == 0))
        {
            P1Win();
        }
        else if ((tiles[2].GetComponent<tileState>().state == 1) && (tiles[4].GetComponent<tileState>().state == 1) && (tiles[6].GetComponent<tileState>().state == 1))
        {
            P2Win();
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void showAIOptions(Toggle t)
    {
        if(t.isOn)
        {
            AIControlled = true;

            Panels[1].SetActive(true);
        }
        else
        {
            AIControlled = false;

            Panels[1].SetActive(false);
        }
    }

    public void AIgoesFirst(Toggle t)
    {
        if (t.isOn)
        {
            HumanIsPlayer2 = true;
        }
        else
        {
            HumanIsPlayer2 = false;
        }
    }

    public void getAILevel(Dropdown d)
    {
        switch (d.value)
        {
            case 0:
                LevelDifficulty = levels.lvl1;
                levelNum = 1;
                break;

            case 1:
                LevelDifficulty = levels.lvl2;
                levelNum = 2;
                break;

            case 2:
                LevelDifficulty = levels.lvl3;
                levelNum = 3;
                break;
        }
    }

    public void startGame()
    {

        Panels[0].SetActive(false);
        Panels[2].SetActive(true);
        
        gameState = 1;
        AI.SetActive(true);
    }

    public void P1Win()
    {
        gameState = 3;
        AIControlled = false;
        Panels[3].SetActive(true);
        drawable = false;
        Time.timeScale = 0;
    }

    public void P2Win()
    {
        gameState = 4;
        AIControlled = false;
        Panels[4].SetActive(true) ;
        drawable = false;
        Time.timeScale = 0;
    }

    public void Draw()
    {
        gameState = 5;
        AIControlled = false;
        Panels[5].SetActive(true) ;

        Time.timeScale = 0;
    }

    public void checkDraw()
    {
        for (int num = 0; num < tiles.Length; num++)
        {
            if (tiles[num].GetComponent<tileState>().state == 2)
            {
                //Debug.Log(num);
                break;
            }
            else if(num==tiles.Length-1)
            {
                Draw();
                break;
            }
        }
    }

}
