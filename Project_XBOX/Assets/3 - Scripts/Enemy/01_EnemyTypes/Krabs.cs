using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Krabs : Enemy
{
    // ===================== VARIABLES =====================

    private int magnitudeMove = 50;
    private float delay = 4f;
    private float bulletSpeed = 4f;
    private bool isGrowingBody = false;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform leftClamp;
    [SerializeField] private Transform rightClamp;
    [SerializeField] private Transform[] pawsA;
    [SerializeField] private Transform[] pawsB;
    [SerializeField] private Transform posToShoot;
    [SerializeField] private GameObject bulletPref;

    // =====================================================

    public void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    private void Start()
    {
        StartCoroutine(MoveLateral());
        StartCoroutine(AnimatePaws());
        StartCoroutine(AnimateClamp());
    }

    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);

        if (!isGrowingBody)
            StartCoroutine(GrowBody());
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1.3f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + 0.04f, body.localScale.y + 0.04f);
        }

        while (body.localScale.x > 1f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - 0.04f, body.localScale.y - 0.04f);
        }

        isGrowingBody = false;
    }

    private IEnumerator MoveLateral()
    {
        int cpt = 0;

        while(cpt < magnitudeMove)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < magnitudeMove * 2)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < magnitudeMove)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        StartCoroutine(MoveLateral());
    }

    private IEnumerator AnimatePaws()
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();
        int cpt = 0;

        while (cpt < 10)
        {
            for (int i = 0; i < pawsA.Length; i++)
            {
                currentQuaternionRot = pawsA[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z + 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsA[i].localRotation = currentQuaternionRot;
            }

            for (int i = 0; i < pawsB.Length; i++)
            {
                currentQuaternionRot = pawsB[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z - 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsB[i].localRotation = currentQuaternionRot;
            }

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < 20)
        {
            for (int i = 0; i < pawsA.Length; i++)
            {
                currentQuaternionRot = pawsA[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z - 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsA[i].localRotation = currentQuaternionRot;
            }

            for (int i = 0; i < pawsB.Length; i++)
            {
                currentQuaternionRot = pawsB[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z + 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsB[i].localRotation = currentQuaternionRot;
            }

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        cpt = 0;

        while (cpt < 10)
        {
            for (int i = 0; i < pawsA.Length; i++)
            {
                currentQuaternionRot = pawsA[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z + 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsA[i].localRotation = currentQuaternionRot;
            }

            for (int i = 0; i < pawsB.Length; i++)
            {
                currentQuaternionRot = pawsB[i].localRotation;
                currentRot = currentQuaternionRot.eulerAngles;
                currentRot = new Vector3(0, 0, currentRot.z - 1);
                currentQuaternionRot.eulerAngles = currentRot;
                pawsB[i].localRotation = currentQuaternionRot;
            }

            yield return new WaitForSeconds(0.005f);

            cpt++;
        }

        StartCoroutine(AnimatePaws());
    }

    private IEnumerator AnimateClamp()
    {
        delay = Random.Range(2f, 3.5f);

        yield return new WaitForSeconds(delay);

        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();
        int cpt = 0;

        while(cpt < 22)
        {
            currentQuaternionRot = leftClamp.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 1);
            currentQuaternionRot.eulerAngles = currentRot;
            leftClamp.localRotation = currentQuaternionRot;

            currentQuaternionRot = rightClamp.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z - 1);
            currentQuaternionRot.eulerAngles = currentRot;
            rightClamp.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(0.005f);
        }

        cpt = 0;

        while (cpt < 11)
        {
            currentQuaternionRot = leftClamp.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z - 2);
            currentQuaternionRot.eulerAngles = currentRot;
            leftClamp.localRotation = currentQuaternionRot;

            currentQuaternionRot = rightClamp.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 2);
            currentQuaternionRot.eulerAngles = currentRot;
            rightClamp.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(0.001f);
        }

        Shoot(bulletPref, posToShoot, transform, bulletSpeed);

        StartCoroutine(AnimateClamp());
    }

    public override void Shoot(GameObject _bulletPref, Transform _posToShoot, Transform _canon, float _speed)
    {
        base.Shoot(_bulletPref, _posToShoot, _canon, _speed);
    }
}
