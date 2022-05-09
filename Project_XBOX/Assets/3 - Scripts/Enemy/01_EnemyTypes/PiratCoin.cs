using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratCoin : MonoBehaviour
{
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private float speedRotating = 500f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject ptcQuakePref;
    [SerializeField] private GameObject ptcExplodePref;

    private bool canSpin = true;
    private SoundManager soundManager;

    // =====================================================


    private void Start()
    {
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        StartCoroutine(Explode());
    }

    private void Update()
    {
        if (canSpin)
            RotateObject();
    }

    private void RotateObject()
    {
        transform.GetChild(0).Rotate(Vector3.forward * speedRotating * Time.deltaTime);
    }

    private IEnumerator Explode()
    {
        while (speedRotating > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            speedRotating -= 7f;

            rb.velocity = new Vector2(rb.velocity.x / 1.01f, rb.velocity.y / 1.01f);
        }

        canSpin = false;
        rb.bodyType = RigidbodyType2D.Static;

        float delayToExplode = Random.Range(2f, 6f);
        yield return new WaitForSeconds(delayToExplode);

        while (transform.localScale.x > 0.3)
        {
            transform.localScale = new Vector2(transform.localScale.x - 0.075f, transform.localScale.y - 0.075f);

            yield return new WaitForSeconds(0.02f);
        }

        while (transform.localScale.x < 1)
        {
            transform.localScale = new Vector2(transform.localScale.x + 0.15f, transform.localScale.y + 0.15f);

            yield return new WaitForSeconds(0.02f);
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.localScale = new Vector2(1.5f, 1.5f);

        soundManager.playAudioClip(9);

        GameObject ptcExplode;
        ptcExplode = Instantiate(ptcExplodePref, transform.position, Quaternion.identity);
        Destroy(ptcExplode, 3f);

        yield return new WaitForSeconds(0.02f);

        Destroy(gameObject);
    }
}
