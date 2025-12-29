using UnityEngine;

public class DestructibleWall : MonoBehaviour, InterfaceEnemy
{
    [SerializeField] protected float health;
    protected bool isDestructed = false;

    void Update()
    {
        if (health <= 0)
        {
            isDestructed = true;
            Destroy(gameObject);
        }
    }
    
    public virtual void Damage(int _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
    }
}
