using UnityEngine;

public class SendCollisionToOther : MonoBehaviour
{
    public GameObject Other;

    private void OnTriggerEnter(Collider other)
    {
        Other.SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit(Collider other)
    {
        Other.SendMessage("OnTriggerExit", other, SendMessageOptions.DontRequireReceiver);
    }
}