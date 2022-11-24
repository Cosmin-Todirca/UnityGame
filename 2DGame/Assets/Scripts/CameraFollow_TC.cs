using UnityEngine;

//https://www.youtube.com/watch?v=FXqwunFQuao

public class CameraFollow_TC : MonoBehaviour
{
    // Start is called before the first frame update
    public float FollowSpeed = 5f;
    public float yOffset = 0f;
    public float zOffset = -11f;
    public Transform target;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, zOffset);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
