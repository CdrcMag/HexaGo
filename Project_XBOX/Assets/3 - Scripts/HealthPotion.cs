using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPotion : Collectable
{
    private const float DELAY_GROW = 0.005f;
    private const float DELAY_SHAKE = 0.003f;
    private const float MULTIPLICATOR_SCALE = 1.5f;
    private const float TICK_DELAY = 25f;
    private const float SHAKE_DELAY_ACTIVATION = 4f;

    // ======================= VARIABLES =======================

    private GameObject HealthPotionParticles;
    private float startScaleX;
    private float maxScaleX;
    private float addScale;

    // =========================================================

    private void Awake()
    {
        HealthPotionParticles = Resources.Load<GameObject>("Health Potion Particles");
    }

    private void Start()
    {
        startScaleX = transform.localScale.x;
        maxScaleX = startScaleX * MULTIPLICATOR_SCALE;
        addScale = (maxScaleX - startScaleX) / TICK_DELAY;

        StartCoroutine(IStartAnimGrow());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.AddHealthPoint(PlayerManager.Instance.HealAmount);

            GameObject particles_instance = Instantiate(HealthPotionParticles, transform.position, Quaternion.identity);
            Destroy(particles_instance, 1f);

            Destroy(gameObject);
        }    
    }


    private IEnumerator IStartAnimGrow()
    {
        while(transform.localScale.x < maxScaleX)
        {
            yield return new WaitForSeconds(DELAY_GROW);

            transform.localScale = new Vector2(transform.localScale.x + addScale, transform.localScale.y + addScale);
        }

        while (transform.localScale.x > startScaleX)
        {
            yield return new WaitForSeconds(DELAY_GROW);

            transform.localScale = new Vector2(transform.localScale.x - addScale, transform.localScale.y - addScale);
        }

        StartCoroutine(IAnimShake());
    }

    private IEnumerator IAnimShake()
    {
        yield return new WaitForSeconds(SHAKE_DELAY_ACTIVATION);

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


}
