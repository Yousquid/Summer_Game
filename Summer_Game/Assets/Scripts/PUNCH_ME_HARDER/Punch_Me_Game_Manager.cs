using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Punch_Me_Game_Manager : MonoBehaviour
{
    public int player_one_health;
    public int player_two_health;
    public enum PlayStage { rolldice, prepare_punch, punch, gain_punch_compensation, state_purchase, gain_state_income};
    public PlayStage currentPlayStage;

    public int player_one_hate_token;
    public int player_two_hate_token;

    public int p1_money;
    public int p2_money;

    public bool isPlayerOnePunching;
    public bool isPlayerTwoPunching;

    public bool canStartPunch = false;

    public static int leftPuchTimes;

    public GameObject punchObject;

    public int player_one_dice_one;
    public int player_one_dice_two;

    public int player_two_dice_one;
    public int player_two_dice_two;

    public TextMeshProUGUI playerOneDice;
    public TextMeshProUGUI playerTwoDice;

    public TextMeshProUGUI gameplayIllustration;
    public TextMeshProUGUI buttonText;
    public Button confirmButton;
    public GameObject textBar;

    public TextMeshProUGUI playerOneHealthText;
    public TextMeshProUGUI playerTwoHealthText;

    public GameObject arm;

    private bool hasPlayerOneRolled = false;
    private bool hasPlayerTwoRolled = false;

    [Header("HateTokenInquiry")]
    public GameObject inquiryBar;
    public TextMeshProUGUI inquiryText;
    private int usedToken = 0;

    public int damage = 0;


    void Start()
    {
        currentPlayStage = PlayStage.rolldice;
        player_one_health = 15;
        player_two_health = 15;
        arm.SetActive(false);
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

    

    private void DiceCompare()
    {
        if (hasPlayerOneRolled && hasPlayerTwoRolled && currentPlayStage == PlayStage.rolldice)
        {
            if (player_one_dice_one + player_one_dice_two > player_two_dice_one + player_two_dice_two)
            {
                if (player_one_dice_one == player_one_dice_two)
                {
                    gameplayIllustration.text = "P1 wins this round, you can punch twice because you rolled two same number!";
                    leftPuchTimes = 2;
                    buttonText.text = "OK";
                    isPlayerOnePunching = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);

                }
                else
                {
                    gameplayIllustration.text = "P1 wins this round, you can punch once!";
                    leftPuchTimes = 1;
                    buttonText.text = "OK";
                    isPlayerOnePunching = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);
                }
            }
            else if (player_one_dice_one + player_one_dice_two < player_two_dice_one + player_two_dice_two)
            {
                if (player_two_dice_one == player_two_dice_two)
                {
                    gameplayIllustration.text = "P2 wins this round, you can punch twice because you rolled two same number!";
                    leftPuchTimes = 2;
                    buttonText.text = "OK";
                    isPlayerTwoPunching = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);
                }
                else
                {
                    gameplayIllustration.text = "P2 wins this round, you can punch once!";
                    leftPuchTimes = 1;
                    buttonText.text = "OK";
                    isPlayerTwoPunching = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);
                }
            }
            // SPECIAL HERE, NEED TO DIRECT TO THE STATE_PURCHASE STAGE HERE.
            else if (player_one_dice_one + player_one_dice_two == player_two_dice_one + player_two_dice_two)
            {
                gameplayIllustration.text = "A draw! No one can punch this round! But you are all rewarded with one hate token because the temperary peace makes you want to puch each other more!";
                buttonText.text = "OK";
                leftPuchTimes = 0;
                isPlayerTwoPunching = false;
                isPlayerOnePunching = false;
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(ChangePunchStage);
            }
        }
       
    }

    private void ChangePunchStage()
    {
        currentPlayStage = PlayStage.prepare_punch;

        confirmButton.gameObject.SetActive(false);
        if (isPlayerOnePunching)
        {
            if (player_one_dice_one == player_one_dice_two)
            {
                gameplayIllustration.text = $"P1, Pick what you want to punch! You can punch {leftPuchTimes} times!";
            }
            else
            {
                gameplayIllustration.text = $"P1, Pick what you want to punch! You can punch {leftPuchTimes} time!";
            }
        }
        else if (isPlayerTwoPunching)
        {
            if (player_two_dice_one == player_two_dice_two)
            {
                gameplayIllustration.text = $"P2, Pick what you want to punch! You can punch {leftPuchTimes} times!";
            }
            else
            {
                gameplayIllustration.text = $"P2, Pick what you want to punch! You can punch {leftPuchTimes} time!";
            }
        }
    }

    public void OnClickAddHateToken()
    {
        if (isPlayerOnePunching && player_one_hate_token > usedToken)
        {
            usedToken += 1;
            player_one_hate_token--;
        }
        else if (isPlayerTwoPunching && player_two_hate_token > usedToken)
        {
            usedToken += 1;
            player_two_hate_token--;
        }
    }

    public void OnClickDecreaseHateToken()
    {
        if (isPlayerOnePunching && player_one_hate_token > usedToken)
        {
            usedToken += -1;
            player_one_hate_token++;
        }
        else if (isPlayerTwoPunching && player_two_hate_token > usedToken)
        {
            usedToken += -1;
            player_two_hate_token++;
        }
    }

    public void OnClickConfirmPunch()
    {
        currentPlayStage = PlayStage.punch;
        inquiryBar.SetActive(false);
        damage = 1 + usedToken;
        usedToken = 0;
        StartPunch();
    }
    public void InquryPunch()
    {
        inquiryBar.SetActive(true);
        inquiryText.text = $"You are using {usedToken} hate tokens, you can make {1 + usedToken} damages. Do you want to add more?";
    }

    private void StartPunch()
    {
        
            
        Vector3 pos = punchObject.transform.position;
        pos.y -= 6f;
        arm.GetComponent<Arm_Controller>().initialPos = pos;
        arm.SetActive(true);
        punchObject.GetComponent<Collider2D>().isTrigger = true;
        
    }

    public void CheckEnterGainPunchCompensation()
    {
        currentPlayStage = PlayStage.gain_punch_compensation;

        if (isPlayerOnePunching)
        {
            gameplayIllustration.text = $"P2, you gain {damage} hate tokens for P1's puch, use it to punch P1 harder!";
            player_two_hate_token += damage;
            damage = 0;
            CheckIfCanGoToStatePurchase();
        }
        else if (isPlayerTwoPunching)
        {
            gameplayIllustration.text = $"P1, you gain {damage} hate tokens for P2's puch, use it to punch P2 harder!";
            player_one_hate_token += damage;
            damage = 0;
            CheckIfCanGoToStatePurchase();
        }
    }

    private void CheckIfCanGoToStatePurchase()
    {
        if (leftPuchTimes >= 1)
        {
            ChangePunchStage();
        }
        else if (leftPuchTimes <= 0)
        {
            currentPlayStage = PlayStage.state_purchase;
        }
    }
    private void UpdatePlayerDice()
    {
        playerOneDice.text = $"{player_one_dice_one} {player_one_dice_two}";
        playerTwoDice.text = $"{player_two_dice_one} {player_two_dice_two}";

    }
}
