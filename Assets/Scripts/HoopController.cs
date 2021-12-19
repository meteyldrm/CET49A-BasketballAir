using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopController: MonoBehaviour {
    public int totalScore = 0;
    public int ballNumber;
    private int bestScore = 0;

    [SerializeField]
    private TextMeshProUGUI BasketsTMP;
        
    [SerializeField]
    private TextMeshProUGUI ScoreTMP;
    
    [SerializeField]
    private TextMeshProUGUI BestScoreTMP;

    private void Start() {
        bestScore = PlayerPrefs.GetInt("best");
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
        ScoreTMP.text = $"{totalScore}";
        if (totalScore > bestScore) {
            bestScore = totalScore;
            updateBest(bestScore);
        }
    }
    
    private void updateBest() {
        BestScoreTMP.text = $"Best: {PlayerPrefs.GetInt("best")}";
    }

    private void updateBest(int number) {
        BestScoreTMP.text = $"Best: {number}";
    }
}