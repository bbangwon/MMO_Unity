using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene => Object.FindObjectOfType<BaseScene>();
    public void LoadScene(Define.Scene type)
    {
        CurrentScene.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }
}
