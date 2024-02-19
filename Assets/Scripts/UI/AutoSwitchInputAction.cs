using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CustomInspector;

public class AutoSwitchInputAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [ReadOnly]
    [SerializeField] 
    private string originMapName;
    [FixedValues("UI", "Move", "Build", "Do Nothing")]
    [SerializeField]
    private string targetMapName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        originMapName = PlayerControllerSingleton.Instance.currentActionMap.name;
        PlayerControllerSingleton.Instance.SwitchCurrentActionMap(targetMapName);
        Debug.Log($"now action map: {PlayerControllerSingleton.Instance.currentActionMap.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerControllerSingleton.Instance.SwitchCurrentActionMap(originMapName);
        Debug.Log($"now action map: {PlayerControllerSingleton.Instance.currentActionMap.name}");
    }

}
