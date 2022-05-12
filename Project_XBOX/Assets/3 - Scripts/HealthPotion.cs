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

    private SoundManager soundManager;
    private HealthManager healthManager;

    // =========================================================

    private void Awake()
    {
        HealthPotionParticles = Resources.Load<GameObject>("Health Potion Particles");

        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        if(GameObject.Find("SceneManager").GetComponent<HealthManager>() != null) { healthManager = GameObject.Find("SceneManager").GetComponent<HealthManager>(); }
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
            soundManager.playAudioClip(14);

            PlayerManager.Instance.AddHealthPoint(healthManager.getHealthAmount());

            GameObject particles_instance = Instantiate(HealthPotionParticles, transform.position, Quaternion.identity);
            Destroy(particles_instance, 1f);

            PlayerPrefs.SetInt("Nbr_HealRamasses", PlayerPrefs.GetInt("Nbr_HealRamasses") + 1);

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
