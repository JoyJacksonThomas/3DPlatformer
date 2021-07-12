using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    
    public int bulletsPerShot = 1;
    public float fireRate;
    public float burstFireDuration;
    public GameObject BulletPrefab;
    public float shotRadius;
    public Transform BulletSpawnTransform;
    public Transform GunModel;


    float currentTime;
    Vector2 SwayTargetAim;
    float SwayTargetPosX;
    Vector3 StartingLocalPosition;
    public float MaxSwayPosValue;

    public float LerpSwayAimValue;
    public float LerpSwayPosValue;
    public Vector2 LeanClampValue;

    AudioSource gunSound;
    public float randomPitchRange;

    public float BulletForce;
    public float UpwardsBulletForce;

    private void Awake()
    {
        gunSound = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartingLocalPosition = GunModel.localPosition;
        
    }


    void Update()
    {
        currentTime += Time.deltaTime;
    }

    public void LeanGunWithAim(float mouseX, float mouseY, float horizontalMove)
    {
        mouseX *= LeanClampValue.x;
        mouseY *= LeanClampValue.y;

        SwayTargetAim = Vector2.MoveTowards(SwayTargetAim, new Vector2(mouseX, mouseY), LerpSwayAimValue);

        Vector3 rot = transform.eulerAngles;
        GunModel.rotation = Quaternion.Euler(rot.x - SwayTargetAim.y, rot.y + SwayTargetAim.x, 0);

        SwayTargetPosX = Mathf.MoveTowards(SwayTargetPosX, horizontalMove, LerpSwayPosValue);
        SwayTargetPosX = Mathf.Clamp(SwayTargetPosX, -MaxSwayPosValue, MaxSwayPosValue);
        
        GunModel.localPosition = new Vector3(StartingLocalPosition.x + SwayTargetPosX, GunModel.localPosition.y, GunModel.localPosition.z);
    }

    public void Shoot(bool pressed = false, bool held = false, bool released = false)
    {
        if(pressed || held)
        {
            if(currentTime > fireRate)
            {
                currentTime = 0;
                //for(int i = 0; i < bulletsPerShot; i++)
                //{
                //    GameObject pew = GameObject.Instantiate(BulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);
                //    float randomX = Random.Range(-shotRadius, shotRadius);
                //    float randomY = Random.Range(-shotRadius, shotRadius);
                //
                //    pew.transform.forward = transform.right * randomX + transform.up*randomY + transform.forward*3;
                //}

                
                bool hitTarget = false;
                Collider targetCollider = null;
                for(float i = -27; i <= 27; i += 9)
                {
                    for (float j = -27; j <= 27; j += 9)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * .5f + i, Camera.main.pixelHeight * .5f + j - 6, 0));
                        RaycastHit hit;
                        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))
                        {
                            
                            if (hit.collider.tag == "Hookable")
                            {
                                i = 100;
                                j = 100;
                                hit.collider.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                                Vector3 vel = hit.collider.GetComponent<Rigidbody>().velocity ;
                                vel = new Vector3(vel.x, 0, vel.z);
                                hit.collider.GetComponent<Rigidbody>().velocity = vel;
                                hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * UpwardsBulletForce, ForceMode.Impulse);
                                hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(Camera.main.transform.forward * BulletForce,
                                                                                    hit.point,
                                                                                    ForceMode.Impulse);
                            }
                        }
                    }
                }

                float randomPitchOffset = Random.Range(-randomPitchRange, randomPitchRange);
                gunSound.pitch = 1 + randomPitchOffset;

                gunSound.Play();
            }
        }
    }
}
