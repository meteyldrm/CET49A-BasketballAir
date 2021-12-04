using System;
using TMPro;
using UnityEngine;

public class HoopController: MonoBehaviour {
    public int totalScore = 0;
    public int totalBaskets = 0;

    [SerializeField]
    private TextMeshProUGUI BasketsTMP;
        
    [SerializeField]
    private TextMeshProUGUI ScoreTMP;

    private void Start() {
        totalScore = 0;
        totalBaskets = 0;
        updateUI();
    }

    public void onBasket(int score) {
        totalScore += score;
        totalBaskets += 1;
        updateUI();
    }

    private void updateUI() {
        BasketsTMP.text = $"Baskets Scored: {totalBaskets}";
        ScoreTMP.text = $"Total Score: {totalScore}";
    }
}