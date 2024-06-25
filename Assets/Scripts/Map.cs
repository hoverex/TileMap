using UnityEngine;

public class Map : MonoBehaviour
{    
    [SerializeField] 
    private Vector2Int _size;
    private Tile[,] _tiles;
    
    private void Awake()
    {
        _tiles = new Tile[_size.x, _size.y];
    }

    public bool IsCellAvailable(Vector3Int index)
    {
        // Если индекс за пределами сетки - возвращаем false
        var isOutOfGrid = index.x < 0 || index.z < 0 || 
                          index.x >= _tiles.GetLength(0) || index.z >= _tiles.GetLength(1);
        if (isOutOfGrid)
        {
            return false;
        }

        // Возвращаем значение, свободна ли клетка в пределах сетки
        var isFree = _tiles[index.x, index.z] == null;
        return isFree;
    }

    public void SetTile(Vector3Int index, Tile tile)
    {
        _tiles[index.x, index.z] = tile;
    }
}