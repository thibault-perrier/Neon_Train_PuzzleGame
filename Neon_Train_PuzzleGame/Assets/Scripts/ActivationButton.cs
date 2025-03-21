using UnityEngine;

public class ActivationButton : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Recognized a {other.tag}");
        ActivateTilePower();
    }

    public void ActivateTilePower()
    {
        Debug.Log($"Test Button");
        // GetComponent<Track>().
    }
}
