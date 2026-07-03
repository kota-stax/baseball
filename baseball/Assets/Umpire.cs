//判定が出た瞬間にピッチャーの制限を解除するように
using UnityEngine;

public class Umpire : MonoBehaviour
{
    private bool hasTouchedStrikeZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (gameObject.name == "StrikeZone")
            {
                hasTouchedStrikeZone = true;
                Debug.Log("<color=red>★ストライク！★</color>");
            }

            if (gameObject.name == "CatcherWall")
            {
                Umpire zoneUmpire = GameObject.Find("StrikeZone").GetComponent<Umpire>();

                if (zoneUmpire != null && zoneUmpire.hasTouchedStrikeZone)
                {
                    zoneUmpire.hasTouchedStrikeZone = false;
                }
                else
                {
                    Debug.Log("<color=green>◇ボール！◇</color>");
                }

                // 【追加】キャッチャーがボールを受け取ったので、ピッチャーの投球制限を解除する
                Pitcher pitcher = GameObject.Find("Capsule").GetComponent<Pitcher>();
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