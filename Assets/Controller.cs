using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Transform playerCamera;
    private InputActions controls;
    private Rigidbody rigidbody; 

    [Range(0, 100)]
    public float movementSpeed;
    [Range(0, 100)]
    public float lookSpeed;
    [Range(0, 500)]
    public float jumpForce;
    
    void Awake(){
        controls = new InputActions();
        controls.Player.Jump.performed += _ => OnJump();
        controls.Player.Fire.performed += _ => OnFire();
    }

    void OnEnabled(){
        controls.Enable();
    }
    void OnDisabled(){
        controls.Disable();
    }

    void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
        this.movementSpeed = 5;
        this.lookSpeed = 25;
        this.jumpForce = 110;
        controls.Enable();
    }

    void Update()
    {
        var move = controls.Player.Move.ReadValue<Vector2>();
        var rotationX = controls.Player.RotationX.ReadValue<float>();
        var rotationY = controls.Player.RotationY.ReadValue<float>();

        // Player rotation (Yaw)
        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += rotationY * lookSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(playerRotation);
        
        // Camera rotation (Pitch)
        var cameraRotation = Mathf.Clamp(rotationX * lookSpeed * Time.deltaTime, -90f, 90f);
        playerCamera.Rotate(Vector3.left * cameraRotation);

        // Player movement (Forward)
        var forward = transform.forward * move.y * movementSpeed * Time.deltaTime;
        var right = transform.right * move.x * movementSpeed * Time.deltaTime;
        transform.position += forward + right;
    }

    void OnJump() {
        Debug.Log("Jump!");

        this.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnFire() {
        Debug.Log("Fire!");
    }
}
