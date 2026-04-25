using UnityEngine;

public class LookAtGameObject : MonoBehaviour
{

    [SerializeField]
    private GameObject target;
    
    void Update()
    {
        transform.LookAt(target.transform.position);
    }
}
