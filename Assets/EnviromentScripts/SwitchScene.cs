using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public string targetSceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneTransitionManager.Instance.TransitionTo(targetSceneName);
        }
    }
}
