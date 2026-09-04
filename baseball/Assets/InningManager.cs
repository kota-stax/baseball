using UnityEngine;
using TMPro; // TextMeshProを使用
using UnityEngine.SceneManagement; // ★【追加】画面遷移に必要

public class InningManager : MonoBehaviour
{
    [Header("--- イニング表示用テキスト ---")]
    public TMP_Text inningText;

    [Header("--- 画面遷移の設定 ---")]
    [Tooltip("遷移先のリザルトシーンの名前")]
    public string resultSceneName = "ResultScene";
    [Tooltip("GAME SET表示からリザルト画面に遷移するまでの待ち時間（秒）")]
    public float transitionDelay = 3f;

    private int currentInning = 1;   // 現在の回（1~2回）
    private bool isTop = true;       // true: 表(TOP) / false: 裏(BOT)
    private bool isGameOver = false; // 試合終了フラグ

    void Start()
    {
        UpdateInningUI();
    }

    // 3アウト時（チェンジ時）にBSOManagerから呼ばれる関数
    public void ChangeInning()
    {
        if (isGameOver) return;

        if (isTop)
        {
            // 「表」から「裏」へ切り替え
            isTop = false;
        }
        else
        {
            // 「裏」から「次の回の表」へ切り替え
            isTop = true;
            currentInning++;
        }

        // 2回裏が終わったらゲームセット判定
        if (currentInning > 2)
        {
            SetGameOver();
            return;
        }

        // 攻撃チーム（スコアの加算先）を切り替える
        ScoreManager scoreMgr = GameObject.FindAnyObjectByType<ScoreManager>();
        if (scoreMgr != null)
        {
            scoreMgr.SetTurn(isTop);
        }

        UpdateInningUI();
    }

    // イニング表示の更新（英語表記）
    void UpdateInningUI()
    {
        if (inningText != null)
        {
            // 1回: 1st / 2回: 2nd
            string suffix = (currentInning == 1) ? "st" : "nd";
            string topBottom = isTop ? "TOP" : "BOT";

            // 表示例: "TOP 1st" や "BOT 2nd"
            inningText.text = $"{topBottom} {currentInning}{suffix}";
        }
    }

    // 試合終了処理
    void SetGameOver()
    {
        isGameOver = true;
        if (inningText != null)
        {
            inningText.text = "GAME SET!";
        }
        Debug.Log("<color=gold>★★★ 試合終了！ GAME SET ★★★</color>");

        // ★【追加】数秒後にリザルト画面へ遷移する
        Invoke("LoadResultScene", transitionDelay);
    }

    // リザルト画面を読み込む関数
    void LoadResultScene()
    {
        SceneManager.LoadScene(resultSceneName);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}