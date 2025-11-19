using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    [HideInInspector] public StretchZone currentZone;

    void OnTriggerEnter2D(Collider2D other)
    {
        var zone = other.GetComponent<StretchZone>();
        if (zone != null)
        {
            currentZone = zone;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var zone = other.GetComponent<StretchZone>();
        if (zone != null && currentZone == zone)
        {
            currentZone = null;
        }
    }
}
