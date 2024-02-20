using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class OnlyWASDCameraFacade : MonoBehaviour
{

    private void Start()
    {
        EventCenter.Instance.TriggerEvent("StartWASDTarget", this, null);
    }
}
