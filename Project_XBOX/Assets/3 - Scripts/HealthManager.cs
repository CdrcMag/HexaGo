using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private const int HEAL_AMOUNT = 20;

    private const int MIN_CHANCE_TO_SPAWN = 10;
    private const int MAX_CHANCE_TO_SPAWN = 25;

    private const int EASY_SPAWN = 14;
    private const int NORMAL_SPAWN = 12;
    private const int HARD_SPAWN = 10;

    private const float DELAY_TO_INCREASE_SPAWN_RATE = 5f;

    // ======================= VARIABLES =======================

    private int chanceToSpawnHealthPotion = 0; // Pourcentage
    private int healAmount = 0; // Pourcentage

    private GameObject healthPotionPrefab;

    // =========================================================

    private void Awake()
    {
        healthPotionPrefab = Resources.Load<GameObject>("Health Potion");

        setHealAmount(HEAL_AMOUNT);
        resetSpawnRate();
    }

    private void Start()
    {
        StartCoroutine(IIncreaseRateSpawn());
    }

    public int getHealthAmount()
    {
        return healAmount;
    }

    public void setHealAmount(int _value)
    {
        healAmount = _value;
    }

    public void callToSpawnHeal(Vector2 _pos)
    {
        int randomRate = Random.Range(1, 101);

        if (randomRate <= clampSpawnRate())
        {
            spawnHealAtPosition(_pos);
        }
    }

    private void spawnHealAtPosition(Vector2 _pos)
    {
        if (healthPotionPrefab != null)
        {
            Instantiate(healthPotionPrefab, _pos, Quaternion.identity);
            resetSpawnRate();
        }
    }

    private int clampSpawnRate()
    {
        return Mathf.Clamp(chanceToSpawnHealthPotion, MIN_CHANCE_TO_SPAWN, MAX_CHANCE_TO_SPAWN);
    }

    private void resetSpawnRate()
    {
        if(PlayerPrefs.GetString("Difficulty", "Easy") == "Easy") { chanceToSpawnHealthPotion = EASY_SPAWN; }
        else if (PlayerPrefs.GetString("Difficulty", "Easy") == "Normal") { chanceToSpawnHealthPotion = NORMAL_SPAWN; }
        else if (PlayerPrefs.GetString("Difficulty", "Easy") == "Hard") { chanceToSpawnHealthPotion = HARD_SPAWN; }
    }

    private IEnumerator IIncreaseRateSpawn()
    {
        yield return new WaitForSeconds(DELAY_TO_INCREASE_SPAWN_RATE);

        chanceToSpawnHealthPotion++;

        StartCoroutine(IIncreaseRateSpawn());
    }
}