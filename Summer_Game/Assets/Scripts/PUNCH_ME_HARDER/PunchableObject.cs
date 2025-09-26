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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnClickPunchThis()
    {
        if (gameManager.currentPlayStage == Punch_Me_Game_Manager.PlayStage.punch)
        {
            gameManager.punchObject = this.gameObject;
            gameManager.InquryPunch();
            //gameManager.canStartPunch = true;
            //collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Object")
        {
            health -= gameManager.damage;
            Punch_Me_Game_Manager.leftPuchTimes--;
            gameManager.CheckEnterGainPunchCompensation();
        }
    }


}
