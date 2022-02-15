using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Eye : MonoBehaviour
{
    public enum State { None, Blink, Angry };

    private const int COEFF_SCALE = 5;
    private const float MIN_DELAY_BLINK = 4f;
    private const float MAX_DELAY_BLINK = 5f;


    // ===================== VARIABLES =====================

    [SerializeField] private Animator animator;
    [SerializeField] private Transform eyeIris;
    [SerializeField] private Enemy enemy;
    private State state = State.None;
    private float scale;
    private Vector3 startPositionIris;
    private float magnitude = 0.1f;
    private Vector3 startLocalPosition;

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetState(State _state) { state = _state; }

    // =====================================================


    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (eyeIris == null)
        {
            eyeIris = transform.GetChild(0);
        }

        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
        }

        scale = transform.lossyScale.x;
        startPositionIris = eyeIris.position;
        startLocalPosition = transform.localPosition;

        StartCoroutine(BlinkEye());
    }

    private void Update()
    {
        if (state == State.None)
        {
            FollowTarget();
        }
    }



    // Set the state of animation of the eye
    private void SetState(int _state)
    {
        animator.SetInteger("State", _state);

        if (_state == 0)
        {
            state = State.None;
        }
        else if (_state == 1)
        {
            state = State.Blink;
        }
        else if (_state == 2)
        {
            state = State.Angry;
        }
    }

    // The Iris follows the target
    private void FollowTarget()
    {
        magnitude = scale / COEFF_SCALE;

        Vector3 newPos = Vector3.ClampMagnitude(new Vector3(enemy.GetTarget().position.x - startPositionIris.x, enemy.GetTarget().position.y - startPositionIris.y, startPositionIris.z), magnitude);

        eyeIris.localPosition = newPos;
    }

    // Blink at delay and continuously
    private IEnumerator BlinkEye()
    {
        float delay = Random.Range(MIN_DELAY_BLINK, MAX_DELAY_BLINK);

        yield return new WaitForSeconds(delay);

        if (state == State.None)
        {
            SetState(1);
            eyeIris.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.3f);

            SetState(0);
            eyeIris.gameObject.SetActive(true);
        }

        StartCoroutine(BlinkEye());
    }

    public void MakeAngryEye()
    {
        StartCoroutine(AngryEye());
    }

    // Angry animation
    private IEnumerator AngryEye()
    {
        eyeIris.gameObject.SetActive(false);
        SetState(2);

        yield return new WaitForSeconds(0.45f);

        int cpt = 0;
        int cpt2 = 0;

        while(cpt < 40)
        {
            float x = Random.Range(-0.15f, 0.15f);
            float y = Random.Range(-0.15f, 0.15f);
            float xPart = x / 4;
            float yPart = y / 4;
            cpt2 = 0;

            while (cpt2 < 4)
            {
                yield return new WaitForSeconds(0.01f);
                cpt++;
                cpt2++;
                transform.localPosition = new Vector2(transform.localPosition.x + xPart, transform.localPosition.y + yPart);
            }

            cpt2 = 0;

            while (cpt2 < 4)
            {
                yield return new WaitForSeconds(0.01f);
                cpt++;
                cpt2++;
                transform.localPosition = new Vector2(transform.localPosition.x - xPart, transform.localPosition.y - yPart);
            }
        }

        transform.localPosition = startLocalPosition;
        eyeIris.gameObject.SetActive(true);
        SetState(0);
    }
}
