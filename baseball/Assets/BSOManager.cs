using UnityEngine;
using UnityEngine.UI;

public class BSOManager : MonoBehaviour
{
    [Header("--- ボールランプ (3個) ---")]
    public Image[] ballLamps;
    [Header("--- ストライクランプ (2個) ---")]
    public Image[] strikeLamps;
    [Header("--- アウトランプ (2個) ---")]
    public Image[] outLamps;

    // 現在のカウント
    private int ballCount = 0;
    private int strikeCount = 0;
    private int outCount = 0;

    void Start()
    {
        UpdateVisuals(); // 最初はすべて消灯
    }

    // ボールが1つ増える関数
    public void AddBall()
    {
        ballCount++;
        if (ballCount >= 4)
        {
            Debug.Log("フォアボール！");
            ResetInning(); // 本来はランナー進塁ですが、一旦リセット
        }
        UpdateVisuals();
    }

    // ストライクが1つ増える関数
    public void AddStrike()
    {
        strikeCount++;
        if (strikeCount >= 3)
        {
            Debug.Log("三振！アウト！");
            AddOut();
            Reset打席();
        }
        UpdateVisuals();
    }

    // アウトが1つ増える関数
    public void AddOut()
    {
        outCount++;
        if (outCount >= 3)
        {
            Debug.Log("3アウト！チェンジ！");
            outCount = 0;
            ResetInning();
        }
        UpdateVisuals();
    }

    // 打者交替時のリセット（BとSをゼロに）
    public void Reset打席()
    {
        ballCount = 0;
        strikeCount = 0;
        UpdateVisuals();
    }

    // イニング終了時などの全リセット
    public void ResetInning()
    {
        ballCount = 0;
        strikeCount = 0;
        outCount = 0;
        UpdateVisuals();
    }

    // カウントの数に応じてランプの色を塗り替える
    void UpdateVisuals()
    {
        Color darkGray = new Color(0.2f, 0.2f, 0.2f); // 消灯時の色

        // ボールランプの更新 (黄色)
        for (int i = 0; i < ballLamps.Length; i++)
        {
            ballLamps[i].color = (i < ballCount) ? Color.yellow : darkGray;
        }

        // ストライクランプの更新 (青または黄緑)
        for (int i = 0; i < strikeLamps.Length; i++)
        {
            // プロ野球中継風に少し鮮やかな青（シアン）にします
            ballLamps[i].color = (i < ballCount) ? Color.cyan : darkGray;
            strikeLamps[i].color = (i < strikeCount) ? Color.yellow : darkGray;
        }

        // アウトランプの更新 (赤)
        for (int i = 0; i < outLamps.Length; i++)
        {
            outLamps[i].color = (i < outCount) ? Color.red : darkGray;
        }
    }
}