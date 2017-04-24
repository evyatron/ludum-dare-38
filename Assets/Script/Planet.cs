using System.Collections.Generic;
using UnityEngine;
using Octogear;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public Transform RingsParent;
    public MaterialTextureSetter CurrentRingShadowTextureSetter;
    public SpriteShadow CurrentRingShadow;
    public Rotator Rotator;
    public Tweener RotatorTweener;
    public Tweener DamageTweener;
    public GameObject RingPrefab;
    public int RingsOnScreen = 3;

    [Header("Rotations")]
    public int Levels = 20;
    public RandomRangeFloat RotationSpeed = new RandomRangeFloat(10.0f, 30.0f);

    [Header("Clouds")]
    [Range(0.0f, 1.0f)]
    public float ChanceToBeOnFront = 0.5f;
    public int NumberOfClouds = 20;
    public RandomRangeFloat CloudsHeight = new RandomRangeFloat(1.0f, 2.0f);
    public float MaxCloudsHeight = 2.0f;
    public GameObject CloudPrefab;
    public Transform CloudsParent;

    [Header("Rotations")]
    public Text LevelNumber;

    [Header("Spiral")]
    public SpiralMesh Spiral;
    public float SpiralScaleSpeed = 0.01f;

    public Ring CurrentRing { get; private set; }

    public float CurrentRadius
    {
        get
        {
            Ring currentRing = CurrentRing;

            if (!Application.isPlaying)
            {
                currentRing = GetComponentInChildren<Ring>();
            }

            if (currentRing.Art.activeInHierarchy)
            {
                return currentRing.Surface;
            }
            else
            {
                return (Spiral.MaxRadius + Spiral.Thickness * 0.5f) * Spiral.transform.localScale.x;
            }
        }
    }

    private List<Ring> rings = new List<Ring>();

    void Awake()
    {
        rings = new List<Ring>(GetComponentsInChildren<Ring>());
    }

    void Start()
    {
        for (int i = 0; i < RingsOnScreen - 1; i++)
        {
            CreateChildRing(rings[i]);
        }

        BeginRing(rings[0]);

        GenerateClouds();

        Tutorial.Instance.DoStep(Tutorial.kTutorialIntro);
    }

    void Update()
    {
        CurrentRingShadow.ReferenceTransform = CurrentRing.ScaleAnchor;

        if (Application.isPlaying)
        {
            if (Spiral != null)
            {
                Spiral.transform.localScale += Vector3.one * SpiralScaleSpeed * Time.deltaTime;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * CurrentRadius);
    }

    public void FinishLevel()
    {
        CurrentRing.Finish();

        int currentRingIndex = rings.IndexOf(CurrentRing);
        Ring nextRing = rings[currentRingIndex + 1];

        BeginRing(nextRing);
    }

    public void BeginRing(Ring ring)
    {
        int ringIndex = rings.IndexOf(ring);

        if (ringIndex == 1)
        {
            Tutorial.Instance.DoStep(Tutorial.kTutorialEnd);
        }

        CurrentRing = ring;

        LevelNumber.text = (ringIndex + 1).ToString();

        CreateChildRing(rings[rings.Count - 1]);

        CurrentRing.Begin();

        CurrentRing.RotateSpeed = RotationSpeed.GetSpecific((float)ringIndex / Levels);

        SetSpeed(CurrentRing.RotateSpeed, true);

        CurrentRingShadowTextureSetter.SetSprite(CurrentRing.RandomTexture.Sprite);
    }

    public void CreateChildRing(Ring parent)
    {
        GameObject ringObject = Instantiate(RingPrefab, parent.ScaleAnchor);
        Ring ring = ringObject.GetComponent<Ring>();

        ring.LevelIndex = rings.Count;

        rings.Add(ring);
    }

    public void SetSpeed(float speed, bool shouldAnimate)
    {
        if (shouldAnimate)
        {
            RotatorTweener.Stop();
            RotatorTweener.ProgressScaleMin = Rotator.RotationSpeed;
            RotatorTweener.ProgressScaleMax = speed;
            RotatorTweener.Play();
        }
        else
        {
            if (!RotatorTweener.IsRunning)
            {
                Rotator.RotationSpeed = speed;
            }
        }
    }

    [ContextMenu("Create Clouds")]
    private void GenerateClouds()
    {
        while (CloudsParent.childCount > 0)
        {
            DestroyImmediate(CloudsParent.GetChild(0).gameObject);
        }

        for (int i = 0; i < NumberOfClouds; i++)
        {
            GameObject cloud = Instantiate(CloudPrefab, CloudsParent);
            cloud.transform.localPosition = Vector3.forward * (Random.Range(0.0f, 1.0f) > ChanceToBeOnFront? 2.0f : -2.0f) * Random.Range(0.9f, 1.1f);
            cloud.transform.GetChild(0).localPosition = Vector3.up * (CurrentRadius + CloudsHeight.Generate());

            cloud.GetComponent<Rotator>().RotationSpeed = Random.Range(-2.0f, 2.0f);

            cloud.GetComponentInChildren<RandomAroundPlanet>().Position();
            cloud.GetComponentInChildren<RandomTexture>().SetRandom();
        }
    }
}