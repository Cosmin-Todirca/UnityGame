using UnityEngine;

public class GameController_TC : MonoBehaviour
{
    public PlayerController_TC player;
    public Camera cam;
    public float maxDistanceBetweenCamAndPlayer = 9f;
    public float camFollowVelocity = 8f;
    public bool debugMode = false;

    private Vector3 initCamPos;

    // Start is called before the first frame update
    void Start()
    {
        initCamPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraToFollowPlayer();
        if (player.transform.position.y < -20)
        {
            player.Teleport(new Vector3(0, 10, player.transform.position.z));
            player.health.Die();
            cam.transform.position = initCamPos;

        }

        if (debugMode == true)
        {
            ConsoleDebug();
        }
    }

    private void ConsoleDebug()
    {
        if (player.velocity.magnitude > 0)
        {

            Debug.Log($"Player position: x:{player.transform.position.x} y:{player.transform.position.y}");
            Debug.Log($"Camera position: x:{cam.transform.position.x} y:{cam.transform.position.y}");

        }
    }

    private void UpdateCameraToFollowPlayer()
    {
        float distanceXCamPlayer = player.transform.position.x - cam.transform.position.x;
        Vector3 cameraPosition = cam.transform.position;

        if (distanceXCamPlayer > maxDistanceBetweenCamAndPlayer)
        {
            cameraPosition.x += camFollowVelocity * Time.deltaTime;
            cam.transform.position = cameraPosition;
        }
        else if (distanceXCamPlayer < -maxDistanceBetweenCamAndPlayer)
        {
            cameraPosition.x -= camFollowVelocity * Time.deltaTime;
            cam.transform.position = cameraPosition;
        }





    }
}

