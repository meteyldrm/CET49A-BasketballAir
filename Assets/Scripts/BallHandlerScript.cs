using System;
using System.Collections;
using Resources;
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

    private ObjectPool pool;

    private void Start() {
        pool = gameObject.GetComponent<ObjectPool>();
        StartCoroutine(ballSpawnerCoroutine(spawnFrequency));
    }

    private void spawnBall() {
        //Constrain position to x axis
        //Constrain rotation to z axis?

        var position = transform.position;
        Vector3 positionVector = new Vector3(Random.Range(-2f, -0.5f), position.y + 0.5f, position.z);
        GameObject ball = pool.GetFromPool();
        Rigidbody ballRB = ball.GetComponent<Rigidbody>();
        ball.transform.position = positionVector;
        ball.transform.rotation = new Quaternion(0, 0, 1, 0);
        ballRB.velocity = Vector3.zero;
        var TempVec = new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), Random.Range(-25f, 25f));
        var rand = Random.Range(1, 3);
        ballRB.angularVelocity = TempVec * rand;
        ballRB.AddForce((new Vector3(Random.Range(-0.1f, 0.1f), 0.3f, 0)), ForceMode.Impulse);
    }

    IEnumerator ballSpawnerCoroutine(float time) {
        yield return new WaitForSeconds(time);
        spawnBall();
        StartCoroutine(ballSpawnerCoroutine(Random.Range(spawnFrequency/3, spawnFrequency)));
    }
}