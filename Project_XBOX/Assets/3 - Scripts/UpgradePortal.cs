using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePortal : MonoBehaviour
{
    private const float MAX_SCALE = 1.8f;
    private const float MIN_SCALE = 0f;

    // ======================= VARIABLES =======================

    private SoundManager soundManager;

    // =========================================================


    private void Awake()
    {
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameObject.Find("Weapon Selector 2") != null)
            {
                soundManager.playAudioClip(19);
                StartCoroutine(IAnimationPortal(other.gameObject));
            }
            else
            {
                Debug.Log("Objet pour sélectionner l'arme introuvable.");
            }
        }
    }

    private IEnumerator IAnimationPortal(GameObject _player)
    {
        Player_Movement pm = _player.GetComponent<Player_Movement>();
        pm.canMove = false;

        Vector2 target = transform.position;

        float tempScale = 0f;

        while (Vector2.Distance(_player.transform.position, target) > 0.01f)
        {
            _player.transform.position = Vector2.MoveTowards(_player.transform.position, target, 1 * Time.deltaTime);

            yield return null;
        }

        while(transform.GetChild(0).localScale.x < MAX_SCALE)
        {
            yield return new WaitForSeconds(0.01f);

            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector2(transform.GetChild(i).localScale.x + 0.1f, transform.GetChild(i).localScale.y + 0.1f);
            }
        }

        tempScale = _player.transform.localScale.x;

        while (transform.GetChild(0).localScale.x > MIN_SCALE)
        {
            yield return new WaitForSeconds(0.01f);

            if(_player.transform.localScale.x >= 0) { _player.transform.localScale = new Vector2(_player.transform.localScale.x - 0.2f, _player.transform.localScale.y - 0.2f); }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector2(transform.GetChild(i).localScale.x - 0.5f, transform.GetChild(i).localScale.y - 0.5f);
            }
        }

        _player.transform.position = new Vector2(200f, 200f);
        _player.transform.localScale = new Vector2(tempScale, tempScale);

        transform.position = new Vector2(300f, 300f);
        Destroy(gameObject, 2f);

        yield return new WaitForSeconds(0.2f);

        pm.canMove = true;
        GameObject.Find("Weapon Selector 2").GetComponent<WeaponSelectorRemake>().Initialisation();
    }
}
