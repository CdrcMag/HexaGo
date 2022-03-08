using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrap : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private float damage = 0;
    private PlayerManager player;
    private SoundManager soundManager;

    // =====================================================

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float currentLifePoint = player.GetLifePoint();
            player.SetLifePoint(damage);
            soundManager.playAudioClipWithPitch(5, 0.5f);
        }
    }
}
