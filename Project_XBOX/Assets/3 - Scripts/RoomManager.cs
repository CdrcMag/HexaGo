using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    private const float DELAY = 0.05f;
    private const float ADDSCALE = 0.05f;
    private const float TRANSITION_SPEED = 8f;

    // CONST TO TEST SCENES EVENT
    private const float START_EVENT_RATE = 3f; // Write 20f to test event room, 3f for basic game
    private const float LIMIT_SPAWN_EVENT = 3f; // Write 0f to test event room, 3f for basic game
    private const int SCENE_EVENT = -1; // Write the number of the scene event to test, -1 for basic game

    // ===================== VARIABLES =====================

    public Level01[] easyRooms;
    public Level01[] mediumRooms;
    public Level01[] hardRooms;
    public Level01[] eventRooms;
    public int currentNumberRoom = 0;
    public int killedEnemies = 0;
    public Transform enemyPool;
    public Transform player;
    public MusicManager musicManager;
    public WeaponSelectorRemake weaponSelector;
    [SerializeField] private Transition transition;
    public Tutorial tutorial;
    [SerializeField] private GameObject crossPref;

    // BOSS
    public GameObject bossPrefab;

    private Level01[] currentRooms;
    private int numberRoom;
    private bool[] validedRooms = { true, true, true, true, true, true, true, true, true, true, true, true };
    private bool isTutorial = false;

    private int maxRangeEventRate = 11;

    // Upgrade Portal
    [SerializeField] private GameObject upgradePortalPref;
    private GameObject upgradePortal;
    [SerializeField] private GameObject exitPortalPref;
    private GameObject exitPortal;
    private bool mustSpawnPortal = false;
    private bool hasTookPortal = false;

    // =====================================================



    private void Awake()
    {
        if(GameObject.Find("MusicManager") != null) { musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>(); }

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
            if(currentNumberRoom < LIMIT_SPAWN_EVENT)
            {
                PrepareNormalRoom();
            }
            else
            {
                int eventRate = Random.Range(1, maxRangeEventRate);

                if(eventRate < START_EVENT_RATE)
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

                if(enemy.transform.localScale.x < 2) { StartCoroutine(IGrowEnemy(enemy, i)); }
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

        // Set la position du player
        if (!hasTookPortal) { StartCoroutine(ITranslatePlayerToStartPos(currentRooms[numberRoom].startPosPlayer)); }
        else { player.position = currentRooms[numberRoom].startPosPlayer; hasTookPortal = false; }

        if(mustSpawnPortal)
        {
            StartCoroutine(ISpawnExitPortal());
            mustSpawnPortal = false;
        }
    }

    private void PrepareEventRoom()
    {
        currentRooms = eventRooms;
        int choiceEvent = SCENE_EVENT;

        if (choiceEvent == -1) { numberRoom = Random.Range(0, eventRooms.Length); }
        else { numberRoom = choiceEvent; }

        // Set la position du player
        if (!hasTookPortal) { StartCoroutine(ITranslatePlayerToStartPos(currentRooms[numberRoom].startPosPlayer)); }
        else {player.position = currentRooms[numberRoom].startPosPlayer; hasTookPortal = false; }

        maxRangeEventRate += 4;

        if (mustSpawnPortal)
        {
            StartCoroutine(ISpawnExitPortal());
            mustSpawnPortal = false;
        }
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
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
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
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
            }
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy") == "Hard")
        {
            if (currentNumberRoom < 3)
            {
                difficulty = mediumRooms;
            }
            else if (currentNumberRoom < 9)
            {
                difficulty = hardRooms;
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
            ClearScene();
            PlayerPrefs.SetInt("Nbr_TotalSalles", PlayerPrefs.GetInt("Nbr_TotalSalles") + 1);

            if (currentNumberRoom == 2 || currentNumberRoom == 5 || currentNumberRoom == 8)
            {
                hasTookPortal = true;

                // Spawn du portail permettant de choisir une nouvelle arme
                if(upgradePortalPref == null) { Debug.Log("Ajouter le prefab de UpgradePortal dans SceneManager -> RoomManager.cs"); }
                Vector2 nextSpawnPos = new Vector2(0f, 0f);
                upgradePortal = Instantiate(upgradePortalPref, nextSpawnPos, Quaternion.identity);

                // Music change
                musicManager.activateMenuThread();
                musicManager.desactivateGameThread();
            }
            else
            {
                ResetRoom();
            }
        }
    }

    public void ResetRoomWithMute()
    {
        mustSpawnPortal = true;

        musicManager.desactivateMenuThread();
        musicManager.activateGameThread();
        ResetRoom();
    }

    public void ResetRoom()
    {
        // Destroy Upgrade Portal if exist
        if(upgradePortal != null) { Destroy(upgradePortal); }

        player.GetComponent<Player_Movement>().canMove = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        currentNumberRoom++;
        killedEnemies = 0;

        PrepareRoom();
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
        musicManager.setBossTheme();

        GameObject boss;
        boss = Instantiate(bossPrefab, new Vector2(0f, 0f), Quaternion.identity, enemyPool);
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

    private IEnumerator ISpawnExitPortal()
    {
        exitPortal = Instantiate(exitPortalPref, player.transform.position, Quaternion.identity);
        Transform tr = exitPortal.transform;

        for (int i = 0; i < tr.childCount; i++)
        {
            tr.GetChild(i).localScale = new Vector2(0f, 0f);
        }

        while (tr.GetChild(0).localScale.x < 1f)
        {
            yield return new WaitForSeconds(0.01f);

            for (int i = 0; i < tr.childCount; i++)
            {
                tr.GetChild(i).localScale = new Vector2(tr.GetChild(i).localScale.x + 0.05f, tr.GetChild(i).localScale.y + 0.05f);
            }
        }

        yield return new WaitForSeconds(0.2f);

        while (tr.GetChild(0).localScale.x > 0f)
        {
            yield return new WaitForSeconds(0.01f);

            for (int i = 0; i < tr.childCount; i++)
            {
                tr.GetChild(i).localScale = new Vector2(tr.GetChild(i).localScale.x - 0.1f, tr.GetChild(i).localScale.y - 0.1f);
            }
        }

        Destroy(exitPortal);
    }

    private IEnumerator ITranslatePlayerToStartPos(Vector2 _posToReach)
    {
        player.GetComponent<PlayerManager>().isImmune = true;
        player.GetComponent<PlayerManager>().canTakeHeal = false;
        player.GetComponent<Collider2D>().enabled = false;

        GameObject cross;
        cross = Instantiate(crossPref, player.position, Quaternion.identity);

        while (Vector2.Distance(player.position, _posToReach) > 0.01f)
        {
            player.position = Vector2.MoveTowards(player.position, _posToReach, TRANSITION_SPEED * Time.deltaTime);
            cross.transform.position = player.position;

            yield return null;
        }

        Destroy(cross);

        yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerManager>().isImmune = false;
        player.GetComponent<PlayerManager>().canTakeHeal = true;
        player.GetComponent<Collider2D>().enabled = true;
    }
}
