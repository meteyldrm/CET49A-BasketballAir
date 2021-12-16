using System;
using Resources;
using UnityEngine;

public class BallScript: MonoBehaviour {

    private int ballScore = 5;
    private float ballMultiplier = 1f;
    private bool hasEnteredHoop;

    private int iceState = 0;

    [SerializeField] private Material[] Materials;

    private GameObject hoop;
    private HoopController hoopController;

    private Draggable _draggable;
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    
    private AudioController _audioController;

    private void Start() {
        hasEnteredHoop = false;
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _draggable = GetComponent<Draggable>();
        _audioController = gameObject.GetComponent<AudioController>();
        _draggable.canDrag = true;
        hoop = GameObject.Find("Hoop");
        if(hoopController == null) hoopController = hoop.GetComponent<HoopController>();
    }

    private void OnEnable() {
        hasEnteredHoop = false;
        if(_draggable != null) _draggable.canDrag = true;
        if(_renderer == null) _renderer = GetComponent<MeshRenderer>();
        if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_audioController == null) {
            _audioController = gameObject.GetComponent<AudioController>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BallDestroy")) {
            transform.parent.GetComponent<BallHandlerScript>().AddToPool(this.gameObject);
            return;
        }

        if (other.GetComponent<MeshCollider>() != null) {
            if (other.name == "HoopEnter") {
                hasEnteredHoop = true;
            }
            if (other.name == "HoopExit") {
                if (hasEnteredHoop) {
                    hoopController.onBasket((int)(ballScore * ballMultiplier * _draggable.timeMultiplier * ((1 + _rigidbody.velocity.magnitude) * 0.3f)));
                    _draggable.canDrag = false;
                    _audioController.playNetSound();
                }
                hasEnteredHoop = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (iceState > 1) {
            setIceState(iceState-1);
            ballMultiplier += 0.4f;
            ballScore = 0;
        } else {
            setIceState(0);
            ballScore = 5;
        }
        _audioController.playDribbleSound();
    }

    public void setIceState(int state) {
        _renderer.material = Materials[state];

        if (state > 0) {
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            transform.GetChild(state-1).gameObject.SetActive(true);
        }

        if (state == 0) {
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        iceState = state;
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<MeshCollider>() != null) {
            if (other.name == "HoopEnter") {
                hasEnteredHoop = false;
            }
        }
    }
}