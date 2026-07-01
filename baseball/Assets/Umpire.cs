using UnityEngine;

public class Umpire : MonoBehaviour
{
    // ボールがストライクゾーンを通過したかを記録するフラグ
    private bool hasTouchedStrikeZone = false;

    // 何かがTrigger（透明な判定エリア）に入ったときに自動で呼ばれる関数
    private void OnTriggerEnter(Collider other)
    {
        // ぶつかってきたオブジェクトのタグが "Ball" かどうかチェック
        if (other.CompareTag("Ball"))
        {
            // ぶつかった相手が「ストライクゾーン」だった場合
            if (gameObject.name == "StrikeZone")
            {
                hasTouchedStrikeZone = true;
                Debug.Log("<color=red>★ストライク！★</color>");
            }

            // ぶつかった相手が「キャッチャー壁」だった場合（ここで最終判定）
            if (gameObject.name == "CatcherWall")
            {
                // 先にストライクゾーンに触れていたか確認
                // キャッチャー壁自身ではなく、ストライクゾーン側のスクリプトの状態を見に行く
                Umpire zoneUmpire = GameObject.Find("StrikeZone").GetComponent<Umpire>();

                if (zoneUmpire != null && zoneUmpire.hasTouchedStrikeZone)
                {
                    // すでにストライク判定が出ているので、ここでは何もしない（リセットだけ）
                    zoneUmpire.hasTouchedStrikeZone = false;
                }
                else
                {
                    // ストライクゾーンを通らずにここに届いた＝ボール
                    Debug.Log("<color=green>◇ボール！◇</color>");
                }

                // 判定が終わったので、ボールを画面から消去する
                Destroy(other.gameObject);
            }
        }
    }
}