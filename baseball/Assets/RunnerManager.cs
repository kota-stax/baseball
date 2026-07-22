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

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // ランナーがいる塁をチカチカ点滅させる演出
        timer += Time.deltaTime * 3f; // 点滅スピード
        isBlinkOn = Mathf.Sin(timer) > 0;

        // UIの表示更新
        if (image1st != null) image1st.color = has1st ? (isBlinkOn ? onColor : offColor) : offColor;
        if (image2nd != null) image2nd.color = has2nd ? (isBlinkOn ? onColor : offColor) : offColor;
        if (image3rd != null) image3rd.color = has3rd ? (isBlinkOn ? onColor : offColor) : offColor;
    }

    // 単打（シングルヒット / フォアボールなど）の進塁処理
    public void AdvanceSingle()
    {
        // 押し出し判定（満塁なら得点）
        if (has1st && has2nd && has3rd)
        {
            Debug.Log("<color=orange>押し出し1点追加！</color>");
        }
        else if (has1st && has2nd)
        {
            has3rd = true; // 1,2塁なら満塁に
        }
        else if (has1st)
        {
            has2nd = true; // 1塁なら1,2塁に
        }

        has1st = true; // バッターランナーが1塁へ
        UpdateUI();
    }

    // ツーベースヒットの進塁処理
    public void AdvanceTwoBase()
    {
        // 2塁・3塁ランナーは生還（得点）
        // 1塁ランナーは3塁へ
        has3rd = has1st;
        has2nd = true; // バッターランナーが2塁へ
        has1st = false;
        UpdateUI();
    }

    // チェンジ・打席リセット時の全ランナークリア
    public void ResetRunners()
    {
        has1st = false;
        has2nd = false;
        has3rd = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        // 点滅ロジックがあるのでUpdate内で処理
    }
}