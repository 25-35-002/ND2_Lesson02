using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    // === Protected Value === //
    protected Vector2 _inputMoveValue;      // (2次元ベクトル) 移動入力値
    protected float _inputShotValue;        // (小数点数) 攻撃入力値
    protected GameManager _manager;         // (GameManager) ゲーム管理人

    void Start()
    {
        //// GameManagerに自身(BasePlayer)の情報を登録
        //// GameManager.Instance.players = new BasePlayer[] { this };
        //GameManager.Instance.EnterPlayer(this);

        //// 👇 GameManagerのInstanceは静的変数なので、GameManager.Instance で直接参照出来る。
        //Debug.Log($"プレイヤーから {GameManager.Instance} へアクセス。");
        //Debug.Log($"敵の座標は {GameManager.Instance.enemies[0].transform.position} にいる！！");
    }

    // Update is called once per frame
    void Update()
    {
        _manager.OnCollision(player: transform);
    }

    // === 初期化メソッド === //
    public void Initialize(GameManager manager, Vector2 position)
    {
        transform.position = position;      // 初期位置
        _manager = manager;                 // 管理人を覚える
    }

    // === 移動メソッド === //
    public void Movement()
    {
        // Translate(入力値(m) * 時間単位(s))で移動
        transform.Translate(_inputMoveValue * Time.deltaTime);

        Debug.Log($"{transform.name} >> 移動");
    }

    // === 攻撃メソッド === //
    public void Shot()
    {
        Debug.Log($"{transform.name} >> 攻撃");

        // デバイスの入力がない場合は強制終了
        if (_inputShotValue <= 0.5f) return;

        // 管理人から弾丸配列の情報を取得
        BaseBullet[] bullets = _manager.bullets;
        for (int index = 0; index < 100; index++)
        {   // 使ってない弾丸を見つける
            if (bullets[index].gameObject.activeSelf == false)
            {
                bullets[index].Appear(transform.position, Vector3.right);    // 弾丸の出現処理

                break;                      // 繰り返しの強制終了
            }
        }
    }

    // === 移動入力イベント === //
    protected void OnMove(InputValue value)
    {
        Debug.Log($"移動入力値 = {value.Get<Vector2>()}");
        _inputMoveValue = value.Get<Vector2>();     // 値を変数に保存
    }

    // === 攻撃入力イベント === //
    protected void OnShot(InputValue value)
    {
        Debug.Log($"攻撃入力値 = {value.Get<float>()}");
        _inputShotValue = value.Get<float>();       // 値を変数に保存
    }
}