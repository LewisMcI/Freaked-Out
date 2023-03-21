using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class RobotPunchingGlove : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject constraint;
    [SerializeField]
    private Transform endBone;
    private RaycastHit hit;

    [SerializeField]
    private VisualEffect whamVFX;

    private Vector3 startingPos;
    private bool attack;
    #endregion

    #region methods
    private void Start()
    {
        startingPos = constraint.transform.localPosition;
    }

    private void Update()
    {
        if (attack)
        {
            // Rotate towards player
            Vector3 dir = (PlayerController.Instance.transform.position - transform.root.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, lookRot, 3.5f * Time.deltaTime);

            // Move boxing glove towards player
            constraint.transform.position = Vector3.Lerp(constraint.transform.position, PlayerController.Instance.transform.position, 5f * Time.deltaTime);

            // Collide with objects infront
            if (Physics.SphereCast(endBone.position, 0.5f, endBone.forward, out hit, 0.5f))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.name != "Boxing Glove Rig" && hit.transform.gameObject.layer != 6)
                {
                    attack = false;
                    whamVFX.Play();
                    Debug.Log("COLLIDED");
                }
            }

            // Hit player
            if (Vector3.Distance(constraint.transform.position, PlayerController.Instance.transform.position) <= 0.5f)
            {
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
                attack = false;
                whamVFX.Play();
            }
        }
        else
        {
            // Retract boxing glove towards initial position
            if (constraint.transform.localPosition.y <= -0.0007f)
            {
                constraint.transform.localPosition = startingPos;
            }
            else
            {
                constraint.transform.localPosition = Vector3.Lerp(constraint.transform.localPosition, startingPos, 3f * Time.deltaTime);
            }
        }
    }

    public void Action()
    {
        attack = true;
    }
    #endregion
}