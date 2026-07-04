using UnityEngine;

// === 使用するデザインパターン === //
// - シングルトンパターン (Singleton Pattern) -
// このプロジェクト内に、『ただ一つのみ存在するオブジェクト』を作る時に使用するパターン。
// どのオブジェクトからでも即座にアクセスできる！！
public class GameManager : MonoBehaviour, IBattleUnitListener, IBulletListener
{
    // 静的(static)な変数
    // 👇[static]を付けた変数はどこからでも参照（アクセス）できる！！
    public static GameManager Instance { get; private set; }

    // === 各種変数を宣言 === //
    public BasePlayer playerPrefab;     // プレハブ用変数(BasePlayer)
    public BaseEnemy enemyPrefab;       // プレハブ用変数(BaseEnemy)
    public BaseBullet bulletPrefab;     // プレハブ用変数(BaseBullet)

    public BasePlayer[] players;        // インスタンス用変数
    public BaseEnemy[] enemies;         // インスタンス用変数
    public BaseBullet[] bullets;        // インスタンス用変数

    // Start の上位互換 ※Start よりも速く実行される
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;                    // 変数に自身のクラスを代入
        DontDestroyOnLoad(gameObject);    // シーンが変わっても破棄されない設定
    }

    void Start()
    {
        // === 各配列の初期化 === //
        players = new BasePlayer[1];        // プレイヤーを n体 で初期化
        enemies = new BaseEnemy[100];       // 敵を100体で初期化
        bullets = new BaseBullet[100];      // 弾丸を100体で初期化

        // === キャラクターをスポーンさせる ===//
        // indexの初期値を0とし; indexが "1より小さい"ときだけ; indexを1ずつ増やしながら繰り返す
        for (int index = 0; index < 1; index++)
        {
            players[index] = Instantiate(playerPrefab);      // プレハブインスタンスの生成(BasePlayer)
        }

        // indexの初期値を0とし; indexが "100より小さい"ときだけ; indexを1ずつ増やしながら繰り返す
        for (int index = 0; index < 100; index++)
        {
            enemies[index] = Instantiate(enemyPrefab);       // プレハブインスタンスの生成(BaseEnemy)
        }

        // indexの初期値を0とし; indexが "100より小さい"ときだけ; indexを1ずつ増やしながら繰り返す
        for (int index = 0; index < 100; index++)
        {
            bullets[index] = Instantiate(bulletPrefab);      // プレハブインスタンスの生成(BaseBullet)
        }

        // === キャラクターを初期化する === //
        for (int index = 0; index < 1; index++)
        {   // プレイヤー達の初期化
            players[index].Initialize(this, new Vector2(-3, 0));
        }

        for (int index = 0; index < 100; index++)
        {   // 敵はランダムな位置で初期化
            Vector2 randomPos = Vector2.zero;
            randomPos.x = 15;
            randomPos.y = Random.Range(-5f, 5f);

            enemies[index].Initialize(randomPos);
        }

        for (int index = 0; index < 100; index++)
        {
            bullets[index].Initialize(players[0].transform.position, Vector3.right);
        }
    }

    void Update()
    {
        // プレイヤーを全員動かす
        for (int index = 0; index < 1; index++)
        {
            players[index].Movement();
            players[index].Shot();
        }

        // 敵を全員動かす
        for (int index = 0; index < 100; index++)
        {
            enemies[index].Movement();
        }

        // 弾丸を全員動かす
        for (int index = 0; index < 100; index++)
        {
            bullets[index].Movement();
        }
    }

    // === オブジェクトを登録するメソッド === //
    public void EnterPlayer(BasePlayer player)
    {
        // 初期化が出来てなければ、何もしない...
        if (players == null || players.Length == 0)
        {
            Debug.Log("配列の初期化に失敗していますよ...");
            return;     // 強制終了...
        }

        // forループを使って配列の中身を全てチェック
        for (int index = 0; index < players.Length; index++)
        {
            Debug.Log($"配列の {index} 番目の中身 >> {players[index]}");
            if (players[index] == null)
            {
                Debug.Log($"{index}番目に中身がない(Null)ので、{player} を代入します。");
                players[index] = player;

                return;     // 入れたら終了
            }
        }

        Debug.Log($"空きがないので、{player}を設定出来ませんでした...");
    }

    public void EnterEnemy(BaseEnemy enemy)
    {
        if (enemies == null || enemies.Length == 0)
        {
            return;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                enemies[i] = enemy;
                return;
            }
        }
    }
    public void OnCollision(Transform player = null, Transform enemy = null)
    {
        try
        {
            Debug.Log($"衝撃対象　>> {player.name} vs {enemy.name}");
        }
        catch (System.Exception error)
        {
            Debug.Log($"エラーを無視します... >>  \n {error}");
        }

        for(int e = 0; e < enemies.Length; e++)
        {
            if(enemies[e].gameObject.activeSelf)
            {
                float distance = Vector2.Distance(player.transform.position, enemies[e].transform.position);
                if (distance <= 1)
                {
                    Debug.Log($"範囲内！！{player} vs {enemies[e]}");

                    player.gameObject.SetActive(false);
                }

            }
        }
    }

    public void OnBulletDamage(Transform bullet)
    {
        for (int e = 0; e < enemies.Length; e++)
        {

            if (enemies[e].gameObject.activeSelf)
            {
                float distance = Vector2.Distance(bullet.position, enemies[e].transform.position);

                if (distance <= 1)
                {
                    Debug.Log($"範囲内！！{bullet} vs {enemies[e]}");

                    bullet.gameObject.SetActive(false);
                    enemies[e].gameObject.SetActive(false);
                }
            }
        }
    }

}