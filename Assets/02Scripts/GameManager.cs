using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager       instance;
    [Header("# Menu Info")]
    [SerializeReference]
    GameObject                      MenuObj;
    public GameObject               MenuPanel;

    [Header("# Start Info")]
    public Player                   player;
    public CameraCtr                cameraCtr;

    [Header("# Stage Info")]
    public GameObject[]             stages;
    [SerializeReference]
    int                             stageIndex;

    public int                      score;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }


    public void GameStart()
    {
        MenuObj.SetActive(false);
        MenuPanel.SetActive(false);

        player.gameObject.SetActive(true);
        cameraCtr.gameObject.SetActive(true);
        player.transform.position = stages[0].transform.GetChild(1).position;
    }

    public void Retry()
    {
        stageIndex = 0;

        cameraCtr.center = stages[stageIndex].transform.GetChild(0).position;

        player.transform.position = stages[stageIndex].transform.GetChild(1).position;



        player.TrapTrigger();
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