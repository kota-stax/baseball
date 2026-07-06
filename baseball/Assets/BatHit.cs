using UnityEngine;

public class BatHit : MonoBehaviour
{
    [Header("--- 打球の飛びやすさ ---")]
    public float hitPower = 1.5f;     // 打球の勢い（大きいほど強く飛ぶ）
    public float upwardForce = 5f;    // 打球の上がりやすさ（フライの上がり具合）

    // バット（コライダー）に何かがぶつかった瞬間に自動で呼ばれる関数
    private void OnCollisionEnter(Collision collision)
    {
        // ぶつかった相手のタグが "Ball" だった場合
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                // 1. 【重力対策】無重力を解除して、普通の放物線を描いて落ちるようにする
                ballRb.useGravity = true;

                // 2. 【速度対策】当たった瞬間の反発方向を計算して、強い力を加える
                // バットから見たボールの方向を計算
                Vector3 hitDirection = (collision.transform.position - transform.position).normalized;

                // 少し上向きのベクトルを足して、打球が綺麗に上がるようにする
                hitDirection.y += upwardForce * 0.1f;
                hitDirection = hitDirection.normalized;

                // ボールの速度を一度リセットして、新しい弾き返す速度を与える
                float currentSpeed = collision.relativeVelocity.magnitude;
                ballRb.linearVelocity = hitDirection * (currentSpeed * hitPower);

                Debug.Log("ナイスバッティング！打球速度: " + ballRb.linearVelocity.magnitude);
            }
        }
    }
}