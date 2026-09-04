using UnityEngine;

public class PitcherCursor : MonoBehaviour
{
    [Header("--- 移動スピードと制限範囲 ---")]
    public float moveSpeed = 300f; // カーソルの移動速度
    public float minX = -150f;     // 移動範囲（左限界）
    public float maxX = 150f;      // 移動範囲（右限界）
    public float minY = -100f;     // 移動範囲（下限界）
    public float maxY = 100f;      // 移動範囲（上限界）

    [Header("--- ターゲット基準 ---")]
    [Tooltip("ヒエラルキーにある StrikeZone オブジェクトをセットしてください")]
    public Transform strikeZoneTarget;

    private RectTransform rectTransform;
    private Camera mainCamera;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 矢印キー（↑↓←→）の直接入力判定
        Vector2 move = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) move.x += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
        if (Input.GetKey(KeyCode.UpArrow)) move.y += 1f;

        // 位置の更新
        Vector2 currentPos = rectTransform.anchoredPosition;
        currentPos += move.normalized * moveSpeed * Time.deltaTime;

        // ゾーン外に行き過ぎないよう制限
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        rectTransform.anchoredPosition = currentPos;
    }

    // 投球用にUIカーソルの位置を3D空間のストライクゾーン平面の位置に正確に変換する関数
    public Vector3 GetTargetWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // UIカーソルの画面位置から3D空間に飛ばすレイ（光線）を作成
        Ray ray = mainCamera.ScreenPointToRay(transform.position);

        // StrikeZoneが指定されていれば、そのZ位置（奥行き）を基準にする
        float targetZ = 0f;
        if (strikeZoneTarget != null)
        {
            targetZ = strikeZoneTarget.position.z;
        }
        else
        {
            // なければ名前で探す
            GameObject sz = GameObject.Find("StrikeZone");
            if (sz != null) targetZ = sz.transform.position.z;
        }

        // カメラからStrikeZoneまでの奥行き距離を計算
        float distance = Mathf.Abs(targetZ - mainCamera.transform.position.z);
        if (distance <= 0.1f) distance = 10f; // 安全策

        // レイを伸ばして、ちょうどストライクゾーンの高さでの3D座標を取得
        return ray.GetPoint(distance);
    }
}