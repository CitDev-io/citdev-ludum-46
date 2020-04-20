using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateGround : MonoBehaviour
{
    [SerializeField]
    public List<Tile> layableTiles;
    [SerializeField]
    public List<Tile> layableBelowTiles;
    [SerializeField]
    public Tilemap theMap;

    void Start()
    {
        StartCoroutine(groundBuild());
    }

    IEnumerator groundBuild()
    {
        float xPos = 17f;
        while(xPos < 1000f) 
         { 
            xPos += 1.25f;
            Vector3Int currentCell = theMap.WorldToCell(new Vector3(0 + xPos, -2f, 0f));
            theMap.SetTile(currentCell, layableTiles[Random.Range(0, layableTiles.Count)]);

            Vector3Int belowCell = theMap.WorldToCell(new Vector3(0 + xPos, -3.5f, 0f));
            theMap.SetTile(belowCell, layableBelowTiles[Random.Range(0, layableBelowTiles.Count)]);

            Vector3Int belowCell2 = theMap.WorldToCell(new Vector3(0 + xPos, -5.0f, 0f));
            theMap.SetTile(belowCell2, layableBelowTiles[Random.Range(0, layableBelowTiles.Count)]);
            yield return new WaitForSeconds(0.01f);
         }
     }
}
