using UnityEngine;

//https://www.youtube.com/watch?v=wBol2xzxCOU&t=429s
//nu uita sa schimbi modul de desenare pe tiles si sa il faci 3X marimea initiala a imaginii

public class ParallexBackground_TC : MonoBehaviour
{
    public Vector2 parallexEffectMultiplier;
    public bool infiniteHorizontal;
    public bool infiniteVertical;

    private Transform cameraTransform;
    private Vector2 lastCameraPosition;
    private float textureUnitSizeX, textureUnitSizeY;

    private float originalZ;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        originalZ = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()//ca sa ne asiguram ca se face update la script dupa ce s-a modificat pozitia camerei
    {
        Vector2 cameraTransform2 = new Vector2(cameraTransform.position.x, cameraTransform.position.y);
        Vector2 deltaMovement = cameraTransform2 - lastCameraPosition;//am trecut in vector 2
        transform.position += new Vector3(deltaMovement.x * parallexEffectMultiplier.x, deltaMovement.y * parallexEffectMultiplier.y, 0);
        lastCameraPosition = cameraTransform.position;

        //sa verificam daca trebuie sa mutam fundalul
        if (infiniteHorizontal)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y, originalZ);//era vector 3 aici, dar nu aveam al treilea parametru
            }
        }
        if (infiniteVertical)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(cameraTransform.position.x, transform.position.y + offsetPositionY, originalZ);
            }
        }
    }
}
