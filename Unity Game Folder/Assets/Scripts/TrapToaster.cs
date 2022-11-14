using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapToaster : MonoBehaviour
{
    #region fields
    private GameObject knife;
    private bool removed;

    private Animator anim;
    #endregion

    #region methods
    private void Awake()
    {
        knife = transform.GetChild(0).gameObject;
    }

    public void Interact()
    {
        if (knife.transform.IsChildOf(transform))
            Debug.Log("ded");
        else
            Debug.Log("alive");
        // Reset tag
        transform.tag = "Untagged";
    }
    #endregion
}