using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGround : MonoBehaviour
{
    [SerializeField]
    public Tile layableTile;
    [SerializeField]
    public Tilemap theMap;

    int xPos = 0;

    void Start()
    {
        StartCoroutine(groundBuild());
    }

    IEnumerator groundBuild()
    {
        while(true) 
         { 
            Vector3Int currentCell = theMap.WorldToCell(new Vector3(0 + ++xPos, -2.2f, 0f));
            theMap.SetTile(currentCell, layableTile);
            yield return new WaitForSeconds(0.1f);
         }
     }
}
