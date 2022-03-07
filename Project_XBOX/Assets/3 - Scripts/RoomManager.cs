using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private const float DELAY = 0.05f;
    private const float ADDSCALE = 0.05f;

    // ===================== VARIABLES =====================

    public Level01[] easyRooms;
    public Level01[] mediumRooms;
    public Level01[] hardRooms;
    public int currentNumberRoom = 0;
    public int killedEnemies = 0;
    public Transform enemyPool;
    public Transform player;

    private Level01[] currentRooms;
    private int numberRoom;
    private bool[] validedRooms = { true, true, true, true, true, true, true, true, true, true };

    // =====================================================



    private void Awake()
    {
        PrepareRoom();
    }

    private void PrepareRoom()
    {
        currentRooms = ChooseDifficulty();
        numberRoom = ChooseRoom(currentRooms);

        for (int i = 0; i < currentRooms[numberRoom].enemies.Length; i++)
        {
            GameObject enemy = Instantiate(currentRooms[numberRoom].enemies[i], currentRooms[numberRoom].positions[i], Quaternion.identity, enemyPool);

            StartCoroutine(IGrowEnemy(enemy, i));
        }

        StartCoroutine(IActivatePlayer());
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

        while (_enemy.transform.localScale.x < finalScaleX)
        {
            yield return new WaitForSeconds(DELAY);

            _enemy.transform.localScale = new Vector2(_enemy.transform.localScale.x + ADDSCALE, _enemy.transform.localScale.y + ADDSCALE);
        }
    }

    private Level01[] ChooseDifficulty()
    {
        Level01[] difficulty = easyRooms;

        if(currentNumberRoom < 3)
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
        killedEnemies++;

        CheckState();
    }

    private void CheckState()
    {
        if(killedEnemies == currentRooms[numberRoom].killableEnemies)
        {
            ClearScene();

            player.GetComponent<Player_Movement>().canMove = false;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.position = currentRooms[numberRoom].startPosPlayer;

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
}
