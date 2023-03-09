using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : Interactable
{
    #region fields
    [SerializeField]
    private bool electric;
    #endregion

    #region properties
    public bool Electric
    {
        set { electric = value; }
    }
    #endregion

    #region methods
    public override void Action()
    {
        if (electric)
        {
            StartCoroutine(KillPlayer());
            transform.parent.GetComponent<Clothes>().EnableVFX();
        }
        else
        {
            // Disable object
            gameObject.SetActive(false);
            transform.parent.GetComponent<Clothes>().Check();
            AudioManager.Instance.PlayAudio("Cloth");
            // Play grab animation
            Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
            if (!anim.GetBool("Notepad"))
                anim.SetTrigger("Grab");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player" && electric && !PlayerController.Instance.Dead)
        {
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Start player electric fx
        foreach (ParticleSystem effect in PlayerController.Instance.ElectricFX)
        {
            effect.Play();
        }
        yield return new WaitForSeconds(0.2f);
        PlayerController.Instance.Die(0.0f, false, PlayerController.SelectCam.outsideCam);
        // Play animation
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Electrecute");
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, PlayerController.Instance.GetComponent<Rigidbody>().velocity.y, 0.0f);
        // Play sfx
        AudioManager.Instance.PlayAudio("Tase");
        yield return new WaitForSeconds(0.75f);
        // Stop player electric fx
        foreach (ParticleSystem effect in PlayerController.Instance.ElectricFX)
        {
            effect.Stop();
        }
        // Ragdoll
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        PlayerController.Instance.EnableRagdoll();
    }
    #endregion
}
