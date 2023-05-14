using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject particlePrefab;
    public GameObject plankPrefab;
    
    private void Awake()
    {
        var health = GetComponent<Health>();
        health.onDamage.AddListener(OnDamage);
        health.onDeath.AddListener(OnDeath);
    }
    
    private void OnDamage()
    {
        transform.Rotate(0,5,0);
        Instantiate(particlePrefab, transform.position, transform.rotation);
    }
    
    private void OnDeath()
    {
        var n = Random.Range(4, 8);
        for (int i = 0; i < n; i++)
        {
            var pos = transform.position + Random.insideUnitSphere;
            var rot = Quaternion.Euler(0,Random.Range(0,360),0);
            var plank = Instantiate(plankPrefab, pos, rot);
            
            plank.GetComponent<Rigidbody>().AddExplosionForce( 300, transform.position, 5);
        }
        
        //apply explosive force in that point
        
    }

}
