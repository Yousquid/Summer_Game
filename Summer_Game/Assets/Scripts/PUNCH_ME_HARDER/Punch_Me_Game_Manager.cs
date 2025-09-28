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

    private bool isP1FinishPurchase = false;
    private bool isP2FinishPurchase = false;

    public List<Upgrade> p1EstateList;
    public List<Upgrade> p2EstateList;


    public static int leftPuchTimes;

    public GameObject punchObject;

    public int player_one_dice_one;
    public int player_one_dice_two;

    public int player_two_dice_one;
    public int player_two_dice_two;

    public GameObject player_one;
    public GameObject player_two;

    public TextMeshProUGUI playerOneDice;
    public TextMeshProUGUI playerTwoDice;
    public TextMeshProUGUI p1HateTokenText;
    public TextMeshProUGUI p2HateTokenText;
    public TextMeshProUGUI p1CoinText;
    public TextMeshProUGUI p2CoinText;
    public TextMeshProUGUI p1HealthText;
    public TextMeshProUGUI p2HealthText;

    public TextMeshProUGUI gameplayIllustration;
    public TextMeshProUGUI buttonText;
    public Button confirmButton;
    public GameObject textBar;

    public TextMeshProUGUI playerOneHealthText;
    public TextMeshProUGUI playerTwoHealthText;

    public GameObject arm;

    private bool hasPlayerOneRolled = false;
    private bool hasPlayerTwoRolled = false;

    public GameObject pickCoinButton;
    public GameObject pickHateButton;

    public GameObject p1FinishPurchaseButton;
    public GameObject p2FinishPurchaseButton;

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
        pickCoinButton.SetActive(false);
        pickHateButton.SetActive(false);
        gameplayIllustration.text = "Roll your dices, the winner can PUNCH the loser!!";
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerDice();
        DiceCompare();
        UpdateTexts();
        CheckIfGoToGainIncome();
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

    void UpdateTexts()
    {
        
        p1CoinText.text = $"Coins: {p1_money}";
        p1HateTokenText.text = $"Hate Tokens: {player_one_hate_token}";
        p2CoinText.text = $"Coins: {p2_money}";
        p2HateTokenText.text = $"Hate Tokens: {player_two_hate_token}";
        p1HealthText.text = $"HP: {player_one.GetComponent<PunchableObject>().health}";
        p2HealthText.text = $"HP: {player_two.GetComponent<PunchableObject>().health}";
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
                gameplayIllustration.text = $"P1, Pick what you want to punch (You can punch P2's FACE OR estate)! You can punch {leftPuchTimes} times!";
            }
            else
            {
                gameplayIllustration.text = $"P1, Pick what you want to punch (You can punch P2's FACE OR estate)! You can punch {leftPuchTimes} time!";
            }
        }
        else if (isPlayerTwoPunching)
        {
            if (player_two_dice_one == player_two_dice_two)
            {
                gameplayIllustration.text = $"P2, Pick what you want to punch (You can punch P1's FACE OR estate)! You can punch {leftPuchTimes} times!";
            }
            else
            {
                gameplayIllustration.text = $"P2, Pick what you want to punch (You can punch P1's FACE OR estate)! You can punch {leftPuchTimes} time!";
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
        gameplayIllustration.text = $"You are using {usedToken} hate tokens to increase your damage, you can make {1 + usedToken} damages.";
    }

    private void StartPunch()
    {
        
            
        Vector3 pos = punchObject.transform.position;
        pos.y -= 6f;
        arm.GetComponent<Arm_Controller>().initialPos = pos;
        arm.SetActive(true);
        textBar.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        punchObject.GetComponent<Collider2D>().isTrigger = true;
        
    }

    public void CheckEnterGainPunchCompensation()
    {
        currentPlayStage = PlayStage.gain_punch_compensation;

        if (isPlayerOnePunching)
        {
            arm.SetActive(false);
            textBar.SetActive(true);
            Cursor.visible = true;
            gameplayIllustration.text = $"P1, you gain {damage} coins for your punch!" +
               $"P2, you can choose {damage} hate tokens for your increasing resentment or coins as your compensation, which do you want?";
            p1_money += damage;
            punchObject = null;
            pickCoinButton.SetActive(true);
            pickHateButton.SetActive(true);

        }
        else if (isPlayerTwoPunching)
        {
            arm.SetActive(false);
            textBar.SetActive(true);
            Cursor.visible = true;
            gameplayIllustration.text = $"P2, you gain {damage} coins for your punch!" +
                $"P1, you can choose {damage} hate tokens for your increasing resentment or coins as your compensation, which do you want?";
            p2_money += damage;
            punchObject = null;
            pickCoinButton.SetActive(true);
            pickHateButton.SetActive(true);
        }
    }

    public void OnClickAddCoin()
    {
        pickCoinButton.SetActive(false);
        pickHateButton.SetActive(false);
        if (isPlayerOnePunching)
        {
            p2_money += damage;
            CheckIfCanGoToStatePurchase();
        }
        else if (isPlayerTwoPunching)
        {
            p1_money += damage;
            CheckIfCanGoToStatePurchase();
        }
    }

    public void OnClickAddHate()
    {
        pickCoinButton.SetActive(false);
        pickHateButton.SetActive(false);
        if (isPlayerOnePunching)
        {
            player_two_hate_token += damage;
            CheckIfCanGoToStatePurchase();
        }
        else if (isPlayerTwoPunching)
        {
            player_one_hate_token += damage;
            CheckIfCanGoToStatePurchase();
        }
    }

    private void CheckIfCanGoToStatePurchase()
    {
        if (leftPuchTimes >= 1)
        {
            ChangePunchStage();
            damage = 0;
        }
        else if (leftPuchTimes <= 0)
        {
            damage = 0;
            currentPlayStage = PlayStage.state_purchase;
            gameplayIllustration.text = "Now is the purchase stage, click on the estate to spend 1 coin to buy or upgrade one estate. The estates can give you reward each turn!";
            p1FinishPurchaseButton.SetActive(true);
            p2FinishPurchaseButton.SetActive(true);
        }
    }

    public void OnClickFinishP1Purchase()
    {
        isP1FinishPurchase = true;
        p1FinishPurchaseButton.SetActive(false);
    }
    public void OnClickFinishP2Purchase()
    {
        isP2FinishPurchase = true;
        p2FinishPurchaseButton.SetActive(false);
    }

    private void CheckIfGoToGainIncome()
    {
        if (currentPlayStage == PlayStage.state_purchase && isP2FinishPurchase && isP1FinishPurchase)
        {
            currentPlayStage = PlayStage.gain_state_income;
            isP1FinishPurchase = false;
            isP2FinishPurchase = false;
        }
    }

    private void CheckIfGoToRollDice()
    { 
        
    }
    private void UpdatePlayerDice()
    {
        playerOneDice.text = $"Dices: {player_one_dice_one} & {player_one_dice_two}";
        playerTwoDice.text = $"Dices: {player_two_dice_one} & {player_two_dice_two}";

    }
}
