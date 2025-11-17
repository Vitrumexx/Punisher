using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string targetSceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Загружаем новую сцену
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
