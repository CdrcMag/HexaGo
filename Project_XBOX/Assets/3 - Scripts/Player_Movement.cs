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
    [SerializeField] public float speedBoost = 1;

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
            //HandleInputsController();
            //HandleRotationController();

            //Déplacement en X et Y
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            float h = Input.GetAxis("Input_Rotation_Controller_Horizontal");
            float v = Input.GetAxis("Input_Rotation_Controller_Vertical");

            //print($"H : {h} / V : {v}");

            //Movement
            movement.Normalize();
            _rb.MovePosition(_rb.position + movement * speed * speedBoost * Time.deltaTime);

            if (h > 0.8f || v > 0.8f || h < -0.8f || v < -0.8f)
            {
                float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg + 180;
                float newRotation = Mathf.SmoothDampAngle(transform.eulerAngles.z, angle, ref zVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0,0, newRotation);
                //_rb.MoveRotation(angle);
            }
           


        }


    }

    float zVelocity = 0.0f;

    private void FixedUpdate()
    {
        //Gère le mouvement du personnage au clavier
        if(UseKeyboardSettings)
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

        if(Input.GetAxisRaw("Input_Rotation") != 0)
            _rb.MoveRotation(currentRotation * Time.deltaTime);
     
    }

  
    private void HandleRotationController()
    {
        ////Rotation
        //h = Input.GetAxisRaw("Input_Rotation_Controller_Horizontal");
        //v = Input.GetAxisRaw("Input_Rotation_Controller_Vertical");

        //if (h > 0.8f || v > 0.8f || h < -0.8f || v < -0.8f)
        //{
        //    angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;// + 180;

        //    print(angle);
        //    Vector3 newAngle = new Vector3(0, 0, angle);

        //    Vector3 newDirection = Vector3.RotateTowards(transform.rotation.eulerAngles, newAngle, 0, rotationSpeed * Time.deltaTime);

        //    transform.rotation = Quaternion.Euler(0, 0, newDirection.z);

        //}
        //else
        //{
        //    s = 0;
        //}







    }


}
