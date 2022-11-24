using UnityEngine;

//https://www.youtube.com/watch?v=Nke5JKPiQTw

public class PoopBehaviour_TC : MonoBehaviour
{
    public float Speed = 1f;
    private Vector3 poopDir;
    [SerializeField] private Transform PoopMark;

    void Update()
    {
        transform.position += -poopDir * Time.deltaTime * Speed; //move forward
    }
    private void OnCollisionEnter2D(Collision2D collision) //asta daca e debifat IsTrigger
    {
        if (collision.collider.tag != "PoopinBird")
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)//daca e bifat IsTrigger
    {
        if (!collision.tag.Equals("PoopinBird"))
        {
            if (collision.tag.Equals("StaticPlatform"))//sa faca urma numai pe pamant
                Instantiate(PoopMark, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    public void Setup(Vector3 shootdir)
    {
        this.poopDir = shootdir;
        Destroy(gameObject, 5f);//fiindca unele mai trec prin harta
    }
    //sa pun dupa, sa instantieze urma de gainat care se terge in start dupa 5 secunde. dupa destroy mai pui un 5f.
}
