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

    private InstancePool pool;

    private bool touchDeadlock;
    private Draggable draggingBall;

    private Camera cam;

    private void Start() {
        cam = Camera.main;

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

    private void spawnBall() {
        var position = transform.position;
        var positionVector = new Vector3(Random.Range(-1f, -0.6f), position.y + 0.5f, position.z);

        var ball = pool.GetFromPool();
        var ballRB = ball.GetComponent<Rigidbody>();
        var ballScript = ball.GetComponent<BallScript>();
        ballScript.setIceState(Random.Range(0, 4));

        ball.transform.position = positionVector;
        ball.transform.rotation = new Quaternion(0, 0, 1, 0);
        ballRB.velocity = Vector3.zero;

        const float angle = 10f;
        var TempVec = new Vector3(Random.Range(-angle, angle), Random.Range(-angle, angle), Random.Range(-angle, angle));
        var rand = Random.Range(1, 3);
        ballRB.angularVelocity = TempVec * rand;

        ballRB.AddForce(new Vector3(Random.Range(-0.1f, 0.1f), 0.4f, 0), ForceMode.Impulse);
    }

    public void AddToPool(GameObject basketball) {
        pool.AddToPool(basketball);
    }

    private IEnumerator ballSpawnerCoroutine(float time) {
        while (true) {
            yield return new WaitForSeconds(time);
            spawnBall();
            time = Random.Range(spawnFrequency / 3, spawnFrequency);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}