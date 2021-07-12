using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    [HideInInspector]
    public bool isHooking = false;
    public float HookLength = 10;
    public float HookSpeed = 1;
    public float HookedReturnSpeed = 1;

    public Transform startPoint;
    Transform targetTransform;
    Vector3 currentDestination;
    bool trackDestination;
    float currentSpeed = 0;

    bool hookingOut = true;
    public bool enemyOnHook = false;
    public GameObject ThingThatIsOnHook = null;
    public float shootEnemyOffHookForce;
    public LineRenderer HookLineRender;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHooking)
        {
            
            if(trackDestination)
            {
                if (hookingOut)
                    currentDestination = targetTransform.position;
                else
                    currentDestination = startPoint.position;
            }

            if ((transform.position - currentDestination).sqrMagnitude < .09f)
            {
                if (hookingOut)
                {
                    hookingOut = false;
                    trackDestination = true;
                    currentDestination = startPoint.position;
                }
                else
                {
                    isHooking = false;
                    HookLineRender.enabled = false;
                }
            }
            
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, currentSpeed);
            HookLineRender.SetPosition(1, transform.localPosition);
        }
    }

    public void SendHook()
    {
        isHooking = true;
        hookingOut = true;
        GetComponent<Collider>().enabled = true;

        HookLineRender.enabled = true;

        currentSpeed = HookSpeed;

        Transform target = null;
        for (float i = -27; i <= 27; i += 9)
        {
            for (float j = -27; j <= 27; j += 9)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * .5f + i, Camera.main.pixelHeight * .5f + j - 6, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, HookLength))
                {
                    if (hit.collider.GetComponent<HookableScript>() != null)
                    {
                        target = hit.transform;
                        targetTransform = target;
                        trackDestination = true;
                        i = 100;
                        j = 100;
                    }

                }
            }
        }

        if (target == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * .5f, Camera.main.pixelHeight * .5f - 6, 0));
            trackDestination = false;
            currentDestination = ray.origin + ray.direction*HookLength;
        }
        
    }

    public void ShootEnemyOffOfHook()
    {
        //Physics.IgnoreCollision(ThingThatIsOnHook.GetComponent<Collider>(), GetComponent<Collider>());
        GetComponent<Collider>().enabled = false;
        ThingThatIsOnHook.GetComponent<HookableScript>().GetUnHooked(shootEnemyOffHookForce);
        
        enemyOnHook = false;
        ThingThatIsOnHook = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HookableScript>() != null && isHooking)
        {
            hookingOut = false;
            currentDestination = startPoint.position;
            trackDestination = true;
            enemyOnHook = true;
            ThingThatIsOnHook = other.gameObject;
            other.GetComponent<HookableScript>().GetHooked(transform);

            currentSpeed = HookedReturnSpeed;
            
        }
    }
}
