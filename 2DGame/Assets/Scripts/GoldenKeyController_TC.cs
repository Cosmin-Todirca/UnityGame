using UnityEngine;

public class GoldenKeyController_TC : MonoBehaviour
{
    [SerializeField] public Transform lockedGate;
    Vector2 gatePosition;
    Transform gate;

    private void Start()
    {
        gatePosition = new Vector2(34, 0.5f);
        gate = Instantiate(lockedGate, gatePosition, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            gate.GetComponent<UnlockingMechanism_TC>().Unlock();

        }
    }
}
