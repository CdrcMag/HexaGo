using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : Enemy
{
    private const float ROTATE_SPEED_CANON = 10f;
    private const float ADDSCALE = 0.04f;

    // ===================== VARIABLES =====================

    private float delay = 4f;
    private float bulletSpeed = 11f;
    private bool isGrowingBody = false;
    private bool isGrowingBuoy = false;
    private bool hasBuoy = true;

    [Header("Components")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform buoy;
    [SerializeField] private Transform canon;
    [SerializeField] private Transform posToShoot;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private Transform staticAnchor;

    // =====================================================

    private void Awake()
    {
        base.SetTargetInStart();
        base.SetInitialSpeed(GetSpeed());
    }

    private void Start()
    {
        StartCoroutine(ShootByDelay());
    }

    private void Update()
    {
        base.RotateToward(ROTATE_SPEED_CANON, canon);
    }

    public override void TakeDamage(float _damage, bool _b)
    {
        base.TakeDamage(_damage);

        if (GetLifePoint() <= 100 && hasBuoy)
        {
            hasBuoy = false;
            Destroy(buoy.gameObject);
        }

        if (!isGrowingBuoy && hasBuoy)
            StartCoroutine(GrowBuoy());

        if (!isGrowingBody && !hasBuoy)
            StartCoroutine(GrowBody());
    }

    private IEnumerator GrowBody()
    {
        isGrowingBody = true;

        while (body.localScale.x < 1f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x + ADDSCALE, body.localScale.y + ADDSCALE);
        }

        while (body.localScale.x > 0.6f)
        {
            yield return new WaitForSeconds(0.005f);
            body.localScale = new Vector2(body.localScale.x - ADDSCALE, body.localScale.y - ADDSCALE);
        }

        isGrowingBody = false;
    }

    private IEnumerator GrowBuoy()
    {
        isGrowingBuoy = true;

        while (buoy != null && buoy.localScale.x < 0.7f)
        {
            yield return new WaitForSeconds(0.005f);

            if(buoy != null)
                buoy.localScale = new Vector2(buoy.localScale.x + ADDSCALE, buoy.localScale.y + ADDSCALE);
        }

        while (buoy != null && buoy.localScale.x > 0.5f)
        {
            yield return new WaitForSeconds(0.005f);

            if (buoy != null)
                buoy.localScale = new Vector2(buoy.localScale.x - ADDSCALE, buoy.localScale.y - ADDSCALE);
        }

        isGrowingBuoy = false;
    }

    private IEnumerator GrowCanon()
    {
        while (canon.localScale.x < 1.2f)
        {
            yield return new WaitForSeconds(0.01f);
            canon.localScale = new Vector2(canon.localScale.x + ADDSCALE, canon.localScale.y + ADDSCALE);
        }

        while (canon.localScale.x > 0.9f)
        {
            yield return new WaitForSeconds(0.01f);
            canon.localScale = new Vector2(canon.localScale.x - ADDSCALE, canon.localScale.y - ADDSCALE);
        }
    }

    private IEnumerator ShootByDelay()
    {
        yield return new WaitForSeconds(1);

        staticAnchor.gameObject.SetActive(true);
        StartCoroutine(SetAnchor());

        delay = Random.Range(2f, 3.5f);
        yield return new WaitForSeconds(delay);

        staticAnchor.gameObject.SetActive(false);
        Shoot(bulletPref, posToShoot, canon, bulletSpeed);

        StartCoroutine(ShootByDelay());
    }

    private IEnumerator SetAnchor()
    {
        while (staticAnchor.localScale.x < 1.1f)
        {
            yield return new WaitForSeconds(0.01f);
            staticAnchor.localScale = new Vector2(staticAnchor.localScale.x + ADDSCALE, staticAnchor.localScale.y + ADDSCALE);
        }

        while (staticAnchor.localScale.x > 0.9f)
        {
            yield return new WaitForSeconds(0.01f);
            staticAnchor.localScale = new Vector2(staticAnchor.localScale.x - ADDSCALE, staticAnchor.localScale.y - ADDSCALE);
        }
    }

    public override void Shoot(GameObject _bulletPref, Transform _posToShoot, Transform _canon, float _speed)
    {
        //base.Shoot(_bulletPref, _posToShoot, _canon, _speed); // Ancienne m?thode de tir

        GameObject bullet;
        bullet = Instantiate(_bulletPref, _posToShoot.position, _canon.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * _speed;
        Destroy(bullet, 10f);
        bullet.transform.localScale = new Vector2(transform.localScale.x / 1.1f, transform.localScale.y / 1.1f); // La taille de l'ancre lanc? d?pend de la taille de l'ennemi "Anchor"

        StartCoroutine(GrowCanon());
    }
}
