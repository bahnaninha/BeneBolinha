using UnityEngine;
using System.Collections.Generic; // List

/// <summary> 
/// Manages the main gameplay of the game 
/// </summary> 
public class GameManager : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed at")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tiles should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tiles to spawn with no obstacles")]
    public int initNoObstacles = 4;

    /// <summary> 
    /// Where the next tile should be spawned at. 
    /// </summary> 
    private Vector3 nextTileLocation;

    /// <summary> 
    /// How should the next tile be rotated? 
    /// </summary> 
    private Quaternion nextTileRotation;

    /// <summary> 
    /// Start is called before the first frame update 
    /// </summary> 
    private void Start()
    {
        // Set our starting point 
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;
        
        for (int i = 0; i < initSpawnNum; ++i)
        {
            // Spawn the next tile, passing a parameter to decide if obstacles should be spawned
            SpawnNextTile(i >= initNoObstacles); 
        }
    }

    /// <summary> 
    /// Will spawn a tile at a certain location and 
    /// setup the next position 
    /// </summary> 
    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
        // Figure out where and at what rotation we should spawn the next item 
        var nextTile = newTile.transform.Find("Next Spawn Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;

        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    private void SpawnObstacle(Transform newTile)
    {
        // Lista para armazenar os pontos possíveis de spawn do obstáculo
        var obstacleSpawnPoints = new List<GameObject>();

        // Percorre todos os filhos do tile
        foreach (Transform child in newTile)
        {
            // Se o objeto tem a tag "ObstacleSpawn", adiciona à lista de possíveis spawn points
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }

        // Certifica-se de que há pelo menos um ponto de spawn disponível
        if (obstacleSpawnPoints.Count > 0)
        {
            // Escolhe um ponto de spawn aleatório da lista
            int index = Random.Range(0, obstacleSpawnPoints.Count);
            var spawnPoint = obstacleSpawnPoints[index];

            // Obtém a posição para spawnar o obstáculo
            var spawnPos = spawnPoint.transform.position;

            // Cria o obstáculo na posição determinada
            var newObstacle = Instantiate(obstacle, spawnPos, Quaternion.identity);

            // Define o tile como pai do obstáculo
            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}
