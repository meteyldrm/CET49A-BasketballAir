using Unity.Mathematics;
using UnityEngine;

public class Draggable : MonoBehaviour {
    private Rigidbody rb;
    
    public bool canDrag = true;
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

    #region Desktop

    private void OnMouseEnter() {
        hasCollisionOverlap = true;
    }

    private void OnMouseExit() {
        hasCollisionOverlap = false;
    }

    private void OnMouseDown() {
        if (canDrag && hasCollisionOverlap) {
            dragging = true;
            doDrag(true);
        }
    }

    private void OnMouseDrag() {
        if (dragging) {
            Vector3 screenVector = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, math.abs(cam.transform.position.z)));
            
            rb.velocity = (screenVector - currentPos) / (Time.fixedDeltaTime * 4);
            
            //transform.position = screenVector;
        }
    }

    private void OnMouseUp() {
        dragging = false;
        doDrag(false);
    }

    #endregion
    
    #region Mobile
    
    
    
    #endregion

    private void doDrag(bool state) {
        if (state) {
            rb.velocity = Vector3.zero;
        } else {
            rb.velocity = (currentPos - previousPos) / Time.fixedDeltaTime;
        }
    }
}
