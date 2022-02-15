using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy
{
    private const float ROTATE_SPEED_CANON = 10f;

    // ===================== VARIABLES =====================

    private float rotatingSpeed = -200f;
    private float range = 5f;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform spin;
    [SerializeField] private Transform canon;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    public void Update()
    {
        base.MoveToBeInRange(range);
        base.RotateToward(ROTATE_SPEED_CANON, canon);

        RotateSpin(rotatingSpeed);
    }

    // Rotate the spin
    private void RotateSpin(float _speed)
    {
        spin.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }
}
