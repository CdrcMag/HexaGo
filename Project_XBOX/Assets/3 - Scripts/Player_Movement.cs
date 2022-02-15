using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    //Rigidbody du joueur
    private Rigidbody2D _rb;

    [Header("Movement settings")]
    //Vitesse de déplacement
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    //Vecteur de déplacement
    private Vector2 movement;
    private float currentRotation;
    

    [Header("Inputs settings")]
    public bool UseKeyboardSettings = true;
    public bool UseControllerSettings = false;


    private void Awake()
    {
        //Récupère le RB
        _rb = GetComponent<Rigidbody2D>();

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        //Keyboard
        if(UseKeyboardSettings)
            HandleInputsKeyboard();

        //Controller
        if (UseControllerSettings)
            HandleInputsController();
    }

    private void FixedUpdate()
    {
        //Gère le mouvement du personnage au clavier
        if(UseKeyboardSettings)
            HandleMovementKeyboard();

        //Gère le mouvement du personnage à la manette
        if(UseControllerSettings)
            HandleMovementController();
    }

    private void HandleInputsKeyboard()
    {
        //Déplacement en X et Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Rotation
        currentRotation += Input.GetAxisRaw("Input_Rotation") * rotationSpeed * -1;
    }

    private void HandleInputsController()
    {
        //Déplacement en X et Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Rotation
        //currentRotation += Input.GetAxisRaw("Input_Rotation_Controller") * RotationSpeed * -1;
    }

    private void HandleMovementKeyboard()
    {
        movement.Normalize();

        _rb.MovePosition(_rb.position + movement * speed * Time.deltaTime);

        if(Input.GetAxisRaw("Input_Rotation") != 0)
            _rb.MoveRotation(currentRotation * Time.deltaTime);
     
    }

    private void HandleMovementController()
    {
        //Movement
        movement.Normalize();
        _rb.MovePosition(_rb.position + movement * speed * Time.deltaTime);

        //Rotation
        float horizontal = Input.GetAxisRaw("Input_Rotation_Controller_Horizontal");
        float vertical = Input.GetAxisRaw("Input_Rotation_Controller_Vertical");
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg - 180;

        if(horizontal != 0)
            transform.rotation = Quaternion.Euler(0, 0, angle);
        //transform.Rotate(new Vector3(0, 0, angle) * Time.deltaTime);

    }


}
