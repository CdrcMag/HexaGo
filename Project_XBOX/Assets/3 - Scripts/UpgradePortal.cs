using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePortal : MonoBehaviour
{
    private SoundManager soundManager;


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

        while (Vector2.Distance(_player.transform.position, target) > 0.01f)
        {
            _player.transform.position = Vector2.MoveTowards(_player.transform.position, target, 1 * Time.deltaTime);

            yield return null;
        }

        pm.canMove = true;
        GameObject.Find("Weapon Selector 2").GetComponent<WeaponSelectorRemake>().Initialisation();
    }
}
