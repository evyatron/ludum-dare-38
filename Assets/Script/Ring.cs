using Octogear;
using UnityEngine;

[ExecuteInEditMode]
public class Ring : MonoBehaviour
{
    public const float kStartingRadius = 0.81f;

    public Transform ScaleAnchor;
    public GameObject Art;
    public GameObject Objects;
    public RandomTexture RandomTexture;
    public Tweener TweenerBegin;
    public Tweener TweenerEnd;
    public GameObject TutorialTriggerPrefab;
    public bool IsRandom = true;

    public float Radius = 1.0f;
    public float RotateSpeed = 10.0f;

    [Header("Door")]
    public GameObject DoorPrefab;
    public GameObject LeverPrefab;
    public Transform DoorParent;

    [Header("Projectiles")]
    public GameObject ProjectilePrefab;
    public Transform ProjectileParent;
    public float ProjectileFrequency;

    [Header("Obstacles")]
    public GameObject ObstaclePrefab;
    public Transform ObstaclesParent;
    public RandomRangeInt ObstaclesCount;

    [Header("Heal")]
    public GameObject HealPrefab;
    public Transform HealParent;
    public RandomRangeInt HealCount;

    public int LevelIndex { get; set; }
    public float SetRadius { set { Radius = value; } }

    public bool IsActive { get; protected set; }
    public float Surface
    {
        get
        {
            if (circleCollider == null)
            {
                circleCollider = ScaleAnchor.GetComponentInChildren<CircleCollider2D>();
            }

            return circleCollider.bounds.extents.x;
        }
    }

    private CircleCollider2D circleCollider;
    private Door door;
    private Door lever;
    private float projectileTimer = 0.0f;

    private void Update()
    {
        transform.localScale = Vector3.one;

        if (ScaleAnchor != null)
        {
            ScaleAnchor.localScale = Vector3.one * Radius;
        }

        if (!Application.isPlaying)
        {
            return;
        }

        if (IsActive)
        {
            if (door != null && lever != null)
            {
                door.IsLocked = lever.IsLocked;
            }

            projectileTimer += Time.deltaTime;

            if (projectileTimer >= ProjectileFrequency)
            {
                SpawnProjectile();
                projectileTimer = 0.0f;
            }
        }
    }

    public void SpawnProjectile()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, ProjectileParent, false);

        if (LevelIndex == 0)
        {
            GameObject projectileTutorial = Instantiate(TutorialTriggerPrefab, projectile.transform, false);
            projectileTutorial.transform.localScale = new Vector3(3.0f, 1.0f, 1.0f);
            projectileTutorial.GetComponent<TutorialTrigger>().TutorialId = Tutorial.kTutorialProjectiles;
        }
    }

    public void Populate()
    {
        if (IsRandom)
        {
            Instantiate(DoorPrefab, DoorParent, false);
            Instantiate(LeverPrefab, DoorParent, false);

            ObstaclesCount.Generate();
            for (int i = 0; i < ObstaclesCount.Value; i++)
            {
                Instantiate(ObstaclePrefab, ObstaclesParent, false);
            }

            HealCount.Generate();
            for (int i = 0; i < HealCount.Value; i++)
            {
                Instantiate(HealPrefab, HealParent, false);
            }
        }
    }

    private void OnInteractWithDoor()
    {
        Planet planet = GetComponentInParent<Planet>();
        planet.FinishLevel();
        door.PlayerIn.FinishLevel();
    }

    public void Begin()
    {
        IsActive = true;

        Populate();

        Door[] doorAndLever = GetComponentsInChildren<Door>();
        if (doorAndLever.Length > 0)
        {
            door = doorAndLever[0];

            if (doorAndLever.Length > 1)
            {
                lever = doorAndLever[1];
            }
        }

        if (door != null)
        {
            door.OnInteract.AddListener(OnInteractWithDoor);
        }

        if (TweenerBegin != null)
        {
            TweenerBegin.Play();
        }
    }

    public void Finish()
    {
        IsActive = false;

        Objects.SetActive(false);
        ProjectileParent.gameObject.SetActive(false);

        if (door != null)
        {
            door.OnInteract.RemoveListener(OnInteractWithDoor);
        }

        if (TweenerEnd == null)
        {
            TurnOff();
        }
        else
        {
            TweenerEnd.Events.OnFinish.AddListener(TurnOff);
            TweenerEnd.Play();
        }
    }

    private void TurnOff()
    {
        Art.SetActive(false);
    }
}