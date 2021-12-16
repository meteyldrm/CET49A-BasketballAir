using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopController: MonoBehaviour {
    public int totalScore = 0;
    public int ballNumber;

    [SerializeField]
    private TextMeshProUGUI BasketsTMP;
        
    [SerializeField]
    private TextMeshProUGUI ScoreTMP;
    
    [SerializeField]
    private TextMeshProUGUI BestScoreTMP;

    private void Start() {
        Application.targetFrameRate = 60;

        updateBest();
    }

    public void restartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onBasket(int score) {
        if (score != 0) {
            totalScore += score;
            updateUI();
        }
    }

    public void updateUI() {
        BasketsTMP.text = $"{ballNumber}";
        ScoreTMP.text = $"Score: {totalScore}";
    }
    
    public void updateBest() {
        BestScoreTMP.text = $"Best: {PlayerPrefs.GetInt("best")}";
    }
}