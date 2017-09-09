using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public enum GameState {
        Game, Dead
    }

    //Life Counter
    public static int Lives = 3;
    //Current Level
    public static int Level = 0;
    //Are the Ghosts Scared?
    public static bool Scared = false;
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

    // Use this for initialization
    void Start () {
		
    }
	
    // Update is called once per frame
    void Update () {
		
	}
}
