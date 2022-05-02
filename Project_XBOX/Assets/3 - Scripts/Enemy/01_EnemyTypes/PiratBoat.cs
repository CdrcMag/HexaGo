using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratBoat : Enemy
{
    // ===================== VARIABLES =====================

    [SerializeField] private float boatSpeed = 4f;
    [SerializeField] private float endPosY = 0f;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
    }

    private void Start()
    {
        StartCoroutine(ILoadSoundBoat());

        StartCoroutine(moveForward());
    }

    private IEnumerator moveForward()
    {
        float endPosX = transform.position.x;
        Vector2 target = new Vector2(endPosX, endPosY);
 
        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, boatSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator ILoadSoundBoat()
    {
        yield return new WaitForSeconds(0.5f);

        base.soundManager.playAudioClip(13);
    }
}
