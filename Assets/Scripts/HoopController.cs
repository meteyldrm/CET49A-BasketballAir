using TMPro;
using UnityEngine;

public class HoopController: MonoBehaviour {
    public int totalScore;
    public int totalBaskets;

    [SerializeField]
    private TextMeshProUGUI BasketsTMP;
        
    [SerializeField]
    private TextMeshProUGUI ScoreTMP;

    private void Start() {
        Application.targetFrameRate = 60;
        
        totalScore = 0;
        totalBaskets = 0;
        updateUI();
    }

    public void onBasket(int score) {
        if (score != 0) {
            totalScore += score;
            totalBaskets += 1;
            updateUI();
        }
    }

    private void updateUI() {
        BasketsTMP.text = $"Baskets Scored: {totalBaskets}";
        ScoreTMP.text = $"Total Score: {totalScore}";
    }
}