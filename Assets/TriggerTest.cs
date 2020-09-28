using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log("Stay");
    }
}
