using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public GameObject prefab;
    public GameObject muzzleFlash;
    
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;

    AudioSource source;

    public int maxAmmo = 30;
    public int ammo;
    
    public int clipSize = 10;
    public int clip;
    public int damage = 10;

    public float fireRate = 5;
    public float cooldown;
    public bool autoFire;

    [Range(0,10)]public int bulletFireCount = 1;

    [Range(0,20f)]public float spreadAngle = 0.1f;
    private Camera cam;

    public UnityEvent onShoot;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        muzzleFlash.SetActive(false);

        ammo = maxAmmo;
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
        if (autoFire && Input.GetKey(KeyCode.Mouse0) )
        {
            TryShoot();
        }

        if (!autoFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryShoot();
        }

        cooldown -= Time.deltaTime;
    }

    public void TryShoot()
    {
        if (cooldown > 0) return;
        
        if (clip <= 0)
        {
            Reload();
        }

        if (clip <= 0)
        {
            source.PlayOneShot(emptySound);
            return;
        }
        
        clip -= 1;
        
        onShoot.Invoke();
            
        muzzleFlash.SetActive(true);
        Invoke(nameof(TurnOffMuzzleFlash),0.05f);
        source.PlayOneShot(shootSound);
            
        for (int i = 0; i < bulletFireCount; i++)
        {
            Shoot();
        }

        cooldown = 1/fireRate;
    }

    public void Reload()
    {
        var bulletsNeeded = clipSize - clip;
        var bulletToAdd = Mathf.Min(bulletsNeeded,ammo);
        clip += bulletToAdd;
        ammo -= bulletToAdd;
        
        source.PlayOneShot(reloadSound);
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
            var health = hit.transform.GetComponent<Health>();
            if (!health)
            {
                var obj = Instantiate(prefab, hit.point, Quaternion.Euler(0, 0, 0));
                obj.transform.forward = hit.normal;
                obj.transform.position += hit.normal * 0.02f;
            }
            else
            {
                health.Damage(damage);
            }
        }
    }

    public void TurnOffMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }
}
