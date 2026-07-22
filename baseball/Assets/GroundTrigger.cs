using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private bool hasLanded = false;     // 着地・判定済みか
    private bool isHitByBat = false;    // バットで打たれたか？

    [Header("判定後、ボールが消えるまでの時間（秒）")]
    public float destroyDelay = 5f;

    // バットに当たった時に外部（Batスクリプトなど）から呼ぶ、または衝突で検知
    public void SetHitByBat()
    {
        isHitByBat = true;
    }

    // 1. 特設エリア（Trigger: Hit, TwoBase, Foul, Out等）に入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (hasLanded) return;

        // 特設エリアのタグを持っている場合
        if (other.CompareTag("Hit") || other.CompareTag("TwoBase") || other.CompareTag("Foul") || other.CompareTag("Out"))
        {
            // 打った後だけエリア判定を行う
            if (isHitByBat)
            {
                ProcessResult(other.tag);
            }
        }
    }

    // 2. 普通の地面（マウンドや芝生など）に触れたとき
    private void OnCollisionEnter(Collision collision)
    {
        // もしバットに当たった衝突なら、打球フラグをオンにする
        if (collision.gameObject.name.Contains("Bat") || collision.gameObject.CompareTag("Bat"))
        {
            isHitByBat = true;
            return;
        }

        if (hasLanded) return;

        // ★重要：バットで打たれる前の「投球」が地面やマウンドに触れても無視する！
        if (!isHitByBat) return;

        // キャッチャー壁などに当たった場合は無視
        if (collision.gameObject.name.Contains("Catcher")) return;

        // バットで打った後の球が、どこにも入らず地面に落ちたら「アウト」
        Debug.Log("<color=magenta>どこにも入らなかった！ ➔ ◇◇◇ アウト！ ◇◇◇</color>");
        ProcessResult("Out");
    }

    // 結果を一括で処理する共通関数
    private void ProcessResult(string resultTag)
    {
        hasLanded = true;
        BSOManager bso = GameObject.FindAnyObjectByType<BSOManager>();

        switch (resultTag)
        {
            case "Hit":
                Debug.Log("<color=cyan>★★★ ヒット！ ★★★</color>");
                if (bso != null) bso.Reset打席();
                break;

            case "TwoBase":
                Debug.Log("<color=yellow>★★★ ツーベースヒット！ ★★★</color>");
                if (bso != null) bso.Reset打席();
                break;

            case "Foul":
                Debug.Log("<color=white>◇◇◇ ファウル ◇◇◇</color>");
                break;

            case "Out":
            default:
                Debug.Log("<color=magenta>◇◇◇ アウト！ ◇◇◇</color>");
                if (bso != null)
                {
                    bso.AddOut();
                    bso.Reset打席();
                }
                break;
        }

        Invoke("ResetNextPitch", destroyDelay);
    }

    void ResetNextPitch()
    {
        Pitcher pitcher = GameObject.FindAnyObjectByType<Pitcher>();
        if (pitcher != null)
        {
            pitcher.ResetPitching();
        }
        Destroy(gameObject);
    }
}