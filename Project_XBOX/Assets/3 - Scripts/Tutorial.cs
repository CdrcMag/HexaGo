using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Vector3 STARTPOS = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion STARTROT = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    private const float MAXSCALE_TARGET = 1.2f;
    private const float MINSCALE_TARGET = 0.3f;
    private const float SCALE_TARGET = 1f;
    private const float ADDSCALE = 0.035f;
    private const float DELAY = 0.01f;
    private const float FORCE = 100f;
    private const float TORQUE = 20f;

    // ===================== VARIABLES =====================

    [Header("Drag from Scene")]
    public RoomManager roomManager;
    public Transform player;
    public Transform background;
    public AudioSource musicManager;
    public Transform tutorialRoot;
    public SoundManager soundManager;
    public Transform enemyPool;

    [Header("Drag from Assets")]
    public AudioClip tutoTheme;
    public GameObject targetPrefab;
    public AudioClip level01Theme;
    public GameObject infoDashPref;
    public GameObject infoBombsPref;

    private int state = 0;
    private int cptTarget = 0;
    private Player_Shooting ps;
    private bool isActive = false;

    // =====================================================

    public void LoadTutorial()
    {
        isActive = true;

        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<Player_Movement>().canMove = true;

        ps = player.GetComponent<Player_Shooting>();

        for(int i = 0; i < background.childCount; i++)
        {
            background.GetChild(i).gameObject.SetActive(false);
        }

        musicManager.Stop();
        musicManager.clip = tutoTheme;
        musicManager.Play();

        tutorialRoot.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        if (Input.GetButtonUp("Start") || Input.GetKeyUp(KeyCode.A))
        {
            // Skip Tutorial
            roomManager.UpdateState();
        }

        // Check conditions for the current state
        if (state == 6) { return; }
        else if (state == 0) { CheckState00(); }
        else if(state == 1) { CheckState01(); }
        else if (state == 2) { CheckState02(); }
        else if (state == 3) { CheckState03(); }
        else if (state == 4) { CheckState04(); }
        else if (state == 5) { CheckState05(); }
    }

    private void CheckState00()
    {
        if(player.position != STARTPOS && player.rotation != STARTROT)
        {
            tutorialRoot.GetChild(1).gameObject.SetActive(false);
            tutorialRoot.GetChild(2).gameObject.SetActive(true);

            state++;
        }
    }

    private void CheckState01()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire1_Controller") > 0.9f)
        {
            tutorialRoot.GetChild(2).gameObject.SetActive(false);
            SpawnTarget(new Vector2(6f, 0f));

            state++;
        }
    }

    private void CheckState02()
    {
        if (cptTarget == 1)
        {
            SpawnTarget(new Vector2(-6f, 0f));

            state++;
        }
    }

    private void CheckState03()
    {
        if (cptTarget == 2)
        {
            SpawnTarget(new Vector2(-5f, -3f));
            SpawnTarget(new Vector2(5f, -3f));

            state++;
        }
    }

    private void CheckState04()
    {
        if (cptTarget == 4)
        {
            tutorialRoot.GetChild(3).gameObject.SetActive(true);
            ps.SetUpgrade(Player_Shooting.SlotName.BackLeft, ps.Upgrades[1]);
            ps.SetUpgrade(Player_Shooting.SlotName.BackRight, ps.Upgrades[2]);

            StartCoroutine(ShowInfoUpgrades());

            state++;
        }
    }

    private void CheckState05()
    {
        if (Input.GetAxis("Controller_LeftTrigger") > 0.9f)
        {
            tutorialRoot.GetChild(0).gameObject.SetActive(false);
            tutorialRoot.GetChild(3).gameObject.SetActive(false);
            tutorialRoot.GetChild(4).gameObject.SetActive(true);
            tutorialRoot.GetChild(5).gameObject.SetActive(true);

            SpawnMiniBoss();

            state++;
        }
    }

    public void AddCptTarget()
    {
        cptTarget++;
    }

    private void SpawnTarget(Vector2 pos)
    {
        GameObject target;
        target = Instantiate(targetPrefab, pos, Quaternion.identity, tutorialRoot);

        StartCoroutine(IGrowTarget(target.transform));
    }

    private IEnumerator IGrowTarget(Transform obj)
    {
        obj.localScale = new Vector2(MINSCALE_TARGET, MINSCALE_TARGET);

        while(obj != null && obj.localScale.x < MAXSCALE_TARGET)
        {
            if(obj != null)
                obj.localScale = new Vector2(obj.localScale.x + ADDSCALE, obj.localScale.y + ADDSCALE);

            yield return new WaitForSeconds(DELAY);
        }

        while(obj != null && obj.localScale.x > SCALE_TARGET)
        {
            if(obj != null)
                obj.localScale = new Vector2(obj.localScale.x - ADDSCALE, obj.localScale.y - ADDSCALE);

            yield return new WaitForSeconds(DELAY);
        }
    }

    private IEnumerator ShowInfoUpgrades()
    {
        GameObject infoDash;
        infoDash = Instantiate(infoDashPref, new Vector2(player.position.x, player.position.y + 1f), Quaternion.identity);
        Destroy(infoDash, 5f);
        StartCoroutine(FadeInfo(infoDash));

        yield return new WaitForSeconds(1f);

        GameObject infoBombs;
        infoBombs = Instantiate(infoBombsPref, new Vector2(player.position.x, player.position.y + 1f), Quaternion.identity);
        Destroy(infoBombs, 5f);
        StartCoroutine(FadeInfo(infoBombs));
    }

    private IEnumerator FadeInfo(GameObject _info)
    {
        float a = 1f;
        SpriteRenderer rd = _info.GetComponent<SpriteRenderer>();
        Color color;

        while (a > 0f)
        {
            color = new Color(1f, 1f, 1f, a);
            rd.color = color;
            a -= 0.05f;

            yield return new WaitForSeconds(0.1f);
        }

        color = new Color(1f, 1f, 1f, 0);
        rd.color = color;
    }

    private void SpawnMiniBoss()
    {
        soundManager.playAudioClipWithPitch(10, 1);

        for(int i = 0; i < tutorialRoot.GetChild(4).childCount; i++)
        {
            tutorialRoot.GetChild(4).GetChild(i).GetComponent<Rigidbody2D>().gravityScale = 2f;
        }

        Collider2D[] objects = Physics2D.OverlapCircleAll(tutorialRoot.GetChild(4).position, 5f);

        foreach (Collider2D obj in objects)
        {
            if (obj.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 direction = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * FORCE);
                obj.GetComponent<Rigidbody2D>().AddTorque(TORQUE);
            }
        }

        Destroy(tutorialRoot.GetChild(4).gameObject, 5f);
    }

    public void EndTutorial()
    {
        isActive = false;

        PlayerPrefs.SetInt("PlayedTutorial", 1);

        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<Player_Movement>().canMove = false;
        player.GetComponent<PlayerManager>().ResetLifePoint();

        for (int i = 0; i < background.childCount; i++)
        {
            background.GetChild(i).gameObject.SetActive(true);
        }

        for(int j = 0; j < enemyPool.childCount; j++)
        {
            Destroy(enemyPool.GetChild(j).gameObject);
        }

        musicManager.Stop();
        musicManager.clip = level01Theme;
        musicManager.Play();

        tutorialRoot.gameObject.SetActive(false);
    }
}
