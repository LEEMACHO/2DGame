using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager   instance;

    public Player               player;
    public CameraCtr            cameraCtr;

    [Header("# Stage Info")]
    public GameObject[]         stages;
    public int                  stageIndex;

    private void Awake()
    {
        instance = this;
    }

    public void Retry()
    {
        cameraCtr.center = stages[0].transform.GetChild(0).position;

        player.transform.position = stages[0].transform.GetChild(1).position;

        stageIndex = 0;

    }

    public void PlayerSpawn()
    {
        stageIndex++;
        if (stageIndex < stages.Length)
        {
            cameraCtr.center = stages[stageIndex].transform.GetChild(0).position;

            player.transform.position = stages[stageIndex].transform.GetChild(1).position;
        }

    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

}
