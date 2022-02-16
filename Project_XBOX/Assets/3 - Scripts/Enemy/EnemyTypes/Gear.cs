using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : Enemy
{
    // ===================== VARIABLES =====================

    private float rotatingSpeed = 100f;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform gear;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    public void Update()
    {
        base.MoveToward();

        RotateGear(rotatingSpeed);
    }

    // Rotate the spin
    private void RotateGear(float _speed)
    {
        gear.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }
}
