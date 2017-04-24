using UnityEngine;
using Octogear;

public class DealDamageOnCollision : MonoBehaviour
{
    public int DamageToDeal = 1;
    public bool DestroyOnCollision = true;
    public Tweener KillTweener = null;
    public Tweener DestroyTweener = null;

    public bool IsAlive { get; protected set; }

    void Awake()
    {
        IsAlive = true;

        if (DestroyTweener != null)
        {
            DestroyTweener.Events.OnFinish.AddListener(OnDestroyTweenerFinish);
        }
        if (KillTweener != null)
        {
            KillTweener.Events.OnFinish.AddListener(OnDestroyTweenerFinish);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(DamageToDeal);

            if (DestroyOnCollision)
            {
                Kill(true);
            }
        }
    }

    public void Kill(bool isFromCollision)
    {
        if (IsAlive)
        {
            IsAlive = false;

            if (isFromCollision && KillTweener != null)
            {
                KillTweener.Play();
            }
            else if (!isFromCollision && DestroyTweener != null)
            {
                DestroyTweener.Play();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroyTweenerFinish()
    {
        Destroy(gameObject);
    }
}