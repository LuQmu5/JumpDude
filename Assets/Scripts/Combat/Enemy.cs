using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public void Kill()
    {
        Destroy(gameObject);
    }
}