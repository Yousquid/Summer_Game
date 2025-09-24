using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Punch_Me_Game_Manager : MonoBehaviour
{
    public int player_one_health;
    public int player_two_health;
    public enum PlayStage { rolldice, prepare_punch, punch, gain_push_compensation, state_purchase};
    public PlayStage currentPlayStage;

    public int player_one_hate_token;
    public int player_two_hate_token;

    public int isPlayerOnePunched;
    public int isPlayerTwoPunched;

    public int player_one_dice_one;
    public int player_one_dice_two;

    public int player_two_dice_one;
    public int player_two_dice_two;

    public TextMeshProUGUI playerOneDice;
    public TextMeshProUGUI playerTwoDice;

    public TextMeshProUGUI gameplayIllustration;
    public TextMeshProUGUI buttonText;

    private bool hasPlayerOneRolled = false;
    private bool hasPlayerTwoRolled = false;


    void Start()
    {
        currentPlayStage = PlayStage.rolldice;
        player_one_health = 15;
        player_two_health = 15;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerDice();
        DiceCompare();
    }

    public void RollP1Dice()
    {
        player_one_dice_one = Random.Range(1, 6);
        player_one_dice_two = Random.Range(1, 6);
        hasPlayerOneRolled = true;
    }

    public void RollP2Dice()
    {
        player_two_dice_one = Random.Range(1, 6);
        player_two_dice_two = Random.Range(1, 6);
        hasPlayerTwoRolled = true;
    }

    public void ChangeStage()
    {
        if (currentPlayStage == PlayStage.rolldice)
        {
            currentPlayStage = PlayStage.prepare_punch;
        }
        if (currentPlayStage == PlayStage.prepare_punch)
        {
            currentPlayStage = PlayStage.punch;
        }
        if (currentPlayStage == PlayStage.gain_push_compensation)
        {
            currentPlayStage = PlayStage.state_purchase;
        }
        if (currentPlayStage == PlayStage.state_purchase)
        {
            currentPlayStage = PlayStage.rolldice;
        }
        
    }

    private void DiceCompare()
    {
        if (hasPlayerOneRolled && hasPlayerTwoRolled)
        {
            if (player_one_dice_one + player_one_dice_two > player_two_dice_one + player_two_dice_two)
            {
                if (player_one_dice_one == player_one_dice_two)
                {
                    gameplayIllustration.text = "P1 wins this round, you can punch twice because you rolled two same number!";
                    buttonText.text = "OK";
                }
                else
                {
                    gameplayIllustration.text = "P1 wins this round, you can punch once!";
                    buttonText.text = "OK";
                }
            }
            else if (player_one_dice_one + player_one_dice_two < player_two_dice_one + player_two_dice_two)
            {
                if (player_two_dice_one == player_two_dice_two)
                {
                    gameplayIllustration.text = "P2 wins this round, you can punch twice because you rolled two same number!";
                    buttonText.text = "OK";
                }
                else
                {
                    gameplayIllustration.text = "P2 wins this round, you can punch once!";
                    buttonText.text = "OK";
                }
            }
            else if (player_one_dice_one + player_one_dice_two == player_two_dice_one + player_two_dice_two)
            {
                gameplayIllustration.text = "A draw! No one can punch this round!";
                buttonText.text = "OK";
            }
        }
       
    }
    private void UpdatePlayerDice()
    {
        playerOneDice.text = $"{player_one_dice_one} {player_one_dice_two}";
        playerTwoDice.text = $"{player_two_dice_one} {player_two_dice_two}";

    }
}
