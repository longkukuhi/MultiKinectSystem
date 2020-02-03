using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCameraController : MonoBehaviour
{

    private Vector3 vec3MouseStart;
    private Vector3 vec3MouseEnd;

    public float fMoveRate = 0.1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if ( Input.GetKey( KeyCode.W ) )
        {
            transform.position += transform.forward.normalized * fMoveRate;
        }
        if ( Input.GetKey( KeyCode.A ) )
        {
            transform.position -= transform.right.normalized * fMoveRate;
        }

        if ( Input.GetKey( KeyCode.S ) )
        {
            transform.position -= transform.forward.normalized * fMoveRate;
        }
        if ( Input.GetKey( KeyCode.D ) )
        {
            transform.position += transform.right.normalized * fMoveRate;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            vec3MouseStart = Input.mousePosition;
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            vec3MouseEnd = Input.mousePosition;
            transform.Rotate( new Vector3( -(vec3MouseEnd.y-vec3MouseStart.y)/Screen.height*90, (vec3MouseEnd.x-vec3MouseStart.x)/Screen.width*360, 0 ) );
            vec3MouseStart = vec3MouseEnd;
        }
    }
}
