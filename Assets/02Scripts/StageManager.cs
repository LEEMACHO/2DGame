using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public GameObject[] stages;

    private void Awake()
    {
        instance = this;
    }

    public void Retry()
    {
        GameManager.instance.cameraCtr.center = stages[0].transform.GetChild(0).position;

        GameManager.instance.player.transform.position = stages[0].transform.GetChild(1).position;

        Debug.Log("tets");
 
    }

    public void PlayerSpawn()
    {
        int ran = Random.Range(1, stages.Length);

        GameManager.instance.cameraCtr.center = stages[ran].transform.GetChild(0).position;

        GameManager.instance.player.transform.position = stages[ran].transform.GetChild(1).position;

        Debug.Log(ran);

    }


}
