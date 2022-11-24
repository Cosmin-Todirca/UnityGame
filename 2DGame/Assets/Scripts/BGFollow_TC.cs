using UnityEngine;

//e corect, dar foloseste parallexBackground
//Dani tutorial

public class BGFollow_TC : MonoBehaviour
{
    private float length, startpos; //of the sprite
    public GameObject cam;
    public float followSpeed;
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - followSpeed));
        float dist = (cam.transform.position.x * followSpeed);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}
