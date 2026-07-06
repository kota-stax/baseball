//テスト、球連射できないように
using UnityEngine;
using UnityEngine.InputSystem;

public class Pitcher : MonoBehaviour
{
    [Header("--- 紐付け設定 ---")]
    public GameObject ballPrefab;
    public Transform releasePoint;

    [Header("--- 調整パラメータ ---")]
    public float pitchSpeed = 20f;

    // 【追加】今、球がすでに投げられているかどうかを管理するフラグ
    [HideInInspector] // インスペクターには表示させない設定
    public bool isPitching = false;

    void Update()
    {
        // 【追加】すでに球が投げられている（投球中）なら、キー入力を無視する
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

        // 【追加】投げたので「投球中」にする
        isPitching = true;

        GameObject ball = Instantiate(ballPrefab, releasePoint.position, Quaternion.identity);
        Debug.Log("球を発射しました！向き: " + transform.forward + " / 速度: " + pitchSpeed);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = transform.forward * pitchSpeed;
        }
        else
        {
            Debug.LogWarning("【警告】生成されたBallにRigidbodyコンポーネントがついていません！");
        }

        // 念のため、秒経ってもどこにも当たらなかった場合はフラグをリセットして次を投げられるようにする
        Invoke("ResetPitching", 5f);
        Destroy(ball, 5f);
    }

    // 【追加】判定が終わったときに、アンパイアやInvokeから呼ばれる関数
    public void ResetPitching()
    {
        // すでにリセットされていれば何もしない（重複防止）
        if (!isPitching) return;

        isPitching = false;
        Debug.Log("ピッチャーが次の球を投げられるようになりました。");
    }
}