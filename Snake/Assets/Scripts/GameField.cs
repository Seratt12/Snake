using UnityEngine;

public class GameField : MonoBehaviour
{

    // Позиция первой ячейки
    public Transform FirstCellPoint;

    // Размер ячейки (по X и Y)
    public Vector2 CellSize;

    // Количество ячеек в одном ряду
    public int CellsInRow = 12;

    // Двумерный массив из позиций каждой ячейки
    private GameFieldCell[,] _cells;


    public void FillCellsPositions()
    {
        // Создаём массив ячеек размерностью CellsInRow x CellsInRow
        _cells = new GameFieldCell[CellsInRow, CellsInRow];

        for (int i = 0; i < CellsInRow; i++)
        {
            for (int j = 0; j < CellsInRow; j++)
            {
                // Вычисляем позицию текущей ячейки
                Vector2 cellPosition = (Vector2)FirstCellPoint.position + Vector2.right * i * CellSize.x + Vector2.up * j * CellSize.y;

                // Создаём новую ячейку
                GameFieldCell newCell = new GameFieldCell(cellPosition);

                // Записываем эту ячейку в массив _cells
                _cells[i, j] = newCell;
            }
        }
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        // Получаем ячейку по заданным координатам
        GameFieldCell cell = GetCell(x, y);

        // Если ячейка не была найдена, возвращаем (0, 0)
        if (cell == null)
        {
            return Vector2.zero;
        }
        // Возвращаем позицию найденной ячейки
        return _cells[x, y].GetPosition();
    }

    public void SetObjectCell(GameFieldObject obj, Vector2Int newCellId)
    {
        // Получаем позицию ячейки по заданным координатам
        Vector2 cellPosition = GetCellPosition(newCellId.x, newCellId.y);

        // Устанавливаем объект на найденную ячейку
        obj.SetCellPosition(newCellId, cellPosition);

        // Задаём значение занятости ячейки
        SetCellIsEmpty(newCellId.x, newCellId.y, false);
    }

    public Vector2 GetCellPosition(Vector2Int cellId)
    {
        // Возвращаем позицию указанной ячейки
        return GetCellPosition(cellId.x, cellId.y);
    }


    public bool GetCellIsEmpty(int x, int y)
    {
        // Получаем ячейку по указанным координатам
        GameFieldCell cell = GetCell(x, y);

        // Если ячейка не была найдена
        if (cell == null)
        {
            // Возвращаем false
            return false;
        }
        // Возвращаем значение занятости найденной ячейки
        return cell.GetIsEmpty();
    }

    public void SetCellIsEmpty(int x, int y, bool value)
    {
        // Получаем ячейку по указанным координатам
        GameFieldCell cell = GetCell(x, y);

        // Если ячейка не была найдена
        if (cell == null)
        {
            // Выходим из метода
            return;
        }
        // Устанавливаем значение занятости ячейки
        _cells[x, y].SetIsEmpty(value);
    }

    private GameFieldCell GetCell(int x, int y)
    {
        // Если координаты выходят за границы игрового поля
        if (x < 0 || y < 0 || x >= CellsInRow || y >= CellsInRow)
        {
            // Возвращаем null
            return null;
        }
        // Возвращаем ячейку с заданными координатами
        return _cells[x, y];
    }

}
