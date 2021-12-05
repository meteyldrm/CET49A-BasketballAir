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

    private InstancePool pool;

    private void Start() {
        pool = gameObject.GetComponent<InstancePool>();

        for (var i = 0; i < transform.childCount; i++) {
            pool.AddToPool(transform.GetChild(i).gameObject);
        }
        
        StartCoroutine(ballSpawnerCoroutine(0));
    }

    private void spawnBall() {
        //Constrain position to x axis
        //Constrain rotation to z axis?

        var position = transform.position;
        var positionVector = new Vector3(Random.Range(-2f, -0.5f), position.y + 0.5f, position.z);
        var ball = pool.GetFromPool();
        var ballRB = ball.GetComponent<Rigidbody>();
        ball.transform.position = positionVector;
        ball.transform.rotation = new Quaternion(0, 0, 1, 0);
        ballRB.velocity = Vector3.zero;
        var TempVec = new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), Random.Range(-25f, 25f));
        var rand = Random.Range(1, 3);
        ballRB.angularVelocity = TempVec * rand;
        ballRB.AddForce((new Vector3(Random.Range(-0.1f, 0.1f), 0.4f, 0)), ForceMode.Impulse);
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