using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class scr_controlCam : MonoBehaviour
{
    public CinemachineFreeLook movementCam;
    public CinemachineFreeLook idleCam;
    private GameObject mainPlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayerObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainPlayerObj.GetComponent<Animator>().GetBool("isWalking") == true)
        {
            movementCam.Priority = 10;
            idleCam.Priority = 5;
            idleCam.m_XAxis = movementCam.m_XAxis;
            idleCam.m_YAxis = movementCam.m_YAxis;
        }
        if (mainPlayerObj.GetComponent<Animator>().GetBool("isWalking") == false)
        {
            idleCam.Priority = 10;
            movementCam.Priority = 5;
            movementCam.m_XAxis = idleCam.m_XAxis;
            movementCam.m_YAxis = idleCam.m_YAxis;
        }
    }
}
