using UnityEngine;

public interface IBattleUnitListener
{
    public void OnCollision(Transform player = null, Transform enemy = null);

}
