using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialTrigger : MonoBehaviour
{
    public string TutorialId = "";

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            Tutorial.Instance.DoStep(TutorialId);
            GetComponentInChildren<Collider>().enabled = false;
        }
    }
}