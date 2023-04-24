using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeButton : Interactable
{
    [SerializeField]
    private GameObject lights;
    [SerializeField]
    private RobotAgent robot;

    [SerializeField]
    private Animator bombAnimator;
    private Animator buttonAnimator;

    [SerializeField]
    Transform[] barricadeParents;

    [SerializeField]
    private BombTimer timer;

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public override void Action()
    {
        GameManager.Instance.SwapMusic();
        lights.SetActive(true);
        buttonAnimator.SetTrigger("activate");
        bombAnimator.SetTrigger("Open");
        StartCoroutine(ActivateRobot());
        StartCoroutine(ActivateTimer());
        GameManager.Instance.taskManager.UpdateTaskCompletion("Do Not Press");
        GameManager.Instance.taskManager.SwapTasksOver(false);
        if (!PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().GetBool("Notepad"))
        {
            StartCoroutine(PlayerController.Instance.UseNotepad());
            GameManager.Instance.taskManager.FindNotepadText();
            StartCoroutine(PlayerController.Instance.UseNotepad());
        }
        else
            GameManager.Instance.taskManager.FindNotepadText();
        GetComponent<Collider>().enabled = false;
        CanInteract = false;
    }

    IEnumerator ActivateRobot()
    {
        yield return new WaitForSeconds(1.0f);
        // TODO: Activate RigidBodies of Boxes
        // TODO: Default robot to lights off and Activate here
        foreach(Transform boxParent in barricadeParents)
        {
            for (int i = 0; i < boxParent.childCount; i++)
            {
                boxParent.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        robot.Activate();
    }

    IEnumerator ActivateTimer()
    {
        yield return new WaitForSeconds(1.3f);
        timer.StartTimer();
        Destroy(this);
    }
}
