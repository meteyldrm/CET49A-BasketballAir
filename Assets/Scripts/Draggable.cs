using System;
using Unity.Mathematics;
using UnityEngine;

public class Draggable : MonoBehaviour {
    private Rigidbody rb;
    
    public bool canDrag = true;
    private bool dragging;
    private Vector3 dragOffset = Vector3.zero;

    private Camera cam;

    private Vector3 currentPos = Vector3.zero;
    private Vector3 previousPos = Vector3.zero;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();

        cam = Camera.main;
    }

    private void OnDisable() {
        dragging = false;
        dragOffset = Vector3.zero;
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
                RaycastHit hit;
                Physics.Raycast(ray, out hit, 100);

                //TODO Add a child to every basketball and increase its collider size to match finger size.
                
                try {
                    if (canDrag && hit.collider.gameObject == this.gameObject) {
                        dragOffset = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -cam.transform.position.z)) - transform.position;
                        dragOffset.z = 0;
                        dragging = true;
                        doDrag(true);
                    }
                } catch (NullReferenceException) {
                    return;
                }
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
                if (canDrag && dragging) {
                    Vector3 screenVector = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, math.abs(cam.transform.position.z))) - dragOffset;
            
                    rb.velocity = (screenVector - currentPos) / (Time.fixedDeltaTime * 4);
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                dragging = false;
                doDrag(false);
                dragOffset = Vector3.zero;
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
