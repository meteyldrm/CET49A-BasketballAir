using System;
using Resources;
using UnityEngine;

public class BallScript: MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BallDestroy")) {
            other.GetComponent<ObjectPool>().AddToPool(this.gameObject);
        }
    }
}