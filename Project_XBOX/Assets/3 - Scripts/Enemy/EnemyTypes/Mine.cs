using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mine : MonoBehaviour
{
    private const float DELAY = 0.01f;
    private const float ADDSCALE = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private Light2D lightMine;
    [SerializeField] private GameObject ptcMinePref;

    private float startScale = 0f;

    // =====================================================


    private void Start()
    {
        StartCoroutine(StartAnimateMine());
    }

    private IEnumerator StartAnimateMine()
    {
        yield return new WaitForSeconds(2f);
        startScale = transform.localScale.x;
        StartCoroutine(AnimateMine());
    }

    private IEnumerator AnimateMine()
    {
        while(transform.localScale.x < startScale + 0.1f)
        {
            transform.localScale = new Vector2(transform.localScale.x + ADDSCALE, transform.localScale.y + ADDSCALE);
            lightMine.intensity += 0.1f;

            yield return new WaitForSeconds(DELAY);
        }

        while (transform.localScale.x > startScale)
        {
            transform.localScale = new Vector2(transform.localScale.x - ADDSCALE, transform.localScale.y - ADDSCALE);
            lightMine.intensity -= 0.1f;

            yield return new WaitForSeconds(DELAY);
        }

        StartCoroutine(AnimateMine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        GameObject ptcMine;
        ptcMine = Instantiate(ptcMinePref, transform.position, Quaternion.identity);
        Destroy(ptcMine, 4f);

        yield return new WaitForSeconds(0.2f);

        Destroy(gameObject);
    }
}
