using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerscript : MonoBehaviour
{
    [SerializeField] GameObject starttxt;
    [SerializeField] GameObject restarttxt;
    [SerializeField] GameObject cleartxt;


    [SerializeField] GameObject ClearZone;
    [SerializeField] GameObject DeadZone;

    [SerializeField] CinemachineVirtualCamera Camera;

    

    public enum GameState { Start, Play, Restart, Clear }

    public GameState curstate;

    private void Awake()
    {   

        curstate = GameState.Start;
        starttxt.SetActive(true);
    }
    private void Start()
    {

    }

    private void Update()
    {
        
        if (curstate == GameState.Start && Input.anyKeyDown == true)
        {
            StartGame();
            Camera.Priority = 9;
        }
        else if (curstate == GameState.Restart && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("PlatformGame");
            curstate = GameState.Play;
        }
        else if (curstate == GameState.Clear && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("PlatformGame");
            curstate = GameState.Play;
        }

    }
    public void Dead()
    {
        curstate = GameState.Restart;
        starttxt.SetActive(false);
        restarttxt.SetActive(true);
        cleartxt.SetActive(false);
    }
    public void Cleared()
    {
        curstate = GameState.Clear;
        starttxt.SetActive(false);
        restarttxt.SetActive(false);
        cleartxt.SetActive(true);
    }
    private void StartGame()
    {
        curstate = GameState.Play;

        starttxt.SetActive(false);
        restarttxt.SetActive(false);
        cleartxt.SetActive(false);
    }

}
