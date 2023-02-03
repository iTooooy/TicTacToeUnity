using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class Player
{
    public Image panel;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public GameObject MainUI;
    public Text[] buttonList;
    public Text tipsText;
    
    private string playerSide;
    private bool isPVE;

    public GameObject gameOverPanel;
    public GameObject restartButton;
    public Text gameOverText;
    public Player playerX;
    public Player playerO;
    public PlayerColor activeColor;
    public PlayerColor inactiveColor;


    private int moveCount;

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    void Awake()
    {
        moveCount = 0;
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerColors(playerX, playerO);
    }

    public void StartPVE()
    {
        isPVE = true;
        MainUI.SetActive(false);
        tipsText.gameObject.SetActive(true);
        float rand = Random.Range(-1f,1f);
        Debug.Log("随机数：：：：：：：" + rand);
        if (rand <= 0)
        {
            playerSide = "COCA";
            Debug.Log("玩家可口可乐先行");
            tipsText.text = "You are on the offensive";
        }
        else
        {
            playerSide = "PEPSI";
            Debug.Log("Ai百事可乐先行");
            tipsText.text = "AI is on the offensive";
            AiAction();
        }
    }

    public void StartPVP()
    {
        tipsText.gameObject.SetActive(false);
        isPVE = false;
        MainUI.SetActive(false);
        playerSide = "COCA";
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackMainUI()
    {
        MainUI.SetActive(true);
        isPVE = false;
        RestartGame();
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
            ChangeSide();
    }

    void ChangeSide()
    {
        playerSide = (playerSide == "COCA") ? "PEPSI" : "COCA";
        if (playerSide == "COCA")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            if (isPVE)
            {
                AiAction();
            }
        }
    }

    void GameOver(string winningPlayer)
    {
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
        }
        else
        {
            if (winningPlayer == "COCA")
            {
                SetGameOverText( "Coca Cola Wins!");
            }
            else
            {
                SetGameOverText( "Pepsi Cola Wins!");
            }
            
        }
        SetBoardInteractable(false);
        restartButton.SetActive(true);
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        float rand = Random.Range(-1f,1f);
        if (isPVE)
        {
            tipsText.gameObject.SetActive(true);
            Debug.Log("随机数：：：：：：：" + rand);
            if (rand <= 0)
            {
                playerSide = "COCA";
                Debug.Log("玩家可口可乐先行");
                tipsText.text = "You are on the offensive";
            }
            else
            {
                playerSide = "PEPSI";
                Debug.Log("Ai百事可乐先行");
                tipsText.text = "AI is on the offensive";
                AiAction();
            }
        }
        else
        {
            tipsText.gameObject.SetActive(false);
            playerSide = "COCA";
        }
        
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetBoardInteractable(true);
        foreach (var button in buttonList)
        {
            button.text = "";
        }
        SetPlayerColors(playerX, playerO);
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i <buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activeColor.panelColor;
        //newPlayer.text.color = activeColor.textColor;
        oldPlayer.panel.color = inactiveColor.panelColor;
        //oldPlayer.text.color = inactiveColor.textColor;
    }

    async void AiAction()
    {
        foreach (var button in buttonList)
        {
            button.GetComponentInParent<GridSpace>().button.interactable = false;
        }
        await Task.Delay(TimeSpan.FromSeconds(1f));
        
        foreach (var button in buttonList)
        {
            var grid = button.GetComponentInParent<GridSpace>();
            if (string.IsNullOrEmpty(button.text))
            {
                grid.button.interactable = true;
            }
        }
        // 1. 如果下在该位置可以赢棋，那么下在该位置
        // 2. 如果对手下在该位置可以赢棋，那下在该位置
        if (CheckAiAndPlayerWin()) return;
        // 3. 如果中心位置空闲，那么下在中心位置要优于边上和角上位置
        if (string.IsNullOrEmpty(buttonList[4].text))
        {
            buttonList[4].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("中间格子空着");
            return;
        }
        // 4. 如果角上位置空闲，那么下在角上位置要优于边上位置
        if (string.IsNullOrEmpty(buttonList[0].text))
        {
            buttonList[0].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("角上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[2].text))
        {
            buttonList[2].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("角上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[6].text))
        {
            buttonList[6].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("角上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[8].text))
        {
            buttonList[8].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("角上格子空着");
            return;
        }
        // 5. 如果只有边上位置空闲，那么只能下在边上位置
        if (string.IsNullOrEmpty(buttonList[1].text))
        {
            buttonList[1].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("边上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[3].text))
        {
            buttonList[3].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("边上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[5].text))
        {
            buttonList[5].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("边上格子空着");
            return;
        }
        if (string.IsNullOrEmpty(buttonList[7].text))
        {
            buttonList[7].GetComponentInParent<GridSpace>().SetSpace();
            Debug.Log("边上格子空着");
        }
    }

    private bool CheckAiAndPlayerWin()
    {
        Text canWinGrid = null;
        Text canLoseGrid = null;
       
        int winCount = 0;
        int loseCount = 0;
        for (int i = 0; i < 3; i++) 
        {
            //检查行
            winCount = 0;
            loseCount = 0;
            switch (buttonList[3 * i].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i];
                    canLoseGrid = buttonList[3 * i];
                    break;
            }
            switch (buttonList[3 * i + 1].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i + 1];
                    canLoseGrid = buttonList[3 * i + 1];
                    break;
            }
            switch (buttonList[3 * i + 2].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[3 * i + 2];
                    canLoseGrid = buttonList[3 * i + 2];
                    break;
            }

            if (winCount + loseCount < 3)
            {
                if (winCount == 2)
                {
                    if (canWinGrid != null)
                    {
                        canWinGrid.GetComponentInParent<GridSpace>().SetSpace(); 
                        Debug.Log("Ai能在" +(i+1) + "行赢");
                    }
                    return true;
                }
                if (loseCount == 2)
                {
                    if (canLoseGrid != null)
                    {
                        canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();   
                        Debug.Log("Player能在" + (i+1) + "行赢");
                    }
                    return true;
                }
            }
            
            //检查列
            winCount = 0;
            loseCount = 0;
            switch (buttonList[i].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i];
                    canLoseGrid = buttonList[i];
                    break;
            }
            switch (buttonList[i + 3].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i + 3];
                    canLoseGrid = buttonList[i + 3];
                    break; 
            }
            switch (buttonList[i + 6].text)
            {
                case "COCA":
                    loseCount++;
                    break;
                case "PEPSI":
                    winCount++;
                    break;
                default:
                    canWinGrid = buttonList[i + 6];
                    canLoseGrid = buttonList[i + 6];
                    break;
            }

            if (winCount + loseCount < 3)
            {
                if (winCount == 2)
                {
                    if (canWinGrid != null)
                    {
                        canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                        Debug.Log("Ai能在" + (i + 1) + "列赢");
                    }

                    return true;
                }

                if (loseCount == 2)
                {
                    if (canLoseGrid != null)
                    {
                        canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                        Debug.Log("Player能在" + (i + 1) + "列赢");
                    }

                    return true;
                }
            }
        }
        
        //检查对角线1
        winCount = 0;
        loseCount = 0;
        switch (buttonList[0].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[0];
                canLoseGrid = buttonList[0];
                break;
        }
        switch (buttonList[4].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[4];
                canLoseGrid = buttonList[4]; 
                break;
        }
        switch (buttonList[8].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[8];
                canLoseGrid = buttonList[8];
                break;
        }

        if (winCount + loseCount < 3)
        {
            if (winCount == 2)
            {
                if (canWinGrid != null)
                {
                    canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Ai能在对角线1赢");
                }

                return true;
            }

            if (loseCount == 2)
            {
                if (canLoseGrid != null)
                {
                    canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Player能在对角线1赢");
                }

                return true;
            }
        }

        //检查对角线2
        winCount = 0;
        loseCount = 0;
        switch (buttonList[2].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[2];
                canLoseGrid = buttonList[2];
                break;
        }
        switch (buttonList[4].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[4];
                canLoseGrid = buttonList[4];
                break;
        }
        switch (buttonList[6].text)
        {
            case "COCA":
                loseCount++;
                break;
            case "PEPSI":
                winCount++;
                break;
            default:
                canWinGrid = buttonList[6];
                canLoseGrid = buttonList[6];
                break;
        }

        if (winCount + loseCount < 3)
        {
            if (winCount == 2)
            {
                if (canWinGrid != null)
                {
                    canWinGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Ai能在对角线2赢");
                }

                return true;
            }

            if (loseCount == 2)
            {
                if (canLoseGrid != null)
                {
                    canLoseGrid.GetComponentInParent<GridSpace>().SetSpace();
                    Debug.Log("Player能在对角线2赢");
                }

                return true;
            }
        }

        return false;
    }
}

