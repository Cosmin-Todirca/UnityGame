using System.Collections;
using UnityEngine;

/// <summary>
///https://www.youtube.com/watch?v=ptvK4Fp5vRY
///https://www.youtube.com/watch?v=hkaysu1Z-N8
/// din animator, la animatie, avem loop si scoatem
/// </summary>

public class BlobMovement_TC : MonoBehaviour
{
    public GameObject BLOB;
    private Vector3 initPoz = new Vector3();
    private Rigidbody2D rigidbody;
    private CircleCollider2D circleCollider;
    [SerializeField] private LayerMask platformsLayerMask;
    private bool dreapta = true;
    public int punctPivotare;
    public float timpAsteptare;
    public Animator animator;

    private void Awake()
    {
        punctPivotare = Random.Range(1, 25);
        timpAsteptare = Random.Range(1.0f, 3.0f);
    }
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
        initPoz = BLOB.transform.position;
        circleCollider = transform.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        timpAsteptare = Random.Range(1.0f, 3.0f);

        if (initPoz.x - BLOB.transform.position.x < -punctPivotare && dreapta == true && isGrounded())//merge in dreapta
        {
            //Debug.Log("rotit stanga");
            dreapta = false;
            BLOB.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (initPoz.x - BLOB.transform.position.x > punctPivotare && dreapta == false && isGrounded())//adica merge in stanga
        {
            //Debug.Log("rotit dreapta");
            dreapta = true;
            BLOB.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (isGrounded())
        {
            animator.SetBool("IsJumping", false);
            //Invoke("saritura", 2);
            StartCoroutine(saritura());
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }
    }
    public IEnumerator saritura()
    {
        //in loc de void IEnumerator
        //si in loc de invoke, StartCoroutine(functia)
        yield return new WaitForSeconds(timpAsteptare);
        if (dreapta)
            rigidbody.velocity = new Vector2(1f, 2f);
        else
            rigidbody.velocity = new Vector2(-1f, 2f);
        //rigidbody.AddForce(new Vector2(10f, 10f));
        //Debug.Log("jump");
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(circleCollider.bounds.center, circleCollider.bounds.size, 0f, Vector2.down, .1f, platformsLayerMask);
        //Debug.Log(raycastHit2d.collider);
        return raycastHit2d.collider != null;
    }
}
