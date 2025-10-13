using UnityEngine;
using UnityEngine.SceneManagement;
public class Character_Movemennt : MonoBehaviour
{
    public float speed = 2f; 
    public GameObject loseWord;
    public GameObject winWord;
    public GameObject winWordScore;


    void Update()
    {
        float v = 0f;

        if (Input.GetKey(KeyCode.W))
            v = 1f;
        else if (Input.GetKey(KeyCode.S))
            v = -1f;

        transform.position += (Vector3)(transform.up * v * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            People_Spawner.score = 30;
            People_Spawner.hasStop = false;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Select_one"))
        {
            loseWord.SetActive(true);
        }

        if (collision.CompareTag("Select_two"))
        {
            winWord.SetActive(true);
            winWordScore.SetActive(true);
            People_Spawner.hasStop = true;
        }
    }
}
