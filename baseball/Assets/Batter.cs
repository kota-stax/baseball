using UnityEngine;
using UnityEngine.InputSystem;

public class Batter : MonoBehaviour
{
    public Transform batTransform; // バットのTransformを紐付ける
    public float swingSpeed = 1000f; // スイングの速さ

    private bool isSwinging = false;
    private Quaternion originalRotation;
    private float swingTimer = 0f;

    void Start()
    {
        if (batTransform != null)
        {
            // 最初（構えているとき）のバットの角度を記憶
            originalRotation = batTransform.localRotation;
        }
    }

    void Update()
    {
        // 【入力】新方式・旧方式どちらの「Enterキー（または右クリックなど）」でもスイングできるように二段構え
        bool isSwingPressed = false;
        // 修正後（うしろに丸カッコを閉じます）
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            isSwingPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            isSwingPressed = true;
        }

        // スイング中でなければ、Enterキーでスイング開始
        if (isSwingPressed && !isSwinging)
        {
            isSwinging = true;
            swingTimer = 0f;
        }

        // スイング中の回転処理
        if (isSwinging && batTransform != null)
        {
            swingTimer += Time.deltaTime;

            if (swingTimer < 0.15f)
            {
                // 前半0.15秒でバットを鋭く振る（Y軸を中心に回転）
                batTransform.Rotate(Vector3.up, swingSpeed * Time.deltaTime);
            }
            else if (swingTimer < 0.4f)
            {
                // 後半で元の位置に素早く戻す
                batTransform.localRotation = Quaternion.Slerp(batTransform.localRotation, originalRotation, Time.deltaTime * 10f);
            }
            else
            {
                // スイング終了、完全に元の角度に戻す
                batTransform.localRotation = originalRotation;
                isSwinging = false;
            }
        }
    }
}