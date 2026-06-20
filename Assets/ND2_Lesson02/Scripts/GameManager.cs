using UnityEngine;

//＝＝＝　使用するパターン　＝＝＝//
//===シングルトンパターン
//このプロジェクト内に、「ただ一つのみ存在するオブジェクト」を作るときに使用するパターン。
//どのオブジェクトからでも即座にアクセスできる！！  
public class GameManager : MonoBehaviour
{
    //静的（static）な変数
    //👇【static】を付けた変数はどこからでも参照（アクセス）できる！！
    public static GameManager Instance { get; private set; }


    public BasePlayer playerPrefab;
    public BaseEnemy enemyPrefab;

    public BasePlayer[] players;
    public BaseEnemy[] enemies;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        players = new BasePlayer[1];
        enemies = new BaseEnemy[100];


        // ===　キャラクターをスポーンさせる　  === //
        for (int index = 0; index < 1; index++)
        {
            players[index] = Instantiate(playerPrefab);
        }

        for (int index = 0; index < 100; index++)
        {
            enemies[index] = Instantiate(enemyPrefab);
        }

        for (int index = 0; index < 1; index++)
        {
            players[index].Initialize(new Vector2(-3, 0));
        }

        for (int index = 0; index < 100; index++)
        {
            Vector2 randomPos = Vector2.zero;
            randomPos.x = 15;
            randomPos.y = Random.Range(-5f, 5f);

            enemies[index].Initialize(randomPos);
        }
    }

    void Update()
    {
        for (int index = 0; index < 1; index++)
        {
            players[index].Movement();
        }

        for (int index = 0; index < 100; index++)
        {
            enemies[index].Movement();
        }

    }

    public void EnterPlayer(BasePlayer player)
    {
        //初期化が出来てなければ、何もしない
        if (players == null || players.Length == 0)
        {
            Debug.Log("配列の初期化に失敗していますよ...");
            return;      //強制終了
        }

        //配列の中身をすべてチェック
        for (int index = 0; index < players.Length; index++)
        {
            Debug.Log($"配列の{index}番目の中身>>　{players[index]}");

            if (players[index] == null)
            {
                Debug.Log($"中身がない(Null)なので、{player}を代入します。");
                players[index] = player;

                return;
            }
        }

        Debug.Log($"空きがないので、{player}を設定できませんでした。");
    }

    public void EnterEnemy(BaseEnemy enemy)
    {
        //初期化が出来てなければ、何もしない
        if (enemies == null || enemies.Length == 0)
        {
            return;      //強制終了
        }

        //配列の中身をすべてチェック
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                enemies[i] = enemy;

                return;
            }
        }
    }
}