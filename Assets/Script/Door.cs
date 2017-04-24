using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public ParticleSystem ParticlesUnlock;
    public bool DefaultIsUnlocked = false;

    public Sprite SpriteLocked;
    public Sprite SpriteUnlocked;

    public UnityEvent OnInteract = new UnityEvent();

    public bool IsLocked
    {
        get
        {
            return isLocked;
        }
        set
        {
            if (isLocked != value)
            {
                isLocked = value;
                SpriteRenderer.sprite = isLocked ? SpriteLocked : SpriteUnlocked;

                if (!isLocked && ParticlesUnlock != null)
                {
                    ParticlesUnlock.Play();
                }
            }
        }
    }
    public bool IsPlayerIn { get { return PlayerIn != null; } }
    public PlayerController PlayerIn { get; protected set; }

    private Planet planet;

    private bool isLocked;

    void Awake()
    {
        IsLocked = !DefaultIsUnlocked;
    }

    void Update()
    {
        if (planet == null)
        {
            planet = GetComponentInParent<Planet>();

            if (planet == null)
            {
                return;
            }
        }

        if (IsPlayerIn)
        {
            if (Input.GetButtonDown("Interact"))
            {
                OnInteract.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerIn = other.gameObject.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (player == PlayerIn)
        {
            PlayerIn = null;
        }
    }
}