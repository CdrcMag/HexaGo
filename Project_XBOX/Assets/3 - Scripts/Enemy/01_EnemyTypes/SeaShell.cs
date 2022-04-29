using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaShell : MonoBehaviour
{
    private const float DELAY = 0.01f;

    // ===================== VARIABLES =====================

    [SerializeField] private float speedRotating = 500f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject pearlPref;
    [SerializeField] private float speedPearl = 7f;

    private bool canSpin = true;

    // =====================================================


    private void Start()
    {
        StartCoroutine(Explode());
    }

    private void Update()
    {
        if(canSpin)
            RotateObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.forward * speedRotating * Time.deltaTime);
    }

    private IEnumerator Explode()
    {
        while(speedRotating > 0f)
        {
            yield return new WaitForSeconds(DELAY);

            speedRotating -= 7f;

            rb.velocity = new Vector2(rb.velocity.x / 1.01f, rb.velocity.y / 1.01f);
        }

        canSpin = false;
        rb.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(0.1f);

        while (transform.localScale.x > 0.3)
        {
            transform.localScale = new Vector2(transform.localScale.x - 0.05f, transform.localScale.y - 0.05f);

            yield return new WaitForSeconds(0.02f);
        }

        while (transform.localScale.x < 1)
        {
            transform.localScale = new Vector2(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f);

            yield return new WaitForSeconds(0.02f);
        }

        transform.localScale = new Vector2(0f, 0f);

        GameObject bullet01;
        bullet01 = Instantiate(pearlPref, transform.position, Quaternion.identity);
        bullet01.GetComponent<Rigidbody2D>().velocity = speedPearl * Vector2.left;
        Destroy(bullet01, 10f);

        GameObject bullet02;
        bullet02 = Instantiate(pearlPref, transform.position, Quaternion.identity);
        bullet02.GetComponent<Rigidbody2D>().velocity = speedPearl * Vector2.right;
        Destroy(bullet02, 10f);

        GameObject bullet03;
        bullet03 = Instantiate(pearlPref, transform.position, Quaternion.identity);
        bullet03.GetComponent<Rigidbody2D>().velocity = speedPearl * Vector2.up;
        Destroy(bullet03, 10f);

        GameObject bullet04;
        bullet04 = Instantiate(pearlPref, transform.position, Quaternion.identity);
        bullet04.GetComponent<Rigidbody2D>().velocity = speedPearl * Vector2.down;
        Destroy(bullet04, 10f);

        Destroy(gameObject);
    }
}
