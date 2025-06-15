using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public int value = 1;
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(value);
        Destroy(gameObject);
    }
}
//very simple very straightforward haha