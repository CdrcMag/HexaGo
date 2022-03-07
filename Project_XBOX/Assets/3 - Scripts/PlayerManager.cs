using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // ===================== VARIABLES =====================

    [SerializeField] private float lifePoint = 100;
    public float reduction = 0;//Percentage ( 0 - 1 )
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private Transition transition;

    // =====================================================

    // =================== [ SET - GET ] ===================

    public void SetLifePoint(float _damage) 
    {
        float damageToTake = _damage - (_damage * reduction);

        lifePoint -= damageToTake;
        
        if(lifePoint <= 0)
        {
            Die();
        }
    }

    public float GetLifePoint() { return lifePoint; }

    // =====================================================


    private void Die()
    {
        soundManager.playAudioClip(7);
        transform.position = new Vector2(100f, 100f);

        StartCoroutine(ILoadMenu());
    }

    private IEnumerator ILoadMenu()
    {
        yield return new WaitForSeconds(1f);

        transition.StartAugment();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }
}
