using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookableScript : MonoBehaviour
{
    Transform hookTransform;
    Quaternion hookedRotation;
    public float lerpSpeed;
    public ParticleSystem BloodParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hookTransform)
        {
            //transform.position = Vector3.MoveTowards(transform.position, hookTransform.position, lerpSpeed);
            //transform.rotation = hookTransform.rotation;
            transform.localPosition = Vector3.zero;
            transform.localRotation = hookedRotation;
        }
        
    }

    public void GetShot()
    {

    }
    public void GetHooked(Transform hooktrans)
    {
        hookTransform = hooktrans;
        transform.parent = hookTransform;
        transform.localPosition = Vector3.zero;

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        GetComponent<Collider>().enabled = false;
        hookedRotation = transform.localRotation;

        BloodParticle.transform.forward = transform.position + Camera.main.transform.up - Camera.main.transform.position;
        BloodParticle.transform.parent = transform;
        BloodParticle.transform.localPosition = new Vector3(0, .7f, 0);
        BloodParticle.Play();

        BloodParticle.transform.parent = null;
    }

    public void GetUnHooked(float unHookForce)
    {
        transform.parent = null;
        hookTransform = null;
        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * unHookForce, ForceMode.Impulse);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().enabled = true;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(gameObject.tag == "Hook")
    //    {
    //        if (gameObject.GetComponent<HookScript>().isHooking == false)
    //        {
    //            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
    //        }
    //    }
    //}
}
