using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("--- 遷移先のゲームシーン名 ---")]
    public string nextSceneName = "SampleScene"; // メインのゲームシーン名

    void Update()
    {
        // Enterキー（ReturnキーまたはテンキーのEnter）が押されたら
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartGame();
        }
    }

    // ゲームシーンに遷移する処理
    public void StartGame()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}