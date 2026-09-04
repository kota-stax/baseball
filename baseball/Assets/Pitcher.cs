using UnityEngine;
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    [Header("--- 紐付け設定 ---")]
    public GameObject ballPrefab;
    public Transform releasePoint;

    [Header("--- 調整パラメータ ---")]
    public float pitchSpeed = 20f;

    // 今、球がすでに投げられているかどうかを管理するフラグ
    [HideInInspector]
    public bool isPitching = false;

    void Update()
    {
        // すでに球が投げられている（投球中）なら、キー入力を無視する
        if (isPitching) return;

        bool isSpacePressed = false;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isSpacePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }

        if (isSpacePressed)
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        if (ballPrefab == null || releasePoint == null)
        {
            Debug.LogError("【エラー】Ball Prefab または Release Point が設定されていません！");
            return;
        }

        // 投げたので「投球中」にする
        isPitching = true;

        GameObject ball = Instantiate(ballPrefab, releasePoint.position, Quaternion.identity);

        // ★【変更箇所】 PitcherCursor3D を探して目標方向を計算
        PitcherCursor3D cursor = GameObject.FindAnyObjectByType<PitcherCursor3D>();
        Vector3 throwDirection = transform.forward; // デフォルト（正面）

        if (cursor != null)
        {
            // releasePoint から 3Dカーソルへ向かうベクトルを算出
            Vector3 targetPos = cursor.GetTargetWorldPosition();
            throwDirection = (targetPos - releasePoint.position).normalized;
        }

        Debug.Log("球を発射しました！ 速度: " + pitchSpeed);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = throwDirection * pitchSpeed;
        }
        else
        {
            Debug.LogWarning("【警告】生成されたBallにRigidbodyコンポーネントがついていません！");
        }

        // 5秒経過しても判定が終わらなければ自動リセット
        Invoke("ResetPitching", 5f);
        Destroy(ball, 5f);
    }

    // 判定が終わったときにアンパイアやInvokeから呼ばれる関数
    public void ResetPitching()
    {
        if (!isPitching) return;

        isPitching = false;
        Debug.Log("ピッチャーが次の球を投げられるようになりました。");
    }
}