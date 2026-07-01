using UnityEngine;
// 新方式の入力システムを使うための宣言
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    [Header("--- 紐付け設定 ---")]
    public GameObject ballPrefab;   // 投げるボールのプレハブ
    public Transform releasePoint; // ボールが発射される位置

    [Header("--- 調整パラメータ ---")]
    public float pitchSpeed = 20f;  // 球速（20くらいがおすすめです）

    void Update()
    {
        // 【対策版】新方式と旧方式、どちらのスペースキー入力でも反応するように二段構えにしています
        bool isSpacePressed = false;

        // 1. 新方式（Input System）でのチェック
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isSpacePressed = true;
        }
        // 2. 旧方式（Input Manager）でのチェック（念のため）
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }

        // スペースキーが押されていたら発射
        if (isSpacePressed)
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        // 紐付けチェック
        if (ballPrefab == null || releasePoint == null)
        {
            Debug.LogError("【エラー】Ball Prefab または Release Point がインスペクターで設定されていません！");
            return;
        }

        // ボールを生成
        GameObject ball = Instantiate(ballPrefab, releasePoint.position, Quaternion.identity);

        // ログを出して、プログラムがここを通過したか確認できるようにする
        Debug.Log("球を発射しました！向き: " + transform.forward + " / 速度: " + pitchSpeed);

        // 生成したボールのRigidbodyを取得して飛ばす
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // 重力をオフにしてまっすぐ飛ばす
            rb.linearVelocity = transform.forward * pitchSpeed; // 正面方向に発射
        }
        else
        {
            Debug.LogWarning("【警告】生成されたBallにRigidbodyコンポーネントがついていません！");
        }

        // 3秒後に消去
        Destroy(ball, 3f);
    }
}