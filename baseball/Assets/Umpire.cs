using UnityEngine;

public class Umpire : MonoBehaviour
{
    private bool hasTouchedStrikeZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // BSOManagerを探しておく
            BSOManager bso = GameObject.FindAnyObjectByType<BSOManager>();

            if (gameObject.name == "StrikeZone")
            {
                hasTouchedStrikeZone = true;
                Debug.Log("<color=red>★ストライク！★</color>");

                // 【追加】ストライクゾーンを通過した瞬間、ストライクカウントを1増やす
                if (bso != null)
                {
                    bso.AddStrike();
                }
            }

            if (gameObject.name == "CatcherWall")
            {
                Umpire zoneUmpire = GameObject.Find("StrikeZone").GetComponent<Umpire>();

                if (zoneUmpire != null && zoneUmpire.hasTouchedStrikeZone)
                {
                    // ストライクゾーンをすでに通っている場合、ここではカウントは増やさない（リセットだけ）
                    zoneUmpire.hasTouchedStrikeZone = false;
                }
                else
                {
                    Debug.Log("<color=green>◇ボール！◇</color>");

                    // 【追加】ストライクゾーンを通らずにキャッチャー壁に当たった＝ボールカウントを1増やす
                    if (bso != null)
                    {
                        bso.AddBall();
                    }
                }

                // キャッチャーがボールを受け取ったので、ピッチャーの投球制限を解除する
                Pitcher pitcher = GameObject.Find("Capsule")?.GetComponent<Pitcher>();
                if (pitcher == null)
                {
                    // ピッチャーのオブジェクト名が「Batter」の子オブジェクトなど別名になっている場合の対策
                    pitcher = GameObject.FindAnyObjectByType<Pitcher>();
                }

                if (pitcher != null)
                {
                    pitcher.CancelInvoke("ResetPitching"); // 3秒の自動消滅タイマーを止める
                    pitcher.ResetPitching();               // 次の球を投げられるようにリセット
                }

                Destroy(other.gameObject);
            }
        }
    }
}