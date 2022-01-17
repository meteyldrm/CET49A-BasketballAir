using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainScene {
    public class MainSceneController : MonoBehaviour {

        [SerializeField] private GameObject BestScoreText;
        
        // Start is called before the first frame update
        void Start() {
            var best = PlayerPrefs.GetInt("best");
            BestScoreText.GetComponent<TextMeshProUGUI>().text = $"Best Score: {best}";
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void onStartClicked() {
            SceneManager.LoadScene("GameScene");
        }

        public void onPrivacyClicked() {
            Application.OpenURL("https://sites.google.com/view/basketball-air-privacy-policy/");
        }
    }
}
