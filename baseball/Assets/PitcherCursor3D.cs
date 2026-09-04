using UnityEngine;

public class PitcherCursor3D : MonoBehaviour
{
    [Header("--- 移動設定 ---")]
    public float moveSpeed = 3f; // 3D空間での移動スピード

    [Header("--- 移動制限（ワールド座標基準） ---")]
    public float minX = -0.35f;
    public float maxX = 0.35f;
    public float minY = 0.3f;
    public float maxY = 1.2f;

    void Update()
    {
        // 矢印キー入力
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) move.x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) move.x += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) move.y -= 1f;
        if (Input.GetKey(KeyCode.UpArrow)) move.y += 1f;

        // ワールド座標で位置を更新
        Vector3 currentPos = transform.position;
        currentPos += move.normalized * moveSpeed * Time.deltaTime;

        // ストライクゾーンのワールド座標に合わせて制限
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        transform.position = currentPos;
    }

    // 投球目標としてそのままのワールド座標を返す
    public Vector3 GetTargetWorldPosition()
    {
        return transform.position;
    }
}