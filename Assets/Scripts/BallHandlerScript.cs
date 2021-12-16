using System.Collections;
using Resources;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallHandlerScript : MonoBehaviour {
    /*
     *Calculate angleRange based on position, assume angleRange is based on zero vector
     * 
     *positionRange to anglePivot: calculate the slope based on xy coords
     * The perpendicular distance from the pivot is angleRange
     * Low range, high pivot is desirable
     */

    [SerializeField] private float spawnFrequency; //Upper boundary of the random ball spawn frequency
    [SerializeField] private float ballTouchRadius = 2;
    [SerializeField] public int ballNumber = 30;
    
    private int trackerBallNumber = 30;

    [SerializeField] private GameObject HoopControllerObject;
    private HoopController _hoopController;

    [SerializeField] private GameObject GameOverUI;
    
    private InstancePool pool;

    private bool touchDeadlock;
    private Draggable draggingBall;

    private Camera cam;

    private const int normalAmount = 40;
    private const int cold1Amount = 32;
    private const int cold2Amount = 17;
    private const int cold3Amount = 10;

    private bool doSpawn = true;

    private void Start() {
        cam = Camera.main;
        _hoopController = HoopControllerObject.GetComponent<HoopController>();

        trackerBallNumber = ballNumber;
        _hoopController.ballNumber = ballNumber;
        updateUI();

        pool = gameObject.GetComponent<InstancePool>();
        touchDeadlock = false;

        for (var i = 0; i < transform.childCount; i++) {
            var ball = transform.GetChild(i).gameObject;
            ball.GetComponent<Draggable>().setRadius(ballTouchRadius * 0.12f);
            pool.AddToPool(ball);
        }

        StartCoroutine(ballSpawnerCoroutine(0.5f));
    }

    private void Update() {
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);
            var spaceVector = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, math.abs(cam.transform.position.z)));

            if (touch.phase == TouchPhase.Began) {
                foreach (GameObject ball in pool) {
                    if (!touchDeadlock) {
                        if (!ball.activeSelf) {
                            continue;
                        }
                        draggingBall = ball.GetComponent<Draggable>();
                        touchDeadlock = draggingBall.interceptTouch(spaceVector);
                        if (touchDeadlock) {
                            draggingBall.SetInteractionState(TouchPhase.Began, spaceVector);
                        }
                    } else {
                        break;
                    }
                }
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
                draggingBall.SetTransformGoal(spaceVector);
            }

            if (touch.phase == TouchPhase.Ended) {
                draggingBall.SetInteractionState(TouchPhase.Ended, Vector3.zero);
                touchDeadlock = false;
            }
        }
    }

    private float spawnBall() {
        if (ballNumber < 1) {
            doSpawn = false;
            return 10000;
        }
        
        var position = transform.position;
        var positionVector = new Vector3(Random.Range(-1f, -0.6f), position.y + 0.5f, position.z);

        var ball = pool.GetFromPool();
        var ballRB = ball.GetComponent<Rigidbody>();
        var ballScript = ball.GetComponent<BallScript>();

        float addTime = 0;

        int state = Random.Range(0, normalAmount + cold1Amount + cold2Amount + cold3Amount);
        if (state < normalAmount) {
            ballScript.setIceState(0);
        } else if (state < normalAmount + cold1Amount) {
            ballScript.setIceState(1);
            addTime = 0.3f;
        } else if (state < normalAmount + cold1Amount + cold2Amount) {
            ballScript.setIceState(2);
            addTime = 0.5f;
        } else if (state < normalAmount + cold1Amount + cold2Amount + cold3Amount) {
            ballScript.setIceState(3);
            addTime = 0.8f;
        }

        ball.transform.position = positionVector;
        ball.transform.rotation = new Quaternion(0, 0, 1, 0);
        ballRB.velocity = Vector3.zero;

        const float angle = 10f;
        var TempVec = new Vector3(Random.Range(-angle, angle), Random.Range(-angle, angle), Random.Range(-angle, angle));
        var rand = Random.Range(1, 3);
        ballRB.angularVelocity = TempVec * rand;

        ballRB.AddForce(new Vector3(Random.Range(-0.1f, 0.1f), 0.4f, 0), ForceMode.Impulse);
        return addTime;
    }

    public void AddToPool(GameObject basketball) {
        pool.AddToPool(basketball);
        ballNumber--;
        if (ballNumber == 0) {
            gameOver();
        }
        updateUI();
    }

    private void updateUI() {
        _hoopController.ballNumber = ballNumber;
        _hoopController.updateUI();
    }

    private void gameOver() {
        GameOverUI.SetActive(true);
        
        ballNumber = 0;
        int best = PlayerPrefs.GetInt("best");
        if (best < _hoopController.totalScore) {
            PlayerPrefs.SetInt("best", _hoopController.totalScore);
            PlayerPrefs.Save();
        }
        updateUI();
        _hoopController.updateBest();
    }

    private IEnumerator ballSpawnerCoroutine(float time) {
        while (doSpawn) {
            yield return new WaitForSeconds(time);
            if (trackerBallNumber > 0) {
                float addTime = spawnBall();
                float t = spawnFrequency + addTime;
                time = Random.Range(t / 3, t);
                trackerBallNumber--;
            }
        }
        // ReSharper disable once IteratorNeverReturns
    }
}