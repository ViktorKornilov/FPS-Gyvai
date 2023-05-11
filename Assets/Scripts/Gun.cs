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
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryShoot();
        }
    }

    public void TryShoot()
    {
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
    
    
    
    // TASK
    // Make gun reload with clips. automatically fills clip with remaining bullets up to
    // maximum clip size.
    // 80 points
    
    // Pistol, Shotgun, Rifle gun models, with custom sounds and variables
    // 40 points

}
