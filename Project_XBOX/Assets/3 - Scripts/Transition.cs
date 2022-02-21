using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public static Transition Instance;

    private List<Transform> carres = new List<Transform>();

    public bool SquaresGoSmallAtStart;

    [Range(1, 100)]
    public float speed;
    [Range(0, 10)]
    public float ecart;

    private bool isFullTransition = false;

    public void Augment() => StartCoroutine(IAugment());
    public void Reduce() => StartCoroutine(IReduce());

    private void Awake()
    {
        Instance = this;

        foreach (Transform t in transform)
        {
            carres.Add(t);
        }

        SetStates(true);

        transform.SetAsLastSibling();


    }

    private void Start()
    {
        if(SquaresGoSmallAtStart)
            StartCoroutine(IReduce());
    }

    public void StartFullTransition()
    {
        StartCoroutine(IAugment());
        isFullTransition = true;
    }

    IEnumerator IAugment()
    {
        transform.gameObject.SetActive(true);
        SetStates(false);

        yield return new WaitForSeconds(0f);

        for (int i = 0; i < carres.Count; i++)
        {
            yield return new WaitForSeconds(ecart/100);
            StartCoroutine(Augment(carres[i]));
        }

        if(isFullTransition)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(IReduce());
        }
    }

    IEnumerator IReduce()
    {
        transform.gameObject.SetActive(true);
        SetStates(true);

        yield return new WaitForSeconds(0f);

        for (int i = 0; i < carres.Count; i++)
        {
            yield return new WaitForSeconds(ecart/100);
            StartCoroutine(Reduce(carres[i]));
        }

        isFullTransition = false;
    }

    IEnumerator Reduce(Transform t)
    {
        float rot = 0;
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        t.localScale = new Vector2(1, 1);
        t.gameObject.SetActive(true);

        while (t.localScale.x > 0)
        {
            t.localScale = new Vector2(t.localScale.x - speed/100, t.localScale.y - speed/100);

            // NEW !!! ROTATION !!!
            rot += 5;
            currentRot = new Vector3(0, 0, rot);
            currentQuaternionRot.eulerAngles = currentRot;
            t.localRotation = currentQuaternionRot;

            //yield return null;
            yield return new WaitForSeconds(0.01f);
        }

        t.gameObject.SetActive(false);

        //Destroy(t.gameObject);
    }

    IEnumerator Augment(Transform t)
    {
        float rot = 0;
        Vector3 currentRot;
        Quaternion currentQuaternionRot = new Quaternion();

        t.localScale = new Vector2(0, 0);
        t.gameObject.SetActive(true);

        while (t.localScale.x < 1.2f)
        {
            t.localScale = new Vector2(t.localScale.x + speed/100, t.localScale.y + speed/100);

            // NEW !!! ROTATION !!!
            rot -= 5;
            currentRot = new Vector3(0, 0, rot);
            currentQuaternionRot.eulerAngles = currentRot;
            t.localRotation = currentQuaternionRot;

            //yield return null;
            yield return new WaitForSeconds(0.01f);
        }

        //t.localScale = new Vector2(1f, 1f);
        t.gameObject.SetActive(true);

        //Destroy(t.gameObject);
    }

    private void SetStates(bool state)
    {
        foreach (Transform t in carres)
        {
            t.gameObject.SetActive(state);
        }
    }

    private RectTransform r;

    [ContextMenu("Spawn")]
    public void SetSquares()
    {
        r = transform.GetChild(0).GetComponent<RectTransform>();

        for(int j = 0; j < 9; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject a = Instantiate(r.gameObject, new Vector3(0, 0, 0), Quaternion.identity, transform);
                a.GetComponent<RectTransform>().localPosition = new Vector3(r.localPosition.x + (i * 50), r.localPosition.y - (50 * j), 0);

            }
        }

      
    }
}
