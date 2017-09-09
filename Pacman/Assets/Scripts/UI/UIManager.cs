using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public List<Image> lives;

    public Text score;
    public Text level;
    public Text highscore;

    public Canvas gameOverCanvas;
    public Canvas youWinCanvas;

    void Start () {
        gameOverCanvas.enabled = false;
        youWinCanvas.enabled = false;
    }
	
    // Update is called once per frame
    void Update () {
        score.text = "Score\n" + GameManager.Score;
        level.text = "Level\n" + (GameManager.Level + 1);
        highscore.text = "High Score\n" + GameManager.HighScore;
    }

    public void ShowGameOverScreen() {
        gameOverCanvas.enabled = true;
    }

    public void ShowYouWinScreen() {
        youWinCanvas.enabled = true;
    }
}
