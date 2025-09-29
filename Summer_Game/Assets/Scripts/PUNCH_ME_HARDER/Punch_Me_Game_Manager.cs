using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class Punch_Me_Game_Manager : MonoBehaviour
{
    public int player_one_health;
    public int player_two_health;
    public enum PlayStage { rolldice, prepare_punch, punch, gain_punch_compensation, state_purchase, gain_state_income};
    public PlayStage currentPlayStage;

    public int player_one_hate_token;
    public int player_two_hate_token;

    public bool isInquiryHate = false;

    public int p1_money;
    public int p2_money;

    private bool hasAddedHateToken = false;

    public bool isPlayerOnePunching;
    public bool isPlayerTwoPunching;

    public bool canStartPunch = false;

    private bool isP1FinishPurchase = false;
    private bool isP2FinishPurchase = false;

    public List<Upgrade> p1EstateList;
    public List<Upgrade> p2EstateList;

    public int turnNumber = 0;

    public static int leftPuchTimes;

    public GameObject punchObject;

    public int player_one_dice_one;
    public int player_one_dice_two;

    public int player_two_dice_one;
    public int player_two_dice_two;

    public GameObject P1Picture;
    public GameObject P2Picture;

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
    public Button playerOneDiceButton;
    public Button playerTwoDiceButton;

    public GameObject pickCoinButton;
    public GameObject pickHateButton;

    public GameObject p1FinishPurchaseButton;
    public GameObject p2FinishPurchaseButton;

    [Header("HateTokenInquiry")]
    public GameObject inquiryBar;
    public TextMeshProUGUI inquiryText;
    private int usedToken = 0;

    public static int damage = 0;

    private bool hasEnd = false;


    void Start()
    {
        currentPlayStage = PlayStage.rolldice;
        player_one_health = 15;
        player_two_health = 15;
        arm.SetActive(false);
        pickCoinButton.SetActive(false);
        pickHateButton.SetActive(false);
        gameplayIllustration.text = "Roll your dices, the winner can PUNCH the loser!!";
        playerOneDiceButton.onClick.AddListener(RollP1Dice);
        playerTwoDiceButton.onClick.AddListener(RollP2Dice);
    }

    void EndPlay()
    {
        if (player_one_health <= 0)
        {
            GetComponent<VideoPlayer>().Play();
            gameplayIllustration.text = "P2 Wins!";
            P2Picture.SetActive(true);
            hasEnd = true;


        }
        if (player_two_health <= 0)
        {
            GetComponent<VideoPlayer>().Play();
            P1Picture.SetActive(true);
            gameplayIllustration.text = "P1 Wins!";
            hasEnd = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnd)
        {
            UpdatePlayerDice();
            DiceCompare();
            UpdateTexts();
            CheckIfGoToGainIncome();
            if (isInquiryHate)
            {
                InquryPunch();
            }
        }
       
        EndPlay();

       

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    public void RollP1Dice()
    {
        player_one_dice_one = Random.Range(1, 6);
        player_one_dice_two = Random.Range(1, 6);
        hasPlayerOneRolled = true;
        playerOneDiceButton.onClick.RemoveAllListeners();
    }

    public void RollP2Dice()
    {
        player_two_dice_one = Random.Range(1, 6);
        player_two_dice_two = Random.Range(1, 6);
        hasPlayerTwoRolled = true;
        playerTwoDiceButton.onClick.RemoveAllListeners();
    }

    void UpdateTexts()
    {
        
        p1CoinText.text = $"Coins: {p1_money}";
        p1HateTokenText.text = $"Hate Tokens: {player_one_hate_token}";
        p2CoinText.text = $"Coins: {p2_money}";
        p2HateTokenText.text = $"Hate Tokens: {player_two_hate_token}";
        player_one_health = player_one.GetComponent<PunchableObject>().health;
        player_two_health = player_two.GetComponent<PunchableObject>().health;
        p1HealthText.text = $"HP: {player_one.GetComponent<PunchableObject>().health}";
        p2HealthText.text = $"HP: {player_two.GetComponent<PunchableObject>().health}";
    }

    private void DiceCompare()
    {
        if (hasPlayerOneRolled && hasPlayerTwoRolled && currentPlayStage == PlayStage.rolldice && !hasAddedHateToken)
        {
            confirmButton.gameObject.SetActive(true);

            if (player_one_dice_one + player_one_dice_two > player_two_dice_one + player_two_dice_two)
            {
                if (player_one_dice_one == player_one_dice_two)
                {
                    gameplayIllustration.text = "P1 wins this round, you can punch twice because you rolled two same number!";
                    leftPuchTimes = 2;
                    buttonText.text = "OK";
                    isPlayerOnePunching = true;
                    hasAddedHateToken = true;
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
                    hasAddedHateToken = true;
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
                    hasAddedHateToken = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);
                }
                else
                {
                    gameplayIllustration.text = "P2 wins this round, you can punch once!";
                    leftPuchTimes = 1;
                    buttonText.text = "OK";
                    isPlayerTwoPunching = true;
                    hasAddedHateToken = true;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(ChangePunchStage);
                }
            }
            else if (player_one_dice_one + player_one_dice_two == player_two_dice_one + player_two_dice_two)
            {
                gameplayIllustration.text = "A draw! No one can punch this round! But you are all rewarded with one hate token because the temperary peace makes you want to puch each other more!";
                buttonText.text = "OK";
                if (!hasAddedHateToken)
                {
                    player_one_hate_token++;
                    player_two_hate_token++;
                    confirmButton.onClick.RemoveAllListeners();
                    confirmButton.onClick.AddListener(CheckIfCanGoToStatePurchase);

                }

                leftPuchTimes = 0;
                isPlayerTwoPunching = false;
                isPlayerOnePunching = false;
                hasAddedHateToken = true;
            }
        }

    }

    private void ChangePunchStage()
    {
        player_one_dice_one = 0;
        player_one_dice_two = 0;
        player_two_dice_one = 0;
        player_two_dice_two = 0;

        currentPlayStage = PlayStage.prepare_punch;
        hasPlayerOneRolled = false;
        hasPlayerTwoRolled = false;
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
        if (isPlayerOnePunching && player_one_hate_token > 0)
        {
            usedToken += 1;
            player_one_hate_token--;
        }
        else if (isPlayerTwoPunching && player_two_hate_token > 0)
        {
            usedToken += 1;
            player_two_hate_token--;
        }
    }

    public void OnClickDecreaseHateToken()
    {
        if (isPlayerOnePunching && usedToken > 0)
        {
            usedToken += -1;
            player_one_hate_token++;
        }
        else if (isPlayerTwoPunching && usedToken > 0)
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
        isInquiryHate = false;
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
            if (punchObject.GetComponent<Upgrade>() == null)
            {
                arm.SetActive(false);
                textBar.SetActive(true);
                Cursor.visible = true;
                gameplayIllustration.text = $"P1, you gain {damage} coins for your punch! " +
                   $"P2, you can choose {damage} hate tokens for your increasing resentment or { damage * 2 } coins as your compensation, which do you want?";
                p1_money += damage;
                pickCoinButton.SetActive(true);
                pickHateButton.SetActive(true);
                punchObject = null;

            }
            else if (punchObject.GetComponent<Upgrade>() != null)
            {
                arm.SetActive(false);
                textBar.SetActive(true);
                Cursor.visible = true;
                gameplayIllustration.text = $"P1, you gain 0 coin for your punch of the estate! " +
                   $"P2, you gain no compensation.";
                confirmButton.gameObject.SetActive(true);
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(CheckIfCanGoToStatePurchase);
                punchObject = null;


            }

        }
        else if (isPlayerTwoPunching)
        {
            if (punchObject.GetComponent<Upgrade>() == null)
            {
                arm.SetActive(false);
                textBar.SetActive(true);
                Cursor.visible = true;
                gameplayIllustration.text = $"P2, you gain {damage} coins for your punch! " +
                    $"P1, you can choose {damage} hate tokens for your increasing resentment or { damage * 2 } coins as your compensation, which do you want?";
                p2_money += damage;
                pickCoinButton.SetActive(true);
                pickHateButton.SetActive(true);
                punchObject = null;

            }
            else if (punchObject.GetComponent<Upgrade>() != null)
            {
                arm.SetActive(false);
                textBar.SetActive(true);
                Cursor.visible = true;
                gameplayIllustration.text = $"P2, you gain 0 coin for your punch of the estate! " +
                    $"P1, you gain no compensation";
                confirmButton.gameObject.SetActive(true);
                confirmButton.onClick.RemoveAllListeners();
                confirmButton.onClick.AddListener(CheckIfCanGoToStatePurchase);
                punchObject = null;
            }

        }
    }

    public void OnClickAddCoin()
    {
        pickCoinButton.SetActive(false);
        pickHateButton.SetActive(false);
        if (isPlayerOnePunching)
        {
            p2_money += damage*2;
            CheckIfCanGoToStatePurchase();
        }
        else if (isPlayerTwoPunching)
        {
            p1_money += damage * 2;
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
            isPlayerOnePunching = false;
            isPlayerTwoPunching = false;
            damage = 0;
            currentPlayStage = PlayStage.state_purchase;
            gameplayIllustration.text = "Now is the purchase stage, click on the estate to spend 2 coins to buy or upgrade 1 estate. The estates can give you reward each turn!";
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
            GoToPlayerOnestateCalculate();
        }
    }

    private void GoToPlayerOnestateCalculate()
    {
        if (currentPlayStage == PlayStage.gain_state_income )
        {
            int p1LevelOneEstate = 0;
            int p1LevelTwoEstate = 0;
            int p1LevelThreeEstate = 0;
            int p1LevelFourEstate = 0;

            for (int i = 0; i < p1EstateList.Count; i++)
            {
                if (p1EstateList[i].thisUpgradeLevel == 1)
                {
                    p1LevelOneEstate += 1;
                }
                else if (p1EstateList[i].thisUpgradeLevel == 2)
                {
                    p1LevelTwoEstate++;
                }
                else if (p1EstateList[i].thisUpgradeLevel == 3)
                {
                    p1LevelThreeEstate++;
                }
                else if (p1EstateList[i].thisUpgradeLevel == 4)
                {
                    p1LevelFourEstate++;
                }
            }

            

            gameplayIllustration.text = $"P1 has {p1LevelOneEstate} lv.1, {p1LevelTwoEstate} lv.2, {p1LevelThreeEstate} lv.3, and {p1LevelFourEstate} lv.4 estates, which brings {p1LevelOneEstate+ p1LevelTwoEstate*2+ p1LevelThreeEstate*2+ p1LevelFourEstate*2} coin and {p1LevelThreeEstate+ p1LevelFourEstate} hate tokens, and {p1LevelFourEstate} health to P1 this turn.";
            p1_money += p1LevelOneEstate + p1LevelTwoEstate * 2 + p1LevelThreeEstate * 2 + p1LevelFourEstate * 2;
            player_one_hate_token += p1LevelThreeEstate + p1LevelFourEstate;
            player_one.GetComponent<PunchableObject>().health += p1LevelFourEstate;
            confirmButton.gameObject.SetActive(true);
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnClickGoToPlayerTwoEstateCalculate);
        }
    }

    private void OnClickGoToPlayerTwoEstateCalculate()
    {
        int p2LevelOneEstate = 0;
        int p2LevelTwoEstate = 0;
        int p2LevelThreeEstate = 0;
        int p2LevelFourEstate = 0;


        for (int i = 0; i < p2EstateList.Count; i++)
        {
            if (p2EstateList[i].thisUpgradeLevel == 1)
            {
                p2LevelOneEstate += 1;
            }
            else if (p2EstateList[i].thisUpgradeLevel == 2)
            {
                p2LevelTwoEstate++;
            }
            else if (p2EstateList[i].thisUpgradeLevel == 3)
            {
                p2LevelThreeEstate++;
            }
            else if (p2EstateList[i].thisUpgradeLevel == 4)
            {
                p2LevelFourEstate++;
            }
        }

        gameplayIllustration.text = $"P1 has {p2LevelOneEstate} lv.1, {p2LevelTwoEstate} lv.2, {p2LevelThreeEstate} lv.3, and {p2LevelFourEstate} lv.4 estates, which brings {p2LevelOneEstate + p2LevelTwoEstate * 2 + p2LevelThreeEstate * 2 + p2LevelFourEstate * 2} coins, {p2LevelThreeEstate + p2LevelFourEstate} hate tokens and {p2LevelFourEstate} health to P2 this turn.";
        p2_money += p2LevelOneEstate + p2LevelTwoEstate * 2 + p2LevelThreeEstate * 2 + p2LevelFourEstate * 2;
        player_two_hate_token += p2LevelThreeEstate + p2LevelFourEstate;
        player_two.GetComponent<PunchableObject>().health += p2LevelFourEstate;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(GoToRollDiceStage);
    }

    private void GoToRollDiceStage()
    {
        player_one_dice_one = 0;
        player_one_dice_two = 0;
        player_two_dice_one = 0;
        player_two_dice_two = 0;
        hasPlayerOneRolled = false;
        hasPlayerTwoRolled = false;
        hasAddedHateToken = false;
        confirmButton.gameObject.SetActive(false);
        currentPlayStage = PlayStage.rolldice;
        gameplayIllustration.text = "Roll your dices, the winner can PUNCH the loser!!";
        playerOneDiceButton.onClick.AddListener(RollP1Dice);
        playerTwoDiceButton.onClick.AddListener(RollP2Dice);
    }
    private void UpdatePlayerDice()
    {
        playerOneDice.text = $"Dices: {player_one_dice_one} & {player_one_dice_two}";
        playerTwoDice.text = $"Dices: {player_two_dice_one} & {player_two_dice_two}";

    }
}
