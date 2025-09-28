using UnityEngine;

public class PunchableObject : MonoBehaviour
{
    private Punch_Me_Game_Manager gameManager;
    public int health;
    private Collider2D collider;

    public bool isBelongingToP1;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        gameManager = GameObject.FindWithTag("Manager").GetComponent<Punch_Me_Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && this.tag != "Player")
        {
            GetComponent<Upgrade>().thisUpgradeLevel = 0;
            health = 1;
        }

        if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.state_purchase)
        {
            collider.enabled = true;

        }
        else if (gameManager.currentPlayStage != Punch_Me_Game_Manager.PlayStage.prepare_punch && gameManager.punchObject != this.gameObject)
        {
            collider.enabled = false;
        }
        else if (gameManager.currentPlayStage != Punch_Me_Game_Manager.PlayStage.prepare_punch && gameManager.punchObject == this.gameObject)
        {
            collider.enabled = true;

        }
        if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.prepare_punch)
        {
            if (gameManager.isPlayerOnePunching && isBelongingToP1)
            {
                collider.enabled = false;

            }
            else if (gameManager.isPlayerOnePunching && !isBelongingToP1)
            {
                collider.enabled = true;
            }
            if (gameManager.isPlayerTwoPunching && isBelongingToP1)
            {
                collider.enabled = true;
            }
            else if (gameManager.isPlayerTwoPunching && !isBelongingToP1)
            {
                collider.enabled = false;

            }
        }

    }

    public void OnClickPunchThis()
    {
        if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.prepare_punch)
        {
            gameManager.punchObject = this.gameObject;
            gameManager.InquryPunch();
            gameManager.canStartPunch = true;
            gameManager.isInquiryHate = true;
            //collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            health -= Punch_Me_Game_Manager.damage;
            Punch_Me_Game_Manager.leftPuchTimes--;
            gameManager.CheckEnterGainPunchCompensation();
        
    }

    private void OnMouseDown()
    {
        if (this.GetComponent<Upgrade>() == null)
        {
            OnClickPunchThis();

        }
        else if (this.GetComponent<Upgrade>()!= null && this.GetComponent<Upgrade>().thisUpgradeLevel != 0)
        {
            OnClickPunchThis();

        }
    }
}
