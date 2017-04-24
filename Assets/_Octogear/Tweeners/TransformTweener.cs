using UnityEngine;

namespace Octogear
{
    public class TransformTweener : MonoBehaviour
    {
        public Tweener Tweener = null;
        public Transform ReferenceTransform = null;
        public bool DisableOnFinish = false;
        public bool EnableOnStart = false;

        [Header("Position")]
        public bool AnimatePosition = false;
        public TransformTweenerSettings Position = new TransformTweenerSettings();

        [Header("Rotation")]
        public bool AnimateRotation = false;
        public TransformTweenerSettings Rotation = new TransformTweenerSettings();

        [Header("Scale")]
        public bool AnimateScale = false;
        public TransformTweenerSettings Scale = new TransformTweenerSettings();

        void Awake()
        {
            if (Tweener == null)
            {
                Tweener = GetComponent<Tweener>();
            }

            if (ReferenceTransform == null)
            {
                ReferenceTransform = transform;
            }

            Position.type = TransformTweenerSettings.Type.Position;
            Rotation.type = TransformTweenerSettings.Type.Rotation;
            Scale.type = TransformTweenerSettings.Type.Scale;

            Tweener.Events.OnStart.AddListener(OnTweenerStart);
            Tweener.Events.OnStartReverse.AddListener(OnTweenerStartReverse);
            Tweener.Events.OnProgressUpdate.AddListener(UpdateTransform);

            if (DisableOnFinish)
            {
                Tweener.Events.OnFinish.AddListener(OnTweenerFinish);
            }
        }

        void UpdateTransform(float progress)
        {
            SetValues(TransformTweenerSettings.ValueType.Lerped, progress);
        }

        void OnTweenerStart()
        {
            if (EnableOnStart)
            {
                ReferenceTransform.gameObject.SetActive(true);
            }

            SetValues(TransformTweenerSettings.ValueType.From);
        }

        void OnTweenerStartReverse()
        {
            SetValues(TransformTweenerSettings.ValueType.To);
        }

        void OnTweenerFinish()
        {
            if (DisableOnFinish)
            {
                ReferenceTransform.gameObject.SetActive(false);
            }
        }

        void SetValues(TransformTweenerSettings.ValueType valueType, float lerpProgress = 0.0f)
        {
            if (AnimatePosition)
            {
                Position.Set(ReferenceTransform, valueType, lerpProgress);
            }
            if (AnimateRotation)
            {
                Rotation.Set(ReferenceTransform, valueType, lerpProgress);
            }
            if (AnimateScale)
            {
                Scale.Set(ReferenceTransform, valueType, lerpProgress);
            }
        }
    }

    [System.Serializable]
    public class TransformTweenerSettings
    {
        public Transform FromTransform = null;
        public Transform ToTransform = null;
        public Vector3 From = Vector3.zero;
        public Vector3 To = Vector3.zero;
        public Space TransformSpace = Space.World;

        public Type type { get; set; }

        public enum Type
        {
            Position,
            Rotation,
            Scale
        }

        public enum ValueType
        {
            From,
            To,
            Lerped
        }

        public Vector3 GetFrom()
        {
            return FromTransform == null ? From : GetValue(FromTransform);
        }

        public Vector3 GetTo()
        {
            return ToTransform == null ? To : GetValue(ToTransform);
        }

        public Vector3 GetLerped(float progress)
        {
            if (type == Type.Rotation)
            {
                if (FromTransform != null && ToTransform != null)
                {
                    return Quaternion.LerpUnclamped(FromTransform.rotation, ToTransform.rotation, progress).eulerAngles;
                }
            }

            return Vector3.LerpUnclamped(GetFrom(), GetTo(), progress);
        }

        public void Set(Transform transform, ValueType valueType, float lerpProgress = 0.0f)
        {
            Vector3 value = valueType == ValueType.From ? GetFrom() :
                            valueType == ValueType.To ? GetTo() :
                            GetLerped(lerpProgress);

            bool useWorldSpace = TransformSpace == Space.World ||
                                 (valueType == ValueType.From && FromTransform) ||
                                 (valueType == ValueType.To && ToTransform) ||
                                 (valueType == ValueType.Lerped && FromTransform && ToTransform);

            if (type == Type.Position)
            {
                if (useWorldSpace)
                {
                    transform.position = value;
                }
                else
                {
                    transform.localPosition = value;
                }
            }
            else if (type == Type.Rotation)
            {
                if (useWorldSpace)
                {
                    transform.rotation = Quaternion.Euler(value);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(value);
                }
            }
            else if (type == Type.Scale)
            {
                transform.localScale = value;
            }
        }

        Vector3 GetValue(Transform transform)
        {
            return type == Type.Position ? transform.position :
                   type == Type.Rotation ? transform.rotation.eulerAngles :
                   transform.localScale;
        }
    }
}