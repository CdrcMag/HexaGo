using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    private const float DELAY = 0.05f;
    private const float ADDSCALE = 0.05f;

    // ===================== VARIABLES =====================

    public Level01[] easyRooms;
    public Level01[] mediumRooms;
    public Level01[] hardRooms;
    public Level01[] eventRooms;
    public int currentNumberRoom = 0;
    public int killedEnemies = 0;
    public Transform enemyPool;
    public Transform player;
    public AudioSource musicManager;
    public WeaponSelector weaponSelector;
    [SerializeField] private Transition transition;
    public Tutorial tutorial;

    // BOSS
    public GameObject bossPrefab;
    public AudioClip bossTheme;

    private Level01[] currentRooms;
    private int numberRoom;
    private bool[] validedRooms = { true, true, true, true, true, true, true, true, true, true, true, true };
    private bool isTutorial = false;

    // =====================================================



    private void Awake()
    {
        PrepareRoom();
    }

    private void PrepareRoom()
    {
        if(PlayerPrefs.GetString("Difficulty", "Easy") == "Easy" && PlayerPrefs.GetInt("PlayedTutorial", 0) == 0)
        {
            PrepareTutorial();

            isTutorial = true;

            return;
        }

        if(currentNumberRoom != 9)
        {
            if(currentNumberRoom < 3)
            {
                PrepareNormalRoom();
            }
            else
            {
                int eventRate = Random.Range(1, 11);

                if(eventRate < 10) // < 3
                {
                    PrepareEventRoom();
                }
                else
                {
                    PrepareNormalRoom();
                }
            }
        }
        else
        {
            player.position = new Vector2(0f, 2f);
        }

        if(currentNumberRoom != 9)
        {
            for (int i = 0; i < currentRooms[numberRoom].enemies.Length; i++)
            {
                GameObject enemy = Instantiate(currentRooms[numberRoom].enemies[i], currentRooms[numberRoom].positions[i], Quaternion.identity, enemyPool);

                if(currentRooms[numberRoom].multiplicatorLife[i] != 0)
                {
                    enemy.GetComponent<Enemy>().SetLifePoint(enemy.GetComponent<Enemy>().GetLifePoint() * currentRooms[numberRoom].multiplicatorLife[i]);
                }

                StartCoroutine(IGrowEnemy(enemy, i));
            }
        }
        else
        {
            SpawnBoss();
        }

        killedEnemies = 0;

        StartCoroutine(IActivatePlayer());
    }

    private void PrepareNormalRoom()
    {
        currentRooms = ChooseDifficulty();
        numberRoom = ChooseRoom(currentRooms);
        player.position = currentRooms[numberRoom].startPosPlayer;
    }

    private void PrepareEventRoom()
    {
        currentRooms = eventRooms;
        numberRoom = Random.Range(0, eventRooms.Length);
        player.position = currentRooms[numberRoom].startPosPlayer;
    }

    private IEnumerator IActivatePlayer()
    {
        yield return new WaitForSeconds(1.5f);

        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Player_Movement>().canMove = true;
    }

    private IEnumerator IGrowEnemy(GameObject _enemy, int _index)
    {
        float finalScaleX = _enemy.transform.localScale.x * currentRooms[numberRoom].multiplicatorScale[_index];
        _enemy.transform.localScale = new Vector2(0f, 0f);

        while (_enemy != null && _enemy.transform.localScale.x < finalScaleX)
        {
            yield return new WaitForSeconds(DELAY);

            if(_enemy != null)
                _enemy.transform.localScale = new Vector2(_enemy.transform.localScale.x + ADDSCALE, _enemy.transform.localScale.y + ADDSCALE);
        }
    }

    private Level01[] ChooseDifficulty()
    {
        Level01[] difficulty = easyRooms;

        if(PlayerPrefs.GetString("Difficulty", "Easy") == "Easy")
        {
            if (currentNumberRoom < 3)
            {
                difficulty = easyRooms;
            }
            else if (currentNumberRoom < 6)
            {
                difficulty = mediumRooms;
                musicManager.pitch = 1.15f;
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
                musicManager.pitch = 1.3f;
            }
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy") == "Normal")
        {
            if (currentNumberRoom < 2)
            {
                difficulty = easyRooms;
            }
            else if (currentNumberRoom < 5)
            {
                difficulty = mediumRooms;
                musicManager.pitch = 1.15f;
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
                musicManager.pitch = 1.3f;
            }
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy") == "Hard")
        {
            if (currentNumberRoom < 3)
            {
                difficulty = mediumRooms;
                musicManager.pitch = 1.15f;
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
                musicManager.pitch = 1.3f;
            }
        }

        return difficulty;
    }

    private int ChooseRoom(Level01[] _difficulty)
    {
        bool check = false;
        int numberRoom = 0;

        while(!check)
        {
            numberRoom = Random.Range(0, _difficulty.Length);

            if(validedRooms[numberRoom])
            {
                check = true;
            }
        }

        validedRooms[numberRoom] = false;

        return numberRoom;
    }

    public void UpdateState()
    {
        if(isTutorial)
        {
            tutorial.EndTutorial();

            PrepareRoom();

            isTutorial = false;

            return;
        }
     
        killedEnemies++;

        CheckState();
    }

    private void CheckState()
    {
        if(killedEnemies == currentRooms[numberRoom].killableEnemies)
        {
            if(currentNumberRoom == 2 || currentNumberRoom == 5 || currentNumberRoom == 8)
            {
                weaponSelector.ChooseNewItem();
            }

            ClearScene();

            player.GetComponent<Player_Movement>().canMove = false;
            player.GetComponent<Rigidbody2D>().isKinematic = true;

            currentNumberRoom++;
            killedEnemies = 0;

            PrepareRoom();
        }
    }

    private void ClearScene()
    {
        for(int i = 0; i < enemyPool.childCount; i++)
        {
            Destroy(enemyPool.GetChild(i).gameObject);
        }
    }

    private void SpawnBoss()
    {
        musicManager.Stop();
        musicManager.clip = bossTheme;
        musicManager.Play();
        musicManager.pitch = 1f;

        GameObject boss;
        boss = Instantiate(bossPrefab, new Vector2(0f, 0f), Quaternion.identity);
    }

    public void FinishLevel()
    {   
        if(!isTutorial)
        {
            StartCoroutine(IFinishLevel());
        }
    }

    private IEnumerator IFinishLevel()
    {
        if(transition == null)
        {
            print("The script TRANSITION.cs is missing !!");
        }

        transition.StartAugment();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }

    private void PrepareTutorial()
    {
        tutorial.LoadTutorial();
    }
}
