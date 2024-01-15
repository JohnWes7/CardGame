using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ShipController mainShip;
    [SerializeField] ShipBuildController shipBuilding;
    [SerializeField] IPlayerState playerState;
}
