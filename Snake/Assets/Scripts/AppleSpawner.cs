using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    // Скрипт изменения состояния игры
    public GameStateChanger GameStateChanger;

    // Скрипт игрового поля
    public GameField GameField;

    // Скрипт движения змейки
    public Snake Snake;

    // Префаб яблока
    public GameFieldObject ApplePrefab;

    // Шаги до появления следующего яблока
    public int StepsBeforeSpawn = 0;

    // Счётчик шагов
    private int _stepCounter = -1;

    // Текущий объект яблока
    private GameFieldObject _apple;

    // Анимация появления яблок
    private Animation _appleAnimation;


    public void CreateApple()
    {
        // Создаём новый экземпляр яблока
        _apple = Instantiate(ApplePrefab);

        // НОВОЕ: Получаем компонент Animation объекта _apple
        _appleAnimation = _apple.GetComponent<Animation>();

        // Устанавливаем следующее яблоко
        SetNextApple();
    }

    public void SetNextApple()
    {
        // Если текущего яблока нет
        if (!_apple)
        {
            // Возвращаемся из метода
            return;
        }
        // Если нет свободных клеток на поле
        if (!CheckHasEmptyCells())
        {
            // Завершаем игру
            GameStateChanger.EndGame();

            // Возвращаемся из метода
            return;
        }

        // НОВОЕ: Увеличиваем счётчик шагов
        _stepCounter++;

        // НОВОЕ: Если счётчик шагов меньше количества шагов до появления следующего яблока
        if (_stepCounter < StepsBeforeSpawn)
        {
            // НОВОЕ: Скрываем яблоко
            HideApple();

            // НОВОЕ: Выходим из метода
            return;
        }
        // НОВОЕ: Показываем яблоко
        ShowApple();

        PlayAppleAnimation();


        // Получаем количество свободных клеток
        int emptyCellsCount = GetEmptyCellsCount();

        // Создаём массив возможных клеток для появления яблока, размер которого равен количеству свободных клеток
        Vector2Int[] possibleCellsIds = new Vector2Int[emptyCellsCount];

        // Заводим счётчик для отслеживания индекса текущей свободной клетки
        int counter = 0;

        // Проходим по всем рядам поля
        for (int i = 0; i < GameField.CellsInRow; i++)
        {
            // Проходим по всем ячейкам в ряде
            for (int j = 0; j < GameField.CellsInRow; j++)
            {
                // Если текущая ячейка пуста
                if (GameField.GetCellIsEmpty(i, j))
                {
                    // Добавляем индекс текущей ячейки в массив возможных клеток
                    possibleCellsIds[counter] = new Vector2Int(i, j);

                    // Увеличиваем счётчик свободных клеток
                    counter++;
                }
            }
        }
        // Выбираем случайную клетку из массива возможных клеток для размещения нового яблока
        Vector2Int appleCellId = possibleCellsIds[Random.Range(0, possibleCellsIds.Length)];

        // Устанавливаем яблоко в выбранной клетке
        GameField.SetObjectCell(_apple, appleCellId);
    }

    public Vector2Int GetAppleCellId()
    {
        // Возвращаем индекс текущей клетки яблока
        return _apple.GetCellId();
    }

    public void Restart()
    {
        // Сбрасываем счётчик шагов
        _stepCounter = -1;

        // Устанавливаем следующее яблоко
        SetNextApple();
    }

    public void HideApple()
    {
        // Делаем яблоко невидимым
        SetActiveApple(false);
    }

    public void ShowApple()
    {
        // Обнуляем счётчик шагов
        _stepCounter = 0;

        // Делаем яблоко видимым
        SetActiveApple(true);
    }

    private void SetActiveApple(bool value)
    {
        // Устанавливаем видимость яблока в соответствии с переданным значением
        _apple.gameObject.SetActive(value);
    }

    private bool CheckHasEmptyCells()
    {
        // Возвращаем true, если свободных клеток больше 0
        return GetEmptyCellsCount() > 0;
    }

    private int GetEmptyCellsCount()
    {
        // Определяем длину змейки
        int snakePartsLength = Snake.GetSnakePartsLength();

        // Получаем общее количество клеток на поле
        int fieldCellsCount = GameField.CellsInRow * GameField.CellsInRow;

        // Возвращаем разницу между общим количеством клеток и длиной змейки
        return fieldCellsCount - snakePartsLength;
    }

    private void PlayAppleAnimation()
    {
        // Проигрываем анимацию яблока
        _appleAnimation.Play();
    }
}
