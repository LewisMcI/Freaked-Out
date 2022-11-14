using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    private bool started;
    private bool enableControls;
    private float enableControlsTimer = 2.5f;
    private GameObject player;

    public static GameManager Instance;
    #endregion

    #region properties
    public bool EnableControls
    {
        get { return enableControls; }
        set { enableControls = value; }
    }
    public GameObject Player
    {
        get { return player; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!started)
        {
            if (enableControlsTimer <= 0.0f)
            {
                enableControls = true;
                started = true;
            }
            else
                enableControlsTimer -= Time.deltaTime;
        }
    }
    #endregion
}
