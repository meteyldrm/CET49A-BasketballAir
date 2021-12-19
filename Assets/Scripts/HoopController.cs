using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoopController: MonoBehaviour {
    public int totalScore = 0;
    public int ballNumber;
    private int bestScore = 0;

    private Coroutine basketScoreCoroutine;

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

    public void mainScene() {
        SceneManager.LoadScene("MainScene");
    }

    public void onBasket(int score) {
        if (score != 0) {
            totalScore += score;
            updateUI();
        }
        if(basketScoreCoroutine != null) StopCoroutine(basketScoreCoroutine);
        basketScoreCoroutine = StartCoroutine(basketScoreAnimationCoroutine(0.6f, -25f));
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

    IEnumerator basketScoreAnimationCoroutine(float time, float distance) { 
        var t = ScoreTMP.rectTransform;

        var delta = 0f;
        
        var z = distance;
        t.anchoredPosition3D = new Vector3(0, -40, z);

        while (delta < time) {
            z = Mathf.Lerp(z, 0, delta / time);
            t.anchoredPosition3D = new Vector3(0, -40, z);
            delta += Time.deltaTime;
            yield return null;
        }
        t.anchoredPosition3D = new Vector3(0, -40, 0);
    }
}