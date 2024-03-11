
using UnityEngine;
using TMPro;

public class LaserTagger : MonoBehaviour
{
    //tagger stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI text;

    public void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

    }

    private void Update()
    {
        MyInput();
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();

        }

    }

    private void Shoot()
    {
        readyToShoot = false;
        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calc direction with spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //add line here for enemy
            //if (rayHit.collider.CompareTag("Enemy"))
                //rayHit.collider.GetComponent<....>().TakeDamage(damage);
        }

        //graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Destroy(flash, 0.05f); // Destroy the muzzle flash after 0.5 seconds

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);

    }
    private void ResetShot()
    {
        readyToShoot = true;

    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);

    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;

    }
}
