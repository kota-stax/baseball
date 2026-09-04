using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultDisplay : MonoBehaviour
{
    [Header("--- 得点表示テキスト ---")]
    public TMP_Text myScoreText;    // 自分のスコア用テキスト
    public TMP_Text enemyScoreText; // 相手のスコア用テキスト

    [Header("--- 勝敗結果表示テキスト（任意） ---")]
    public TMP_Text resultText;     // WIN / LOSE / DRAW 表示用テキスト

    [Header("--- 遷移先シーン名 ---")]
    [Tooltip("Enterキーを押した時に戻るタイトルシーンの名前")]
    public string titleSceneName = "TitleScene";

    void Start()
    {
        // ScoreManager に保存されている数値を読み出して表示
        int myScore = ScoreManager.MyScore;
        int enemyScore = ScoreManager.EnemyScore;

        if (myScoreText != null)
        {
            myScoreText.text = myScore.ToString();
        }

        if (enemyScoreText != null)
        {
            enemyScoreText.text = enemyScore.ToString();
        }

        // 勝敗テキストの更新
        if (resultText != null)
        {
            if (myScore > enemyScore)
            {
                resultText.text = "YOU WIN!";
                resultText.color = Color.red;
            }
            else if (myScore < enemyScore)
            {
                resultText.text = "YOU LOSE...";
                resultText.color = Color.blue;
            }
            else
            {
                resultText.text = "DRAW";
                resultText.color = Color.yellow;
            }
        }
    }

    void Update()
    {
        // ★【追加】Enterキー（ReturnキーまたはテンキーのEnter）が押されたらタイトルに戻る
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ToTitle();
        }
    }

    // タイトル画面へ遷移する処理
    public void ToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}