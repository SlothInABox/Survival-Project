using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform obstaclePrefab;
    [SerializeField] private Transform navMeshFloor;
    [SerializeField] private Transform navMeshMaskPrefab;
    public Vector2 mapSize;
    public Vector2 maximumMapSize;

    [Range(0, 1)]
    [SerializeField] float outlinePercent;

    [Range(0, 1)]
    [SerializeField] private float obstaclePercent;

    [SerializeField] private float tileSize;

    private List<Coordinate> allTileCoords;
    private Queue<Coordinate> shuffledTileCoords;

    public int seed = 10;

    private Coordinate mapCentre;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coordinate>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coordinate(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coordinate>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCentre = new Coordinate((int)mapSize.x / 2, (int)mapSize.y / 2);

        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.SetParent(transform);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.SetParent(mapHolder);
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coordinate randomCoordinate = GetRandomCoordinate();
            obstacleMap[randomCoordinate.x, randomCoordinate.y] = true;
            currentObstacleCount++;
            if (randomCoordinate != mapCentre && MapIsAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordinateToPosition(randomCoordinate.x, randomCoordinate.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newObstacle.SetParent(mapHolder);
            }
            else
            {
                obstacleMap[randomCoordinate.x, randomCoordinate.y] = false;
                currentObstacleCount--;
            }
        }

        Transform maskLeft = Instantiate(navMeshMaskPrefab, Vector3.left * (mapSize.x + maximumMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        maskLeft.SetParent(mapHolder);
        maskLeft.localScale = new Vector3((maximumMapSize.x - mapSize.x) / 2, 1, mapSize.y) * tileSize;

        Transform maskRight = Instantiate(navMeshMaskPrefab, Vector3.right * (mapSize.x + maximumMapSize.x) / 4 * tileSize, Quaternion.identity) as Transform;
        maskRight.SetParent(mapHolder);
        maskRight.localScale = new Vector3((maximumMapSize.x - mapSize.x) / 2, 1, mapSize.y) * tileSize;

        Transform maskTop = Instantiate(navMeshMaskPrefab, Vector3.forward * (mapSize.y + maximumMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        maskTop.SetParent(mapHolder);
        maskTop.localScale = new Vector3(maximumMapSize.x, 1, (maximumMapSize.y - mapSize.y)/2) * tileSize;

        Transform maskBottom = Instantiate(navMeshMaskPrefab, Vector3.back * (mapSize.y + maximumMapSize.y) / 4 * tileSize, Quaternion.identity) as Transform;
        maskBottom.SetParent(mapHolder);
        maskBottom.localScale = new Vector3(maximumMapSize.x, 1, (maximumMapSize.y - mapSize.y) / 2) * tileSize;

        navMeshFloor.localScale = new Vector3(maximumMapSize.x, maximumMapSize.y) * tileSize;
    }

    private bool MapIsAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0),obstacleMap.GetLength(1)];
        Queue<Coordinate> queue = new Queue<Coordinate>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coordinate tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 ^ y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coordinate(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private Vector3 CoordinateToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y) * tileSize;
    }

    public Coordinate GetRandomCoordinate()
    {
        Coordinate randomCoordinate = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoordinate);
        return randomCoordinate;
    }

    public struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coordinate c1, Coordinate c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coordinate c1, Coordinate c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate coordinate)
            {
                return this == coordinate;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode() => new { x, y }.GetHashCode();
    }
}
