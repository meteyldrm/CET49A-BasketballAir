using System;
using Resources;
using UnityEngine;

public class BallScript: MonoBehaviour {

    private int ballScore = 5;
    private float ballMultiplier = 1f;
    private bool hasEnteredHoop = false;

    private GameObject hoop;
    private HoopController hoopController;

    private void Start() {
        hasEnteredHoop = false;
        this.GetComponent<Draggable>().canDrag = true;
        hoop = GameObject.Find("Hoop");
        if(hoopController == null) hoopController = hoop.GetComponent<HoopController>();
    }

    private void OnEnable() {
        hasEnteredHoop = false;
        this.GetComponent<Draggable>().canDrag = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BallDestroy")) {
            other.GetComponent<ObjectPool>().AddToPool(this.gameObject);
            return;
        }

        if (other.GetComponent<MeshCollider>() != null) {
            if (other.name == "HoopEnter") {
                hasEnteredHoop = true;
            }
            if (other.name == "HoopExit") {
                if (hasEnteredHoop) {
                    //Handle score and basket logic through controller
                    hoopController.onBasket((int)(ballScore * ballMultiplier));
                    this.GetComponent<Draggable>().canDrag = false;
                }
                hasEnteredHoop = false;
            }
        }
    }
}