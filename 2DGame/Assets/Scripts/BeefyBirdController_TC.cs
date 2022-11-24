using UnityEngine;

public class BeefyBirdController_TC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("BeefyBirdAnim", 0, Random.Range(0.0f, 5.0f));
    }


    // Update is called once per frame
    void Update()
    {

    }
}
