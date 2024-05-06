using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        player.Init();

        player.gameObject.SetActive(true);
        cameraCtr.gameObject.SetActive(true);
    }

    void Init()
    {

    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void Respawn()
    {
        cameraCtr.center = stages[stageIndex].transform.GetChild(0).position;

        player.transform.position = stages[stageIndex].transform.GetChild(1).position;
    }

    public void Nextstage()
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
        StartCoroutine(GameOverRoutine());
        Debug.Log("Game Over");
        Retry();
    }

    IEnumerator GameOverRoutine()
    {

        yield return new WaitForSeconds(1f);

        MenuObj.SetActive(true);
        MenuPanel.SetActive(true);

        player.gameObject.SetActive(false);
        cameraCtr.gameObject.SetActive(false);
    }

}