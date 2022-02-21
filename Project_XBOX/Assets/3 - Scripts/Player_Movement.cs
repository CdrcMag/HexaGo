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
        {
            HandleInputsController();
            HandleRotationController();
        }
            
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

    private void HandleMovementKeyboard()
    {
        movement.Normalize();

        _rb.MovePosition(_rb.position + movement * speed * Time.deltaTime);

        if(Input.GetAxisRaw("Input_Rotation") != 0)
            _rb.MoveRotation(currentRotation * Time.deltaTime);
     
    }

    private void HandleInputsController()
    {
        //Déplacement en X et Y
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Rotation
        //currentRotation += Input.GetAxisRaw("Input_Rotation_Controller") * RotationSpeed * -1;
    }

    private void HandleMovementController()
    {
        //Movement
        movement.Normalize();
        _rb.MovePosition(_rb.position + movement * speed * Time.deltaTime);

    }

    float h;
    float v;
    float angle;
    float s = 0;


    private void HandleRotationController()
    {
        //Rotation
        h = Input.GetAxisRaw("Input_Rotation_Controller_Horizontal");
        v = Input.GetAxisRaw("Input_Rotation_Controller_Vertical");

        if (h > 0.8f || v > 0.8f || h < -0.8f || v < -0.8f)
        {
            angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg + 180;
            //transform.rotation = Quaternion.Euler(0, 0, angle);
            Vector3 newAngle = new Vector3(0, 0, angle);

            Vector3 newDirection = Vector3.RotateTowards(transform.rotation.eulerAngles, newAngle, 0, rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0,0, newDirection.z);






        }
        else
        {
            s = 0;
        }

        
    }


}
