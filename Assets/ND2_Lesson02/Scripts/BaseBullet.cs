using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    // === 変数 === //
    public Vector3 direction;
    public Vector3 startPosition;

    private GameManager _manager;

    void Start()
    {

    }

    void Update()
    {
        _manager.OnBulletDamage(transform);
    }

    // === 初期化メソッド === //
    // 引数1 : 座標
    // 引数2 : 向きベクトル
    public void Initialize(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(false);    // 最初は非表示
        transform.position = position;
        this.direction = direction;

        _manager = GameManager.Instance;
    }

    // === 出現メソッド === //
    public void Appear(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);     // 自身(BaseBullet)を有効化
        transform.position = position;  // 出現座標
        this.direction = direction;     // 向きベクトル
        startPosition = position;       // 生成位置
    }

    // === 移動メソッド === //
    public void Movement()
    {
        // Translate(向きベクトル * 速度 * 時間単位(s))
        transform.Translate(direction * 10 * Time.deltaTime);

        // 範囲外に出たら消滅(10m)
        bool outOfRange = CheckRange(10);
        if (outOfRange == true)
        {
            gameObject.SetActive(false);
        }
    }

    // === 一定距離を求める === //
    // ※ 飛距離チェック
    public bool CheckRange(float range)
    {
        // 生成位置からの距離を求める
        float distance = Vector3.Distance(transform.position, startPosition);

        if (range <= distance)
        {   // 攻撃範囲外に出たら True を返す
            return true;
        }

        // 攻撃範囲内のいるときは False を返す
        return false;
    }
}