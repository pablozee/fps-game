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
    [SerializeField] private GameObject ammoDisplay;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private float cameraShakeMagnitude;
    [SerializeField] private bool isRPG;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletForce;
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioSource reloadAudioSource;

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
        ammoDisplay.SetActive(true);
    }

    private void OnEnable()
    {
        reloading = false;
        anim.SetBool("Reloading", false);
        ammoDisplay.SetActive(true);
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
        anim.SetBool("Recoil", true);
        cameraShake.StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));
        currentAmmo--;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        shootAudioSource.PlayOneShot(shootAudioSource.clip);

        if (!isRPG)
        {
            if (bulletsShot == bulletsPerTap)
                muzzleFlash.Play();

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodSprayPrefab, hit.point, hit.transform.rotation);
                }
            }
        } else
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
        }
        

        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && currentAmmo > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    void ResetShot()
    {
        anim.SetBool("Recoil", false);
        readyToShoot = true;
    }

    IEnumerator Reload()
    {
        reloading = true;
        Debug.Log("Reloading");
        anim.SetBool("Reloading", true);
        reloadAudioSource.PlayOneShot(reloadAudioSource.clip);


        yield return new WaitForSeconds(reloadTime - 0.25f);
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = magazineSize;
        reloading = false;
    }
}
