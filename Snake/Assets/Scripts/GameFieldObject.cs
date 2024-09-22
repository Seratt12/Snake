using UnityEngine;

public class GameFieldObject : MonoBehaviour
{
    // Позиция ячейки с частью змейки
    private Vector2Int _cellId;

    public void SetCellPosition(Vector2Int cellId, Vector2 position)
    {
        // Задаём переменной _cellId полученное значение
        _cellId = cellId;

        // Устанавливаем позицию части змейки на сцене
        transform.position = position;
    }

    public Vector2Int GetCellId()
    {
        // Возвращаем значение переменной _cellId
        return _cellId;
    }
}
