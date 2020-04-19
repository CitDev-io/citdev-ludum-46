using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGround : MonoBehaviour
{
    [SerializeField]
    public List<Tile> layableTiles;
    [SerializeField]
    public Tilemap theMap;

    void Start()
    {
        StartCoroutine(groundBuild());
    }

    IEnumerator groundBuild()
    {
        float xPos = 17f;
        while(true) 
         { 
            xPos += 1.25f;
            Vector3Int currentCell = theMap.WorldToCell(new Vector3(0 + xPos, -3.2f, 0f));
            theMap.SetTile(currentCell, layableTiles[Random.Range(0, layableTiles.Count)]);
            yield return new WaitForSeconds(0.1f);
         }
     }
}
