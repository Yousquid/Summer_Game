using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Sprite level_one;
    public Sprite level_two;
    public Sprite level_three;
    public Sprite level_four;
    public Sprite level_zero;

    private SpriteRenderer spriteRenderer;

    public bool isBelongingToPlayerOne;

    private Collider2D thisCollider;

    private Punch_Me_Game_Manager gameManager;

    public int thisUpgradeLevel = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisCollider = GetComponent<Collider2D>();
        gameManager = GameObject.FindWithTag("Manager").GetComponent<Punch_Me_Game_Manager>();

    }

    void Update()
    {
        UpdateImage();

        if (thisUpgradeLevel != 0)
        {
            if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.state_purchase || gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.prepare_punch)
            {
                thisCollider.enabled = true;
            }
            else if (gameManager.punchObject != this.gameObject)
            {
                thisCollider.enabled = false;
            }
        }
        else if (thisUpgradeLevel == 0)
        {
            
        }
        
    }

    private void UpdateImage()
    {
        if (thisUpgradeLevel == 1)
        {
            spriteRenderer.sprite = level_one;
        }
        else if (thisUpgradeLevel == 2)
        {
            spriteRenderer.sprite = level_two;
        }
        else if (thisUpgradeLevel == 3)
        {
            spriteRenderer.sprite = level_three;
        }
        else if (thisUpgradeLevel == 4)
        {
            spriteRenderer.sprite = level_four;
        }
        else if (thisUpgradeLevel == 0)
        {
            spriteRenderer.sprite = level_zero;
        }
    }
    private void OnMouseDown()
    {
        if (isBelongingToPlayerOne && gameManager.p1_money >= 2 && thisUpgradeLevel < 4 && gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.state_purchase)
        {
            gameManager.p1_money-=2;
            thisUpgradeLevel++;
        }
        else if (!isBelongingToPlayerOne && gameManager.p2_money >= 2 && thisUpgradeLevel < 4 && gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.state_purchase)
        {
            gameManager.p2_money-=2;
            thisUpgradeLevel++;
        }
    }
}
