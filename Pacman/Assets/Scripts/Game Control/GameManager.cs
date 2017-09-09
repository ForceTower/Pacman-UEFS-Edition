using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public enum GameState {
        Init, Game, Dead, Win
    }

    public GameState gameState;

    //Life Counter
    public static int Lives = 3;
    //Current Level
    public static int Level = 0;
    //Are the Ghosts Scared?
    public static bool Scared = false;

    public void Win () {
        gameState = GameState.Win;
        print ("You win! Score: " + Score);
    }

    //Store Player's Score
    public static int Score = 0;

    //How long should the ghosts be scared?
    public float scareTime = 8;
    //Increase ghost speed by this amount so the game gets harder on each level
    public float speedIncrease = 0.2f;

    //Singleton implementation. That is important for GameManager Instances
    private static GameManager _Instance;
    public static GameManager Instance {
        get {
            if (_Instance == null) {
                _Instance = GameObject.FindObjectOfType<GameManager> ();
                DontDestroyOnLoad (_Instance);
            }
            return _Instance;
        }
    }

    private GameObject[] ghosts;

    private float timeToCalm;

    private void Awake () {
        //Singleton Implementation
        if (_Instance == null) {
            _Instance = this;
            DontDestroyOnLoad (this);
        } else {
            if (_Instance != this) {
                Destroy (gameObject);
            }
        }
    }

    void Start () {
        gameState = GameState.Init;
        FindGhosts ();
    }

    void Update () {
        if (gameState == GameState.Init) {
            if (Input.anyKeyDown) {
                gameState = GameState.Game;
            }
        }

        else if (gameState == GameState.Game) {
            if (Scared && timeToCalm <= Time.time)
                CalmDownGhosts ();
        }
	}

    private void FindGhosts () {
        ghosts = GameObject.FindGameObjectsWithTag ("Ghost");
    }

    public void CalmDownGhosts() {
        PlayerController.killingSpree = 0;
        Scared = false;

        foreach (GameObject obj in ghosts) {
            obj.GetComponent<GhostController> ().Calm ();
        }
    }

    public void ScareGhosts() {
        Scared = true;

        foreach (GameObject obj in ghosts) {
            obj.GetComponent<GhostController> ().Scare ();
        }

        timeToCalm = Time.time + scareTime;
        print ("Ghosts are scared!");
    }

    public void KillPlayer () {
        print ("Player just got killed, lol");
        gameState = GameState.Dead;
        Lives--;
    }

    public void RestartLevel () {
        CalmDownGhosts ();
        PlayerController.Instance.Reset ();

        foreach (GameObject ghost in ghosts) {
            ghost.GetComponent<GhostController> ().Reset ();
        }

        gameState = GameState.Init;
        ResetVariables ();
    }

    private void ResetVariables() {
        timeToCalm = 0.0f;
        Scared = false;
        PlayerController.killingSpree = 0;
    }

    private void GameOver () {
        print ("Game over");
    }
}
