using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionColliders : MonoBehaviour
{
    public FlockUnit pOwningUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FlockUnit")
        {
            pOwningUnit.AddVisibleUnit(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "FlockUnit")
        {
            pOwningUnit.RemoveVisibleUnit(other.transform);
        }
    }
}
