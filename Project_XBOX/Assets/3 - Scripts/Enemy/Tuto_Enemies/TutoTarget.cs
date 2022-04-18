using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTarget : MonoBehaviour
{
    // ===================== VARIABLES =====================

    public float force;
    public float torque;
    public GameObject brokenTargetPrefab;
    
    private SoundManager soundManager;
    private Tutorial tutorial;
    private float pitch = 1f;

    // =====================================================


    private void Awake()
    {
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
        tutorial = GameObject.Find("SceneManager").GetComponent<Tutorial>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            GameObject brokenTarget;
            brokenTarget = Instantiate(brokenTargetPrefab, transform.position, Quaternion.identity);
            Destroy(brokenTarget, 5f);

            pitch = Random.Range(0.7f, 1.3f);
            soundManager.playAudioClipWithPitch(10, pitch);

            Destroy(other.gameObject);

            tutorial.AddCptTarget();

            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 2f);

            foreach(Collider2D obj in objects)
            {
                if(obj.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
                    obj.GetComponent<Rigidbody2D>().AddTorque(torque);
                }
            }

            Destroy(gameObject);
        }
    }
}
