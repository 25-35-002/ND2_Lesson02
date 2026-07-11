using UnityEngine;

// === 使用するデザインパターン === //
// - シングルトンパターン (Singleton Pattern) -
public class GameManager : MonoBehaviour, IBattleUnitListener, IBulletListener
{
    public static GameManager Instance { get; private set; }

    // === プレハブ用変数 ===
    public BasePlayer playerPrefab;
    public BaseEnemy enemyPrefab;
    public BaseBullet bulletPrefab;

    // === インスタンス用配列 ===
    public BasePlayer[] players;
    public BaseEnemy[] enemies;
    public BaseBullet[] bullets;

    // ゲーム開始からの経過時間
    public float gameTimer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameTimer = 0f;

        // === 配列の初期化 ===
        players = new BasePlayer[1];
        enemies = new BaseEnemy[100];
        bullets = new BaseBullet[100];

        // === プレイヤーを生成 ===
        for (int index = 0; index < players.Length; index++)
        {
            players[index] = Instantiate(playerPrefab);
        }

        // === 敵を生成 ===
        for (int index = 0; index < enemies.Length; index++)
        {
            enemies[index] = Instantiate(enemyPrefab);
        }

        // === 弾丸を生成 ===
        for (int index = 0; index < bullets.Length; index++)
        {
            bullets[index] = Instantiate(bulletPrefab);
        }

        // === プレイヤーを初期化 ===
        for (int index = 0; index < players.Length; index++)
        {
            players[index].Initialize(
                this,
                new Vector2(-3, 0)
            );
        }

        // === 敵を初期化 ===
        for (int index = 0; index < enemies.Length; index++)
        {
            Vector2 randomPos = Vector2.zero;
            randomPos.x = 15;
            randomPos.y = Random.Range(-5f, 5f);

            // 1～60秒のランダムな出現時間
            float randomAppearTime = Random.Range(1f, 60f);

            enemies[index].Initialize(
                randomPos,
                randomAppearTime
            );
        }

        // === 弾丸を初期化 ===
        for (int index = 0; index < bullets.Length; index++)
        {
            bullets[index].Initialize(
                players[0].transform.position,
                Vector3.right
            );
        }
    }

    void Update()
    {
        gameTimer += Time.deltaTime;

        // === プレイヤーを動かす ===
        for (int index = 0; index < players.Length; index++)
        {
            if (players[index].gameObject.activeSelf)
            {
                players[index].Movement();
                players[index].Shot();
            }
        }

        // === 敵を出現させてプレイヤーへ動かす ===
        for (int index = 0; index < enemies.Length; index++)
        {
            // 生存中・非アクティブ・出現時間を超えた場合
            if (enemies[index].isAlive &&
                !enemies[index].gameObject.activeSelf &&
                gameTimer >= enemies[index].appearTime)
            {
                enemies[index].gameObject.SetActive(true);
            }

            // アクティブな敵をプレイヤーへ向かわせる
            if (enemies[index].gameObject.activeSelf)
            {
                enemies[index].Movement(
                    players[0].transform.position
                );
            }
        }

        // === 弾丸を動かす ===
        for (int index = 0; index < bullets.Length; index++)
        {
            if (bullets[index].gameObject.activeSelf)
            {
                bullets[index].Movement();
            }
        }
    }

    // === プレイヤーを登録するメソッド ===
    public void EnterPlayer(BasePlayer player)
    {
        if (players == null || players.Length == 0)
        {
            Debug.Log("プレイヤー配列が初期化されていません。");
            return;
        }

        for (int index = 0; index < players.Length; index++)
        {
            if (players[index] == null)
            {
                players[index] = player;
                return;
            }
        }
    }

    // === 敵を登録するメソッド ===
    public void EnterEnemy(BaseEnemy enemy)
    {
        if (enemies == null || enemies.Length == 0)
        {
            return;
        }

        for (int index = 0; index < enemies.Length; index++)
        {
            if (enemies[index] == null)
            {
                enemies[index] = enemy;
                return;
            }
        }
    }

    // === プレイヤーと敵の当たり判定 ===
    public void OnCollision(
        Transform player = null,
        Transform enemy = null
    )
    {
        if (player == null || !player.gameObject.activeSelf)
        {
            return;
        }

        for (int index = 0; index < enemies.Length; index++)
        {
            if (enemies[index].gameObject.activeSelf)
            {
                float distance = Vector2.Distance(
                    player.position,
                    enemies[index].transform.position
                );

                if (distance <= 1)
                {
                    Debug.Log(
                        $"範囲内！！{player} vs {enemies[index]}"
                    );

                    player.gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    // === 弾丸と敵の当たり判定 ===
    public void OnBulletDamage(Transform bullet)
    {
        if (bullet == null || !bullet.gameObject.activeSelf)
        {
            return;
        }

        for (int index = 0; index < enemies.Length; index++)
        {
            if (enemies[index].gameObject.activeSelf)
            {
                float distance = Vector2.Distance(
                    bullet.position,
                    enemies[index].transform.position
                );

                if (distance <= 1)
                {
                    Debug.Log(
                        $"範囲内！！{bullet} vs {enemies[index]}"
                    );

                    bullet.gameObject.SetActive(false);

                    enemies[index].isAlive = false;
                    enemies[index].gameObject.SetActive(false);

                    return;
                }
            }
        }
    }
}