using UnityEngine;

public class UnlockingMechanism_TC : MonoBehaviour
{
    // Start is called before the first frame update
    public void Unlock()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
