using UnityEngine;

public class Bird1_TC : MonoBehaviour
{
    //implementat "munitie" ce pica in jos
    public GameObject bird;
    public Camera camera;
    public float relativeDistance;
    public float relativeSpeed;

    //setate ok pentru aspectul estetic.
    private int loopY = 0;
    private int loopYlimit = 10;
    private float down = 0.005f;


    private void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //resetarea
        if (Mathf.Abs(bird.transform.position.x - camera.transform.position.x) > relativeDistance)
        {
            bird.transform.position = new Vector3(camera.transform.position.x + relativeDistance - 0.01f, bird.transform.position.y, bird.transform.position.z);
        }

        //deplasarea
        bird.transform.position = new Vector3(bird.transform.position.x - (relativeSpeed * Time.deltaTime), bird.transform.position.y + down, bird.transform.position.z);

        //efectul de sus/jos.
        if (down > 0)
            loopY++;
        else
            loopY--;

        if (loopY < -loopYlimit)
            down = -down;
        if (loopY > loopYlimit)
            down = -down;

    }
}
