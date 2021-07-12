using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{
    RectTransform rectTransform;

    Vector2 reticlePos;
    Vector2 offset;

    public float leanAmount;
    public float lerpValue;
    public float slowLerpValue;
    public float slowLerpThreshold;

    public int mouseArraySize;
    public float oneOverMouseArraySize;
    Vector2[] mouseArray;
    public Queue<Vector2> mouseQueue;

    public float clampVal;
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        reticlePos = rectTransform.position;

        mouseArray = new Vector2[mouseArraySize];

        mouseQueue = new Queue<Vector2>();
        for(int i = 0; i < mouseArraySize; i++)
        {
            mouseArray[i] = Vector2.zero;
            mouseQueue.Enqueue(Vector2.zero);
            
        }
        //oneOverMouseArraySize = 1 / mouseArraySize;

    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = reticlePos + offset;
    }

    public void LeanReticle(Vector2 mouse)
    {

        mouse *= leanAmount;

        mouseQueue.Dequeue();
        mouseQueue.Enqueue(mouse);

        Vector2 averageMouseVel = Vector2.zero;
        for(int i = 0; i < mouseArraySize; i++)
        {
            averageMouseVel += mouseQueue.ElementAt(i);
        }
        averageMouseVel *= oneOverMouseArraySize;

        averageMouseVel.x = Mathf.Clamp(averageMouseVel.x , -clampVal, clampVal);
        averageMouseVel.y = Mathf.Clamp(averageMouseVel.y, -clampVal, clampVal);

        float currentLerpValue = lerpValue;
        if(mouse.sqrMagnitude < slowLerpThreshold)
        {
            currentLerpValue = slowLerpValue;
        }

        offset = Vector2.MoveTowards(offset, averageMouseVel, currentLerpValue);
    }
}
