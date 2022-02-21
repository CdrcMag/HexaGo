using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMenu : MonoBehaviour
{
    public enum State { None, Blink, Angry };

    private const int COEFF_SCALE = 5;
    private const float MIN_DELAY_BLINK = 4f;
    private const float MAX_DELAY_BLINK = 5f;


    // ===================== VARIABLES =====================

    [SerializeField] private Animator animator;
    [SerializeField] private Transform eyeIris;
    private State state = State.None;

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

        StartCoroutine(BlinkEye());
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

    private IEnumerator BlinkEye()
    {
        float delay = Random.Range(MIN_DELAY_BLINK, MAX_DELAY_BLINK);

        yield return new WaitForSeconds(delay);

        if (state == State.None)
        {
            SetState(1);
            eyeIris.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.35f);

            SetState(0);
            eyeIris.gameObject.SetActive(true);
        }

        StartCoroutine(BlinkEye());
    }
}
