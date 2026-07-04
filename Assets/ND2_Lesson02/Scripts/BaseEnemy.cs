using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //// 👇 GameManagerのInstanceは静的変数なので、GameManager.Instance で直接参照出来る。
        //Debug.Log($"エネミーから { GameManager.Instance } へアクセス。");
        //GameManager.Instance.EnterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // === 初期化メソッド === //
    public void Initialize(Vector2 position)
    {
        transform.position = position;      // 初期位置
    }

    // === 移動メソッド ===
    public void Movement()
    {
        // Translate(移動方向(m) * 時間単位(s))で移動
        transform.Translate(Vector3.left * Time.deltaTime);
    }
}