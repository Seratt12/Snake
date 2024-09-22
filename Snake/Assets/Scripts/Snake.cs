using UnityEngine;

public class Snake : MonoBehaviour
{

    // ������ ��������� �������� �����
    public AppleSpawner BonusAppleSpawner;

    // ������ ������� �����
    public Score Score;

    // ������ ��������� ��������� ����
    public GameStateChanger GameStateChanger;

    // ������ ��������� �����
    public AppleSpawner AppleSpawner;

    // ���� ������
    public float ExplosionForce = 60;


    // ������ �������� ����
    public GameField GameField;

    // ������ ������ ������
    public GameFieldObject HeadPrefab;

    // ������ ���� ������
    public GameFieldObject BodyPrefab;

    // ��������� ��������� ������ � ���� ��������� ������
    // ����� (5, 5) �� ������� ��������, � ��� ��� ����� ����������
    public Vector2Int StartCellId = new Vector2Int(5, 5);

    // �������� ����� ���������� ������
    // ����� �� ������� ��������� ��� �������� � ����������
    public float MoveDelay = 1.3f;

    // ���� ���������� ������
    private bool _isActive;

    // ������ ������ ������
    private GameFieldObject[] _parts;

    // ����������� ��������
    private Vector2Int _moveDirection = Vector2Int.up;

    // ������ ��������
    private float _moveTimer;


    public void StartGame()
    {
        // ������ ������
        CreateSnake();

        // ������������� ���� ����������
        _isActive = true;
    }

    public void StopGame()
    {
        // ������� ���� ����������
        _isActive = false;
    }

    public int GetSnakePartsLength()
    {
        // ���������� ����� ������
        return _parts.Length;
    }

    public void RestartGame()
    {
        // ������� ����� ������
        DestroySnake();

        // �������� ���� ������
        StartGame();
    }

    private void DestroySnake()
    {
        // �������� �� ������ ������
        for (int i = 0; i < _parts.Length; i++)
        {
            // ������� ������ �����
            Destroy(_parts[i].gameObject);
        }
    }

    private void CreateSnake()
    {
        // ������ ������ ������ ������ � ������ 0
        _parts = new GameFieldObject[0];

        // ��������� ������ ������ �� �������� �������
        AddPart(HeadPrefab, StartCellId);

        // ��������� ���� ����� ���� ������ �� �������, ������� ��������� �� ���� ������ ���� �� ������
        AddPart(BodyPrefab, StartCellId + Vector2Int.down);
    }

    private void CheckNextCellFail(Vector2Int nextCellId)
    {
        // �������� �� ������ ������
        for (int i = 1; i < _parts.Length; i++)
        {
            // ���� ������ (���������) ����������� � ����� ����� ������ ��������� � ����������� �������� (����������) ������
            // �� ���� ���� ������ ��������� �� ���� ����
            if (_parts[i].GetCellId() == nextCellId)
            {
                // ��������� ����
                GameStateChanger.EndGame();

                // �����: �������� ������� ���� ����� ������ �� ������� ����
                Vector2 partPosition = GameField.GetCellPosition(_parts[i].GetCellId());

                // �����: �������� ������ �� ������� ���� �����
                ExplodeSnake(partPosition);
            }
        }
    }

