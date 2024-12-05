using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour {
    public int Score{
        get{
            return score;
        }
        set{
            score = value;
            scoreText.text = $"Score: {score}";
        }
    }
    public int Live{
        get{
            return live;
        }
        set{
            live = value;
            liveText.text = $"Live: {live}";
        }
    }
    private int live;
    private int score;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI liveText;
    [SerializeField]
    private TextMeshProUGUI maxScore;
    [SerializeField]
    private TextMeshProUGUI result;
    [SerializeField]
    private Canvas paneEndGame;
    public void addScore(int s){
        score += s;
        scoreText.text = $"Score: {score}";
    }
    public void EndGame(){
        paneEndGame.gameObject.SetActive(true);
        result.text = "Your score: " + score;
        maxScore.text = "Your max score: " + GetScoreMax();
    }
    private int GetScoreMax(){
        int historyScore = PlayerPrefs.GetInt("score");
        if(score > historyScore){
            PlayerPrefs.SetInt("score", score);
            return score;
        }
        else return historyScore;
    }
    private void Awake() {
        Live = 3;
        Score = 0;
        paneEndGame.gameObject.SetActive(false);
    }
    public void OnReStart(){
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void OnClose(){
        Application.Quit();
    }
}