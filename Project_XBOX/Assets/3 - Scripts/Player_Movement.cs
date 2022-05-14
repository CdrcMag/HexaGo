using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    //Allow movement variable
    public bool canMove = true;

    //Rigidbody du joueur
    private Rigidbody2D _rb;

    [Header("Movement settings")]
    //Vitesse de déplacement
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] public float speedBoost = 1;

    //Vecteur de déplacement
    [HideInInspector] public Vector2 movement;
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
        if (!canMove)
            return;

        //Keyboard
        if(UseKeyboardSettings)
            HandleInputsKeyboard();

        //Controller
        if (UseControllerSettings)
        {
            //HandleInputsController();
            //HandleRotationController();

            //Déplacement en X et Y
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            float h = Input.GetAxis("Input_Rotation_Controller_Horizontal");
            float v = Input.GetAxis("Input_Rotation_Controller_Vertical");
            if (Input.GetAxisRaw("Controller_LeftTrigger") == 1)
            {
                speed = 5;
            }
            else
            {
                speed = 7 * speedBoost;
            }

                //print($"H : {h} / V : {v}");

                //Movement
                movement.Normalize();
            _rb.MovePosition(_rb.position + movement * speed * speedBoost * Time.deltaTime);

            if (h > sensitivity || v > sensitivity || h < -sensitivity || v < -sensitivity)
            {
                float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg + 180;
                float newRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, angle, ref zVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0,0, newRotation);
            }
           


        }


    }

    float sensitivity = 0.8f;

    float zVelocity = 0.0f;

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        //Gère le mouvement du personnage au clavier
        if (UseKeyboardSettings)
            HandleMovementKeyboard();
        
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

        _rb.MovePosition(_rb.position + movement * speed * speedBoost * Time.deltaTime);

        float rotationInput = Input.GetAxisRaw("Input_Rotation");

        if (rotationInput != 0)
        {
            _rb.freezeRotation = false;
            _rb.MoveRotation(_rb.rotation + 200 * -rotationInput * Time.deltaTime);
        }
        else
        {
            _rb.freezeRotation = true;
        }
           
     
    }


}
