
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject prefab;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray,out var hit))
            {
                print(hit.point);
                var obj = Instantiate(prefab,hit.point,Quaternion.Euler(0,0,0));
                obj.transform.forward = hit.normal;
                obj.transform.position += hit.normal * 0.02f;

            }
        }
    }
}
