using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdviceManager : MonoBehaviour
{
    [Header("--- 監督演出用UI ---")]
    public GameObject advicePanel;     // 演出用パネル全体（親オブジェクト）
    public Image adviceImage;          // 画像を表示するImageコンポーネント

    [Header("--- チーム別監督画像（Sprite） ---")]
    public Sprite topTeamAdviceSprite;   // 表（先攻）の監督画像
    public Sprite bottomTeamAdviceSprite;// 裏（後攻）の監督画像

    [Header("--- 表示時間設定 ---")]
    public float displayDuration = 3.0f; // 表示する時間（秒）

    void Start()
    {
        // ゲーム開始時（1回表）に演出を表示
        ShowAdvice(true);
    }

    // 表/裏切り替え時に呼ばれる関数（isTop: true=表 / false=裏）
    public void ShowAdvice(bool isTop)
    {
        if (advicePanel == null || adviceImage == null) return;

        // チームに応じた画像をセット
        if (isTop && topTeamAdviceSprite != null)
        {
            adviceImage.sprite = topTeamAdviceSprite;
        }
        else if (!isTop && bottomTeamAdviceSprite != null)
        {
            adviceImage.sprite = bottomTeamAdviceSprite;
        }

        // コルーチンを開始して一定時間だけ表示
        StartCoroutine(DisplayRoutine());
    }

    private IEnumerator DisplayRoutine()
    {
        // 投球を一時禁止にする
        Pitcher pitcher = GameObject.FindAnyObjectByType<Pitcher>();
        if (pitcher != null)
        {
            pitcher.isPitching = true;
        }

        // パネルを表示
        advicePanel.SetActive(true);

        // 指定した時間待つ
        yield return new WaitForSeconds(displayDuration);

        // パネルを非表示
        advicePanel.SetActive(false);

        // 投球禁止を解除
        if (pitcher != null)
        {
            pitcher.isPitching = false;
        }
    }
}