using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private bool triggered;

    [SerializeField]
    AudioSource doingggSFX;
    #endregion

    #region methods
    private void Awake()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false;
        transform.GetChild(0).GetChild(0).GetComponent<Interactable>().CanInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Character" && !triggered)
        {
            TriggerPlayer();
        }
        else if (!triggered)
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        triggered = true;
        GetComponent<Animator>().SetTrigger("Trigger");

        transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = true;
        transform.GetChild(0).GetChild(0).GetComponent<Interactable>().CanInteract = true;
        transform.GetChild(1).GetComponent<LineRenderer>().enabled = false;
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
    }

    private void TriggerPlayer()
    {
        try
        {
            doingggSFX.Play();
        }
        catch { }
        Trigger();
        PlayerController.Instance.EnableRagdoll();
        PlayerController.Instance.AddRagdollForce(-transform.forward * 50f);
        GameManager.Instance.EnableControls = false;
        Camera.main.GetComponent<CameraController>().FreezeRotation = true;
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(2.0f);
        PlayerController.Instance.ResetCharacter();
        GameManager.Instance.EnableControls = true;
        Camera.main.GetComponent<CameraController>().FreezeRotation = false;
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 15.0f;
    }
    #endregion
}
