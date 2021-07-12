using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot()
    {
        transform.parent.GetComponent<TargetManager>().PlaySound();
        transform.parent.GetComponent<TargetManager>().IncrementSound();
        transform.parent.GetComponent<TargetManager>().SpawnTarget();
        Destroy(gameObject);
    }
}
