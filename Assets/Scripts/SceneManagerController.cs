using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    private static SceneManagerController _instance;

    public static SceneManagerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneManagerController>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SceneManagerController>();
                    singletonObject.name = "SceneManagerSingleton";
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch (sceneIndex)
            {
                case 0:
                    Application.Quit();
                    break;
                case 1:
                    SwitchScene(sceneIndex -1);
                    break;
                case 2:
                    SwitchScene(sceneIndex - 1);
                    break;
                case 3:
                    SwitchScene(sceneIndex - 1);
                    break;
                case 4:
                    SwitchScene(1);
                    break;
                case 5:
                    SwitchScene(1);
                    break;
                default:
                    break;
            }
        }
    }
}

