using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] stages;

    private void Awake()
    {
        Vector3 pos = stages[1].transform.GetChild(0).position;
        Debug.Log(pos);
    }

}
