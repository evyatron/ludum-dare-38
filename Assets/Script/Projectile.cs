using UnityEngine;
using Octogear;

public class Projectile : MonoBehaviour
{
    public BoxCollider boxCollider = null;
    public Rotator Rotator = null;
    public DealDamageOnCollision DamageDealer = null;

    public RandomRangeFloat TimeToLive = new RandomRangeFloat(5.0f, 10.0f);
    public RandomRangeFloat Speed = new RandomRangeFloat(25.0f, 40.0f);

    private float timeToLive = 0.0f;

    void Awake()
    {
        timeToLive = TimeToLive.Generate();
        Rotator.RotationSpeed = Speed.Generate();
    }

    void Update()
    {
        boxCollider.enabled = DamageDealer.IsAlive;

        if (DamageDealer.IsAlive && DamageDealer != null)
        {
            timeToLive -= Time.deltaTime;

            if (timeToLive <= 0.0f)
            {
                DamageDealer.Kill(false);
            }
        }
    }
}