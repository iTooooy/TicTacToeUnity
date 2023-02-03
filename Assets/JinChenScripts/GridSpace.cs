using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public Sprite CocaSprite;
    public Sprite PipsiSprite;
    public Image Cola;
    private GameController gameController;

    public void SetGameControllerReference(GameController controller)
    {
        this.gameController = controller;
    }

    public void SetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        gameController.EndTurn();
    }

    private void Update()
    {
        if (buttonText.text == "COCA")
        {
            Cola.sprite = CocaSprite;
            button.interactable = false;
        }
        else if (buttonText.text == "PEPSI")
        {
            Cola.sprite = PipsiSprite;
            button.interactable = false;
        } 
        else if (buttonText.text == "")
        {
            Cola.sprite = null;
        }
    }
}
