using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player           player;
    public CameraCtr        cameraCtr;

    private void Awake()
    {
        instance = this;
    }



}
