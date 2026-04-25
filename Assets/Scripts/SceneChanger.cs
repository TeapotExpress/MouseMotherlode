using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{

    public static SceneChanger instance;
    private void Awake()
    {
        instance = this;
    }
    public void SceneChange(string target)
    {
        SceneManager.LoadScene(target);
    }
}
