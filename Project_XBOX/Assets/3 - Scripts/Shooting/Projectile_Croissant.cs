using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Croissant : Base_Projectile
{

    [HideInInspector] public float damageOnHit = 0;
    [SerializeField] private GameObject ptcSmileyPref;

    private SoundManager soundManager;
    private Coroutine cor;


    private void Start()
    {
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
        cor = StartCoroutine(IDestroyMine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy e = null;

            //Applique les dégâts sur l'ennemi
            if (collision.gameObject.name.Contains("spin"))
            {
                e = collision.transform.parent.parent.GetComponent<Enemy>();
            }
            else
            {
                e = collision.transform.parent.GetComponent<Enemy>();
            }

            if (e != null)
            {
                e.TakeDamage(damageOnHit);
            }

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;

            GameObject ptcSmiley;
            ptcSmiley = Instantiate(ptcSmileyPref, transform.position, Quaternion.identity);
            Destroy(ptcSmiley, 3f);

            float pitch = Random.Range(0.8f, 1.2f);
            soundManager.playAudioClipWithPitch(22, pitch);

            StopCoroutine(cor);
            Destroy(gameObject);
        }
    }

    private IEnumerator IDestroyMine()
    {
        yield return new WaitForSeconds(2.5f);

        float a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        a = 1f;

        while (a > 0f)
        {
            Color color = new Color(1f, 1f, 1f, a);
            transform.GetChild(3).GetComponent<SpriteRenderer>().color = color;
            a -= 0.1f;

            yield return new WaitForSeconds(0.005f);
        }

        Destroy(gameObject);
    }
}
