using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"エネミーから{GameManager.Instance}へアクセス。");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector2 position)
    {
        transform.position = position;
    }

    public void Movement()
    {
        transform.Translate(Vector3.left * Time.deltaTime);

    }
}
