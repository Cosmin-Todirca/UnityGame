using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://www.youtube.com/watch?v=gChUoKShORY
public class ToNextScene_TC : MonoBehaviour
{
    // Start is called before the first frame update
    private int nextSceneToLoad;
    void Start()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (SceneManager.GetSceneAt(nextSceneToLoad) != null)
            {
                SceneManager.LoadScene(nextSceneToLoad);
            }
        }
        catch(IndexOutOfRangeException e)
        {
            Application.Quit();
        }
    }
}
