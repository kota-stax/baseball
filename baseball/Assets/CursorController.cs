using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [Header("--- 連動させるバット ---")]
    public Transform batTransform; // ここにヒエラルキーの「Bat」を入れます

    [Header("--- 移動スピード（下げました） ---")]
    public float moveSpeed = 2f; // 最初5fだったのを2fに落としました（インスペクターでも変えられます）

    // ゲーム開始時の初期位置を覚えておく用
    private Vector3 startLocalPosition;
    private Vector3 batStartLocalPosition;

    void Start()
    {
        // カーソルとバットの最初の位置を記憶
        startLocalPosition = transform.localPosition;
        if (batTransform != null)
        {
            batStartLocalPosition = batTransform.localPosition;
        }
    }

    void Update()
    {
        Vector2 inputMovement = Vector2.zero;

        // キーボード入力のチェック
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputMovement.y = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputMovement.y = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputMovement.x = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputMovement.x = 1f;
        }
        else
        {
            inputMovement.x = Input.GetAxisRaw("Horizontal");
            inputMovement.y = Input.GetAxisRaw("Vertical");
        }

        // 入力があったらカーソルを動かす
        if (inputMovement != Vector2.zero)
        {
            Vector3 pos = transform.localPosition;

            pos.x += inputMovement.x * moveSpeed * Time.deltaTime;
            pos.y += inputMovement.y * moveSpeed * Time.deltaTime;

            // 【ストライクゾーン内に制限】
            // スクショのStrikeZoneのサイズ（X:0.7, Y:0.9）の半分を限界値（半径）として設定します
            float limitX = 0.7f / 2f; // 0.35
            float limitY = 0.9f / 2f; // 0.45

            pos.x = Mathf.Clamp(pos.x, startLocalPosition.x - limitX, startLocalPosition.x + limitX);
            pos.y = Mathf.Clamp(pos.y, startLocalPosition.y - limitY, startLocalPosition.y + limitY);

            transform.localPosition = pos;

            // 【バットとの連動】
            // カーソルが初期位置からどれくらい上下左右に動いたかを計算
            float offsetX = pos.x - startLocalPosition.x;
            float offsetY = pos.y - startLocalPosition.y;

            if (batTransform != null)
            {
                // バットの位置をカーソルの動き（ズレ）に合わせて同期させる
                Vector3 batPos = batStartLocalPosition;
                batPos.x = batStartLocalPosition.x + offsetX; // 左右の連動
                batPos.y = batStartLocalPosition.y + offsetY; // 上下の連動
                batTransform.localPosition = batPos;
            }
        }
    }
}