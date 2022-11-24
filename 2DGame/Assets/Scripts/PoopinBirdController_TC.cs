using UnityEngine;

public class PoopinBirdController_TC : MonoBehaviour
{
    //moving mechanism
    public Vector2 position;
    private bool NeedToMove;
    public float moveRange;
    public float XVelocity;
    //pooping mechanism
    [SerializeField] private Transform pfPoop;
    public bool moveRight;
    private int PoopCounterSpawn;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        NeedToMove = true;
        moveRange = 15;
        XVelocity = -1;
        moveRight = false;
        PoopCounterSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //in this section we implement the movement of the birdie
        Vector2 effectivePosition = transform.position;
        if (NeedToMove)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XVelocity, 0);
            NeedToMove = false;
        }
        if (effectivePosition.x + moveRange < position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-XVelocity, 0);
            moveRight = true;
        }
        if (effectivePosition.x - moveRange > position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XVelocity, 0);
            moveRight = false;
        }

        //in this section we emplement the pooping mechanism
        int poopSpawnRandomized = Random.Range(0, 30);
        if (PoopCounterSpawn == poopSpawnRandomized)
        {
            Vector2 PoopPosition = transform.position;
            Transform poopTransform = Instantiate(pfPoop, PoopPosition, Quaternion.identity);

            Vector3 poopDir = new Vector3(moveRight == true ? 1 : -1, 1f, 0);
            poopTransform.GetComponent<PoopBehaviour_TC>().Setup(poopDir);

        }
    }
    private void FixedUpdate()
    {
        PoopCounterSpawn++;
        if (PoopCounterSpawn > 60)
            PoopCounterSpawn = 0;
    }
}
