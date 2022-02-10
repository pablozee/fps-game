using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float spread;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private int magazineSize = 10;
    [SerializeField] private int bulletsPerTap;
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private Animator anim;
    [SerializeField] private bool allowButtonHold;
    [SerializeField] private GameObject bloodSprayPrefab;
    [SerializeField] private TextMeshProUGUI text;

    private int currentAmmo;
    private int bulletsShot;
    private float nextTimeToFire = 0f;
    private bool shooting;
    private bool readyToShoot;
    private bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        currentAmmo = magazineSize;
        readyToShoot = true;
        text.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        reloading = false;
        anim.SetBool("Reloading", false);
        text.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        text.SetText(currentAmmo + " / " + magazineSize);
    }

    void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && !reloading) StartCoroutine(Reload());

        if (readyToShoot && shooting && !reloading && currentAmmo > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        };

        if (Input.GetButtonDown("Fire1") && currentAmmo <= 0) StartCoroutine(Reload());
    }

    void Shoot()
    {
        readyToShoot = false;

        currentAmmo--;

        if (bulletsShot == bulletsPerTap)
            muzzleFlash.Play();

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Instantiate(bloodSprayPrefab, hit.point, hit.transform.rotation);
            }
        }

        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && currentAmmo > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    IEnumerator Reload()
    {
        reloading = true;
        Debug.Log("Reloading");
        anim.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = magazineSize;
        reloading = false;
    }
}
