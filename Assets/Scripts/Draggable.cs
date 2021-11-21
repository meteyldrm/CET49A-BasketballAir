using UnityEngine;

public class Draggable : MonoBehaviour {
    private Rigidbody rb;
    
    private bool hasCollisionOverlap;
    private bool dragging;

    private Camera cam;

    private Vector3 currentPos = Vector3.zero;
    private Vector3 previousPos = Vector3.zero;
    
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();

        cam = Camera.main;
    }

    private void FixedUpdate() {
        previousPos = currentPos;
        currentPos = rb.position;
    }

    private void OnMouseEnter() {
        hasCollisionOverlap = true;
    }

    private void OnMouseExit() {
        hasCollisionOverlap = false;
    }

    private void OnMouseDown() {
        if (hasCollisionOverlap) {
            dragging = true;
            doDrag(true);
        }
    }

    private void OnMouseDrag() {
        if (dragging) {
            Vector3 screenVector = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4.92f));
            
            transform.position = screenVector;
        }
    }

    private void OnMouseUp() {
        dragging = false;
        doDrag(false);
    }

    private void doDrag(bool state) {
        if (state) {
            rb.velocity = Vector3.zero;
        } else {
            rb.velocity = (currentPos - previousPos) / Time.fixedDeltaTime;
        }
    }
}
