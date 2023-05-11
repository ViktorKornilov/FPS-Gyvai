using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject prefab;
    public GameObject muzzleFlash;
    public AudioClip shootSound;

    AudioSource source;

    public int maxAmmo = 30;
    public int ammo;
    
    public int clipSize = 10;
    public int clip;

    [Range(0,10)]public int bulletFireCount = 1;

    [Range(0,20f)]public float spreadAngle = 0.1f;
    private Camera cam;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        muzzleFlash.SetActive(false);

        ammo = maxAmmo;
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          TryShoot();
        }
    }

    public void TryShoot()
    {
        if (ammo <= 0) return;
        ammo--;
            
        muzzleFlash.SetActive(true);
        Invoke(nameof(TurnOffMuzzleFlash),0.05f);
        source.PlayOneShot(shootSound);
            
        for (int i = 0; i < bulletFireCount; i++)
        {
            Shoot();
        }
    }

    public void Shoot()
    {

        // ROTATE BY RANDOM SPREAD ANGLE
        var direction = cam.transform.forward;
        var xAngle = Random.Range(-spreadAngle, spreadAngle);
        var yAngle = Random.Range(-spreadAngle,spreadAngle);
        var result = Quaternion.Euler(xAngle, yAngle, 0) * direction;
        var ray = new Ray(cam.transform.position, result);
        

        if (Physics.Raycast(ray,out var hit))
        {
            if (!hit.transform.CompareTag("Enemy"))
            {
                var obj = Instantiate(prefab, hit.point, Quaternion.Euler(0, 0, 0));
                obj.transform.forward = hit.normal;
                obj.transform.position += hit.normal * 0.02f;
            }
        }
    }

    public void TurnOffMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }
}
