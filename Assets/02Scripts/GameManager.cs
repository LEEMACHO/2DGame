using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public CameraCtr cameraCtr;


    private void Awake()
    {
        instance = this;
    }

    public void PlayerSpawn(Vector3 vec)
    {
        player.transform.position = vec;
    }

    public void CameraSet(Vector3 vec)
    {
        cameraCtr.GetComponent<CameraCtr>().center = vec;
    }

}
