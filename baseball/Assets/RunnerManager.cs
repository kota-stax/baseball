using UnityEngine;
using UnityEngine.UI;

public class RunnerManager : MonoBehaviour
{
    [Header("各塁のUI（Image）")]
    public Image image1st;
    public Image image2nd;
    public Image image3rd;

    [Header("点灯時・消灯時のカラー")]
    public Color offColor = new Color(0.2f, 0.2f, 0.2f, 0.8f); // 消灯（暗いグレー）
    public Color onColor = new Color(1.0f, 0.6f, 0.0f, 1.0f);  // 点灯（オレンジ）

    // ランナーの状態（true: いる, false: いない）
    public bool has1st = false;
    public bool has2nd = false;
    public bool has3rd = false;

    private float timer = 0f;
    private bool isBlinkOn = true;

    void Update()
    {
        // ランナーがいる塁をチカチカ点滅させる演出
        timer += Time.deltaTime * 3f;
        isBlinkOn = Mathf.Sin(timer) > 0;

        // UIの表示更新
        if (image1st != null) image1st.color = has1st ? (isBlinkOn ? onColor : offColor) : offColor;
        if (image2nd != null) image2nd.color = has2nd ? (isBlinkOn ? onColor : offColor) : offColor;
        if (image3rd != null) image3rd.color = has3rd ? (isBlinkOn ? onColor : offColor) : offColor;
    }

    // 単打（シングルヒット / フォアボール）の進塁処理
    public void AdvanceSingle()
    {
        int runScored = 0;

        if (has1st && has2nd && has3rd)
        {
            // 満塁時のヒット/押し出し ➔ 3塁ランナー生還（1点）
            runScored = 1;
        }
        else if (has1st && has2nd)
        {
            has3rd = true;
        }
        else if (has1st)
        {
            has2nd = true;
        }

        has1st = true; // バッターランナーが1塁へ

        // 生還者がいればスコア加算
        CheckAndAddScore(runScored);
    }

    // ツーベースヒットの進塁処理
    public void AdvanceTwoBase()
    {
        int runScored = 0;

        // 3塁ランナーと2塁ランナーが生還
        if (has3rd) runScored++;
        if (has2nd) runScored++;

        // 1塁ランナーは3塁へ移動
        has3rd = has1st;
        has2nd = true; // バッターランナーが2塁へ
        has1st = false;

        // 生還者がいればスコア加算
        CheckAndAddScore(runScored);
    }

    // スコアマネージャーに得点を送る関数
    private void CheckAndAddScore(int runs)
    {
        if (runs > 0)
        {
            ScoreManager scoreMgr = GameObject.FindAnyObjectByType<ScoreManager>();
            if (scoreMgr != null)
            {
                scoreMgr.AddScore(runs);
            }
        }
    }

    // チェンジ・打席リセット時の全ランナークリア
    public void ResetRunners()
    {
        has1st = false;
        has2nd = false;
        has3rd = false;
    }
}