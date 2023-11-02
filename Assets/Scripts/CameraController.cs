using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BallController target;

    private float offset; // Keep initial distance between cam and ball




    //cette m�thode est  appel� avant la m�thode Start()
    void Awake()
    {
        offset = transform.position.y - target.transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        curPos.y = target.transform.position.y + offset;
        transform.position = curPos;
    }
}
