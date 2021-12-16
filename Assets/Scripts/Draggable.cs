using System;
using Resources;
using Unity.Mathematics;
using UnityEngine;

public class Draggable : MonoBehaviour {
    private Rigidbody rb;
    
    [NonSerialized] public bool canDrag;
    private bool dragging;
    private Vector3 dragOffset = Vector3.zero;

    private Vector3 currentPos = Vector3.zero;
    private Vector3 previousPos = Vector3.zero;

    private float lerpTime;
    
    private Vector3 screenVector;

    private float touchRadius;

    [NonSerialized] public float timeMultiplier = 0f;

    [SerializeField] private PhysicMaterial BallPhysicsMat;
    [SerializeField] private PhysicMaterial NoPhysicsMat;

    private SphereCollider sCollider;

    private AudioController _audioController;
    
    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        _audioController = gameObject.GetComponent<AudioController>();
        sCollider = GetComponent<SphereCollider>();
    }

    private void OnDisable() {
        lerpTime = 0;
    }

    private void OnEnable() {
        if (rb == null) {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        if (_audioController == null) {
            _audioController = gameObject.GetComponent<AudioController>();
        }

        if (sCollider == null) {
            sCollider = GetComponent<SphereCollider>();
        }
    }

    private void FixedUpdate() {
        previousPos = currentPos;
        currentPos = rb.position;
        dragOffset = Vector3.Lerp(dragOffset, Vector3.zero, lerpTime);
        lerpTime += Time.fixedDeltaTime / 25f;
    }

    private void Update() {
        if (canDrag && dragging) {
            rb.velocity = (screenVector - currentPos) / (Time.fixedDeltaTime * 5);
            timeMultiplier = 1;
        }

        if (!dragging) {
            if(timeMultiplier < 1.5f){
                timeMultiplier += 1.2f * Time.deltaTime;
            }
        }
    }

    public bool interceptTouch(Vector3 spaceVector) {
        if (canDrag) {
            return math.abs(Vector3.Distance(transform.position, spaceVector)) < touchRadius;
        }

        return false;
    }

    public void setRadius(float radius) {
        touchRadius = radius;
    }

    public void SetInteractionState(TouchPhase iState, Vector3 spaceVector) {
        switch (iState) {
            case TouchPhase.Began: {
                var position = transform.position;
                dragOffset = spaceVector - position;
                screenVector = position - dragOffset;
                doDrag(true);
                _audioController.playGrabSound();
                sCollider.material = NoPhysicsMat;
                break;
            }
            case TouchPhase.Ended:
                dragOffset = spaceVector;
                screenVector = spaceVector;
                doDrag(false);
                sCollider.material = BallPhysicsMat;
                break;
        }
    }

    public void SetTransformGoal(Vector3 goal) {
        screenVector = goal - dragOffset;
    }

    private void doDrag(bool state) {
        if (state) {
            dragging = true;
        } else {
            try {
                rb.velocity = (currentPos - previousPos) / Time.fixedDeltaTime;
                dragging = false;
            } catch (NullReferenceException) {
            }
        }
    }
}
