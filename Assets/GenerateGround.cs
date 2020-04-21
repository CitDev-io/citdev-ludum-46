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
    float terrainXcovered = 0f;
    public int sectionsPerCut = 100;

    void Start()
    {
        groundBuild(17f, sectionsPerCut);
    }

    void LateUpdate() {
        if (Camera.main.transform.position.x + sectionsPerCut > terrainXcovered) {
            groundBuild(terrainXcovered, sectionsPerCut);
        }
    }

    void groundBuild(float startX, int distance)
    {
        terrainXcovered = startX + distance;
        float xPos = startX;

        int chasmSize = Random.Range(3, 6);
        int spotToStart = Random.Range(1, ( (int) Mathf.Floor((float) distance / 1.25f)) - chasmSize - 2);

        int index = 0;
        while(xPos < startX + distance) 
         {
            bool thisIsAGap = (index == spotToStart || (index > spotToStart && index < chasmSize + spotToStart));
            xPos += 1.25f;
            if (!thisIsAGap) {
                Vector3Int currentCell = theMap.WorldToCell(new Vector3(0 + xPos, -2f, 0f));
                theMap.SetTile(currentCell, layableTiles[Random.Range(0, layableTiles.Count)]);

                Vector3Int belowCell = theMap.WorldToCell(new Vector3(0 + xPos, -3.5f, 0f));
                theMap.SetTile(belowCell, layableBelowTiles[Random.Range(0, layableBelowTiles.Count)]);

                Vector3Int belowCell2 = theMap.WorldToCell(new Vector3(0 + xPos, -5.0f, 0f));
                theMap.SetTile(belowCell2, layableBelowTiles[Random.Range(0, layableBelowTiles.Count)]);
            }
            index++;
         }
     }
}
