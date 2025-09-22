using UnityEngine;
using UnityEngine.UI;
public class MealEat : MonoBehaviour
{
    public Sprite eat_once;
    public Sprite eat_twice;
    private SpriteRenderer sprite;
    private bool canEat = false;
    private int leftEatTimes = 3;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BasketEnd")
        {
            if (leftEatTimes <= 1)
            { 
                //GO TO NEXT STAGE
            }
        }
        if (collision.tag == "Light")
        {
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            canEat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            canEat = false;
        }
    }

    private void Update()
    {
        if (canEat)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                leftEatTimes -= 1;
            }
        }

        if (leftEatTimes == 2)
        {
            sprite.sprite = eat_once;
        }
        if (leftEatTimes == 1)
        {
            sprite.sprite = eat_twice;
        }
    }

}
