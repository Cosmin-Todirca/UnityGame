using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartGame_TC : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        SceneManager.LoadScene(1);

    }


}
