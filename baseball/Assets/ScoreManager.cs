using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("--- スコア表示用UI ---")]
    public TMP_Text scoreText_My;     // 自分（表）
    public TMP_Text scoreText_Enemy;  // 相手（裏）

    // ★【追加】リザルト画面でもスコアを保持・参照するための静的（static）変数
    public static int MyScore { get; private set; } = 0;
    public static int EnemyScore { get; private set; } = 0;

    private bool isMyTurn = true; // true: 表(自分) / false: 裏(相手)

    void Start()
    {
        // ゲーム開始時にスコアをリセット
        MyScore = 0;
        EnemyScore = 0;
        UpdateScoreUI();
    }

    // 攻撃ターンの切り替え（InningManagerから呼ばれる）
    public void SetTurn(bool isTop)
    {
        isMyTurn = isTop;
    }

    // 得点加算処理
    public void AddScore(int amount = 1)
    {
        if (isMyTurn)
        {
            MyScore += amount;
            Debug.Log($"自分に {amount} 点追加！ 現在: {MyScore}点");
        }
        else
        {
            EnemyScore += amount;
            Debug.Log($"相手に {amount} 点追加！ 現在: {EnemyScore}点");
        }

        UpdateScoreUI();
    }

    // 画面のスコア表示を更新
    void UpdateScoreUI()
    {
        if (scoreText_My != null)
        {
            scoreText_My.text = MyScore.ToString();
        }
        if (scoreText_Enemy != null)
        {
            scoreText_Enemy.text = EnemyScore.ToString();
        }
    }
}