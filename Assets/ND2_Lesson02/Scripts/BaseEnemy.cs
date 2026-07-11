using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float appearTime; // 出現時間
    public bool isAlive;     // 生存しているか

    // === 初期化メソッド ===
    public void Initialize(Vector2 position, float appearTime)
    {
        transform.position = position;
        this.appearTime = appearTime;
        isAlive = true;

        gameObject.SetActive(false);
    }

    // === プレイヤーへ向かう移動メソッド ===
    public void Movement(Vector2 playerPosition)
    {
        Vector2 direction =
            (playerPosition - (Vector2)transform.position).normalized;

        transform.position +=
            (Vector3)(direction * Time.deltaTime);
    }
}