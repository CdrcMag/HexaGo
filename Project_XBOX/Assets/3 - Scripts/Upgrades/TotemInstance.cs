using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemInstance : MonoBehaviour
{

    private const float DELAY_GROW = 0.005f;
    private const float DELAY_SHAKE = 0.003f;
    private const float MULTIPLICATOR_SCALE = 1.3f;
    private const float TICK_DELAY = 20f;

    // ======================= VARIABLES =======================

    public GameObject bombCollider;

    [SerializeField] private GameObject ptcExplosionPref;
    private float startScaleX;
    private float maxScaleX;
    private float addScale;

    [HideInInspector] public Totems totemMain;
    public GameObject explosion;

    // =========================================================


    private void Start()
    {
        startScaleX = transform.localScale.x;
        maxScaleX = startScaleX * MULTIPLICATOR_SCALE;
        addScale = (maxScaleX - startScaleX) / TICK_DELAY;

        StartCoroutine(IStartAnimGrow());
        StartCoroutine(IActivateCollision());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject ptcExplosion;
        ptcExplosion = Instantiate(ptcExplosionPref, transform.position, Quaternion.identity);
        Destroy(ptcExplosion, 4f);

        totemMain.totalTotemPlaced -= 1;
    }

    private IEnumerator IStartAnimGrow()
    {
        while (transform.localScale.x < maxScaleX)
        {
            yield return new WaitForSeconds(DELAY_GROW);

            transform.localScale = new Vector2(transform.localScale.x + addScale * 2, transform.localScale.y + addScale * 2);
        }

        while (transform.localScale.x > startScaleX)
        {
            yield return new WaitForSeconds(DELAY_GROW);

            transform.localScale = new Vector2(transform.localScale.x - addScale * 2, transform.localScale.y - addScale * 2);
        }

        StartCoroutine(IAnimShake());
    }

    private IEnumerator IAnimShake()
    {
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();
        int cpt = 0;

        while (cpt < 11)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        cpt = 0;

        while (cpt < 22)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z - 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        cpt = 0;

        while (cpt < 11)
        {
            currentQuaternionRot = transform.localRotation;
            currentRot = currentQuaternionRot.eulerAngles;
            currentRot = new Vector3(0, 0, currentRot.z + 2);
            currentQuaternionRot.eulerAngles = currentRot;
            transform.localRotation = currentQuaternionRot;

            cpt++;

            yield return new WaitForSeconds(DELAY_SHAKE);
        }

        StartCoroutine(IAnimShake());
    }

    private IEnumerator IActivateCollision()
    {
        yield return new WaitForSeconds(1f);

        bombCollider.SetActive(true);
    }
}
