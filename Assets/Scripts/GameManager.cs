using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tm;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && tm != null)
        {
            TileBase tb = tm.GetTile(new Vector3Int(1, 1, 0));
            Debug.Log(tb);
            tb = tm.GetTile(new Vector3Int(0, 0, 0));
            Debug.Log(tb);
            
        }
    }
}