    private void ExplodeSnake(Vector2 explodePosition)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < _parts.Length; i++)
        {
            // �������� ��������� Rigidbody2D ���� ����� ������
            Rigidbody2D partRigid = _parts[i].GetComponent<Rigidbody2D>();

            // �������� ��� �� ��������� ������
            partRigid.simulated = true;

            // �������� � ������� �� ������� ����
            Vector2 partPosition = GameField.GetCellPosition(_parts[i].GetCellId());

            // ��������� ����������� �� ���� ����� ������ �� �������� ������� ������
            Vector2 explodeDirection = partPosition - explodePosition;

            // ������ ���� ���� ����� ������, ����� ��� �������� �� ������� ������
            // ���� ���������� � ��� � ������ �����. ��� ������������� � ������� ExplosionForce
            partRigid.AddForce((explodeDirection.normalized + Vector2.up) * ExplosionForce, ForceMode2D.Impulse);
        }
    }

    private void CheckNextCellApple(Vector2Int nextCellId, Vector2Int cellIdForAddPart)
    {
        if (AppleSpawner.GetAppleCellId() == nextCellId)
        {
            AddPart(BodyPrefab, cellIdForAddPart);
            AppleSpawner.SetNextApple();

            // �����: ������������� ������� ��� ������
            BonusAppleSpawner.SetNextApple();

            Score.AddScore(1);
        }
        // �����: �����, ���� ���������� ������ � ������� ��������� �� ��������� �������, ���� ����� ������
        else if (BonusAppleSpawner.GetAppleCellId() == nextCellId)
        {
            // �����: ����� ���������� ������ ������, ������� ����� ������� (�� ������ ����� ������� ����� ��� ��������)
            int countToRemove = 2;

            // �����: �������� �� ���� ������
            for (int i = 0; i < countToRemove; i++)
            {
                // �����: ������� ��������� ����� ������
                RemovePart();
            }
            // �����: �������� �������� ������
            BonusAppleSpawner.HideApple();
        }
    }

    private void RemovePart()
    {
        // ������� ������ ��������� ����� ������
        Destroy(_parts[_parts.Length - 1].gameObject);

        // ��������� ����� ������� ������ ������ �� 1
        ChangePartsArrayLenght(-1);
    }

    private void AddPart(GameFieldObject partPrefab, Vector2Int cellId)
    {
        // �������� ������ ������� ������ ������ �� 1
        ChangePartsArrayLenght(1);

        GameFieldObject newSnakePart = Instantiate(partPrefab);
        _parts[_parts.Length - 1] = newSnakePart;

        // �����: ������������� ����� ����� ������ � ��������� ������ �� ������� ����
        GameField.SetObjectCell(newSnakePart, cellId);
    }

    private void ChangePartsArrayLenght(int count)
    {
        GameFieldObject[] tempParts = _parts;

        // �����: ��������� � [] �������� count ������ 1
        _parts = new GameFieldObject[tempParts.Length + count];

        // �����: ���������� _parts ������ tempParts
        for (int i = 0; i < _parts.Length; i++)
        {
            // �����: ���� ������� ������ ��������� ���������� ������ ������ � �������� �������
            if (i >= tempParts.Length)
            {
                // �����: ������� �� �����
                break;
            }
            _parts[i] = tempParts[i];
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            // ������� �� ������
            return;
        }

        // �������� ����������� �������� �� ������������
        GetMoveDirection();

        // ���������, ���������� �� ������ �������, ����� ������� ������
        MoveTimerTick();
    }

    private void GetMoveDirection()
    {
        // ���� ������������ ����� �� ������� W ��� ������� ����� � ������� ����������� �� ����
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _moveDirection != Vector2Int.down)
        {
            // ������������� ����������� �������� �����
            SetMoveDirection(Vector2Int.up);
        }
        // ���� ������������ ����� �� ������� A ��� ������� ����� � ������� ����������� � �� ������
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _moveDirection != Vector2Int.right)
        {
            // ������������� ����������� �������� �����
            SetMoveDirection(Vector2Int.left);
        }
        // ���� ������������ ����� �� ������� S ��� ������� ���� � ������� ����������� �� �����
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _moveDirection != Vector2Int.up)
        {
            // ������������� ����������� �������� ����
            SetMoveDirection(Vector2Int.down);
        }
        // ���� ������������ ����� �� ������� D ��� ������� ������ � ������� ����������� � �� �����
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _moveDirection != Vector2Int.left)
        {
            // ������������� ����������� �������� ������
            SetMoveDirection(Vector2Int.right);
        }
    }

    private void SetMoveDirection(Vector2Int moveDirection)
    {
        // ������������� ����� ����������� ��������
        _moveDirection = moveDirection;

        // ������������ ������ ������ �� ������ �����������
        SetHeadRotation(moveDirection);

        // ������� ������ �� �����
        Move();
    }
    private void MoveTimerTick()
    {
        // ����������� �������� ������� �� �����, ������� ������ � ���������� �����
        _moveTimer += Time.deltaTime;

        // ���� �������� ������� �������� �������� ��������
        if (_moveTimer >= MoveDelay)
        {
            // ������� ������ �� �����
            Move();
        }
    }

    private void Move()
    {
        _moveTimer = 0;

        // �����: �������� ������ ��������� ����� ������
        Vector2Int lastPartCellId = _parts[_parts.Length - 1].GetCellId();

        // �����: �������� ����� ������ ��� ������ ������ � ����������� �� �������� �����������
        Vector2Int headNewCell = MoveCellId(_parts[0].GetCellId(), _moveDirection);

        // �����: ����������� ������ ��������� ����� ������ �� ������� ����
        GameField.SetCellIsEmpty(lastPartCellId.x, lastPartCellId.y, true);

        for (int i = _parts.Length - 1; i >= 0; i--)
        {
            Vector2Int partCellId = _parts[i].GetCellId();

            if (i == 0)
            {
                // �����: ����� ����� ������ ��� ������ ������
                partCellId = headNewCell;
            }
            else
            {
                partCellId = _parts[i - 1].GetCellId();
            }
            // �����: ������������� ������� ����� ������ � ����� ������ �� ������� ����
            GameField.SetObjectCell(_parts[i], partCellId);
        }
        // �����: ���������, ���� �� � ��������� ������ ������������
        CheckNextCellFail(headNewCell);

        // �����: ���������, ���� �� � ��������� ������ ������
        CheckNextCellApple(headNewCell, lastPartCellId);
    }

    private Vector2Int MoveCellId(Vector2Int cellId, Vector2Int direction)
    {
        // ����������� �������� cellId �� �������� direction, ����� �������� ����� ������� ������ ������
        cellId += direction;

        // ���� ����� ������� �� ��� x ������ ��� ����� ���������� ������ � ���� �������� ����
        if (cellId.x >= GameField.CellsInRow)
        {
            // �������� x, ����� ������ ������������� �� ������ ����
            cellId.x = 0;
        }
        // �����, ���� ����� ������� �� ��� x ������ 0
        else if (cellId.x < 0)
        {
            // ������ x ������ ���������� ������ � ���� �������� ���� ����� 1, ����� ������ ������������� � ����� ����
            cellId.x = GameField.CellsInRow - 1;
        }
        // ���� ����� ������� �� ��� y ������ ��� ����� ���������� ������ � ���� �������� ����
        if (cellId.y >= GameField.CellsInRow)
        {
            // �������� y, ����� ������ ������������� �� ������ ����
            cellId.y = 0;
        }
        // �����, ���� ����� ������� �� ��� y ������ 0
        else if (cellId.y < 0)
        {
            // ������ y ������ ���������� ������ � ���� �������� ���� ����� 1, ����� ������ ������������� � ����� ����
            cellId.y = GameField.CellsInRow - 1;
        }
        // ���������� ����� �������
        return cellId;
    }

    private void SetHeadRotation(Vector2Int direction)
    {
        // ������ ����� ������, ������� ����� �������������� ��� �������� ������ ������
        Vector3 headEuler = Vector3.zero;

        // ���� ����������� �������� �� ��� x ����� 0 � ����������� �������� �� ��� y ����� 1
        if (direction.x == 0 && direction.y == 1)
        {
            // ������ ������ ������ (0, 0, 0), ����� ������ ������ ���� ���������� �����
            headEuler = new Vector3(0f, 0f, 0f);
        }
        // �����, ���� ����������� �������� �� ��� x ����� 1 � ����������� �������� �� ��� y ����� 0
        else if (direction.x == 1 && direction.y == 0)
        {
            // ������ ������ ������ (0, 0, -90), ����� ������ ������ ���� ���������� ������
            headEuler = new Vector3(0f, 0f, -90f);
        }
        // �����, ���� ����������� �������� �� ��� x ����� 0 � ����������� �������� �� ��� y ����� -1
        else if (direction.x == 0 && direction.y == -1)
        {
            // ������ ������ ������ (0, 0, 180), ����� ������ ������ ���� ���������� ����
            headEuler = new Vector3(0f, 0f, 180f);
        }
        // �����, ���� ����������� �������� �� ��� x ����� -1 � ����������� �������� �� ��� y ����� 0
        else if (direction.x == -1 && direction.y == 0)
        {
            // ������ ������ ������ (0, 0, 90), ����� ������ ������ ���� ���������� �����
            headEuler = new Vector3(0f, 0f, 90f);
        }
        // ������������ ���� �������� ������ ������ � �������
        _parts[0].transform.eulerAngles = headEuler;
    }


}
