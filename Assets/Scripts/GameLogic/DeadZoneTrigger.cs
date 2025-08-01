using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;

    public void Init(GameLoopManager gameLoopManager)
    {
        _gameLoopManager = gameLoopManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _gameLoopManager.HandleLose();
    }
}
