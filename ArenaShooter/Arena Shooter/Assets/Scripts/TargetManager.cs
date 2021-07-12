using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public AudioClip[] targetShotSounds;
    public int currentSoundIndex = 0;
    AudioSource audioSource;

    public Vector3 SpawnRange;
    public Vector3 SpawnPos; //bottom back left corner of spawn box

    public GameObject targetPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnTarget();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTarget()
    {
        float randX = Random.Range(0, SpawnRange.x) + SpawnPos.x;
        float randY = Random.Range(0, SpawnRange.y) + SpawnPos.y;
        float randZ = Random.Range(0, SpawnRange.z) + SpawnPos.z;

        GameObject target = Instantiate(targetPrefab, new Vector3(randX, randY, randZ), Quaternion.identity);

        target.transform.parent = this.transform;

    }

    public void PlaySound()
    {
        audioSource.clip = targetShotSounds[currentSoundIndex];
        audioSource.time = 0;
        audioSource.Play();
    }

    public void IncrementSound()
    {
        currentSoundIndex++;
        currentSoundIndex = Mathf.Clamp(currentSoundIndex, 0, targetShotSounds.Length-1);
    }
    
    public void ResetSound()
    {
        currentSoundIndex = 0;
    }
}
