using UnityEngine;

public class Character_Movemennt : MonoBehaviour
{
    public float speed = 5f; // µ¥Î»£ºÃ×/Ãë

    void Update()
    {
        float v = 0f;

        if (Input.GetKey(KeyCode.W))
            v = 1f;
        else if (Input.GetKey(KeyCode.S))
            v = -1f;

        transform.position += (Vector3)(transform.up * v * speed * Time.deltaTime);
    }
}
