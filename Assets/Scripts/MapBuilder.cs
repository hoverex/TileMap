using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private Grid _grid;

    private Camera _camera;
    private Tile _currentTile;

    private void Awake()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// Данный метод вызывается автоматически при клике на кнопки с изображениями тайлов.
    /// В качестве параметра передается префаб тайла, изображенный на кнопке.
    /// Вы можете использовать префаб tilePrefab внутри данного метода.
    /// </summary>
    public void StartPlacingTile(GameObject tilePrefab)
    {
        if (_currentTile)
        {
            Destroy(_currentTile.gameObject);
        }

        var tileObject = Instantiate(tilePrefab, _map.transform);
        _currentTile = tileObject.GetComponent<Tile>();
    }

    private void Update()
    {
        if (_currentTile == null)
        {
            return;
        }

        // Получаем позицию мыши в координатах экрана
        var mousePosition = Input.mousePosition;
        // Создаем луч из позиции мыши в мировую позицию
        var ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            // Получаем мировую позицию мыши
            var worldPosition = hitInfo.point;
            // Получаем координаты клетки и позицию ее центра
            var cellPosition = _grid.WorldToCell(worldPosition);
            var cellCenterWorld = _grid.GetCellCenterWorld(cellPosition);

            // Ставим тайл в центр клетки
            _currentTile.transform.position = cellCenterWorld;

            // Проверяем доступность клетки и задаем соответствующий цвет тайлу
            var isAvailable = _map.IsCellAvailable(cellPosition);
            _currentTile.SetColor(isAvailable);

            if (!isAvailable)
            {
                return;
            }

            // При клике мыши устанавливаем тайл на игровое поле
            if (Input.GetMouseButtonDown(0))
            {
                _map.SetTile(cellPosition, _currentTile);
                _currentTile.ResetColor();
                _currentTile = null;
            }
        }
    }
}