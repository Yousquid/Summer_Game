using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Sprite level_one;
    public Sprite level_two;
    public Sprite level_three;
    public Sprite level_four;

    private SpriteRenderer spriteRenderer;

    public bool isBelongingToPlayerOne;

    private Collider2D thisCollider;

    private Punch_Me_Game_Manager gameManager;

    private int thisUpgradeLevel = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisCollider = GetComponent<Collider2D>();
        gameManager = GameObject.FindWithTag("Manager").GetComponent<Punch_Me_Game_Manager>();

    }

    void Update()
    {
        UpdateImage();

        if (gameManager.currentPlayStage != Punch_Me_Game_Manager.PlayStage.state_purchase)
        {
            thisCollider.enabled = false;
        }
        else if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.state_purchase)
        {
            thisCollider.enabled = true;
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
    }
    private void OnMouseDown()
    {
        if (isBelongingToPlayerOne && gameManager.p1_money >= 1 && thisUpgradeLevel < 4)
        {
            gameManager.p1_money--;
            thisUpgradeLevel++;
        }
        else if (!isBelongingToPlayerOne && gameManager.p2_money >= 1 && thisUpgradeLevel < 4)
        {
            gameManager.p2_money--;
            thisUpgradeLevel++;
        }
    }
}
