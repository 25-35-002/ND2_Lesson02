using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    protected Vector2 _inputMoveValue;
    protected float _inputShotValue;

    void Start()
    {
        //GameManager.Instance.EnterPlayer(this);

        //Debug.Log($"プレイヤーから{GameManager.Instance}へアクセス。");
        //Debug.Log($"敵の座標は{GameManager.Instance.enemies[0].transform.position}にいる！！");
    }

    void Update()
    {

    }

    public void Initialize(Vector2 position)
    {
        transform.position = position;
    }

    public void Movement()
    {
        transform.Translate(_inputMoveValue * Time.deltaTime);

        Debug.Log($"{transform.name} >>　移動");
    }

    public void shot()
    {
        Debug.Log($"{transform.name}>>攻撃");
    }

    protected void OnMove(InputValue value)
    {
        Debug.Log($"移動入力値 = {value.Get<Vector2>()}");
        _inputMoveValue = value.Get<Vector2>();
    }

    protected void OnShot(InputValue value)
    {
        Debug.Log($"攻撃入力値 = {value.Get<float>()}");
        _inputShotValue = value.Get<float>();
    }
}