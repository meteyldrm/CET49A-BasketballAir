using System;
using Unity.Mathematics;
using UnityEngine;

public class Draggable : MonoBehaviour {
    private Rigidbody rb;
    
    public bool canDrag = true;
    private bool dragging;

    private Camera cam;

    private Vector3 currentPos = Vector3.zero;
    private Vector3 previousPos = Vector3.zero;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();

        cam = Camera.main;
    }
    private void FixedUpdate() {
        previousPos = currentPos;
        currentPos = rb.position;
    }

    private void Update() {
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                var ray = cam.ScreenPointToRay(touch.position);
                Physics.Raycast(ray, out var hit, 100);
                
                //TODO Add a child to every basketball and increase its collider size to match finger size.
                
                try {
                    if (canDrag && hit.collider.gameObject == this.gameObject) {
                        dragging = true;
                        doDrag(true);
                    }
                } catch (NullReferenceException) {
                    return;
                }
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
                if (dragging) {
                    Vector3 screenVector = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, math.abs(cam.transform.position.z)));
            
                    rb.velocity = (screenVector - currentPos) / (Time.fixedDeltaTime * 4);
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                dragging = false;
                doDrag(false);
            }
        }
    }

    private void doDrag(bool state) {
        if (state) {
            rb.velocity = Vector3.zero;
        } else {
            rb.velocity = (currentPos - previousPos) / Time.fixedDeltaTime;
        }
    }
}
