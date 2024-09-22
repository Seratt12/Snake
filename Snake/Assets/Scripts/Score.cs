using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // ��������� � ������ ������� ����� � ��� ���� ������ ������� �������� ��� � ������ ����������
    private const string BestScoreKey = "BestScore";

    // ���������� ������� � �����
    private TextMeshProUGUI _scoreText;

    // ���������� ��� �������� �����
    private int _score;

    // ���������� ��� �������� ������� �����
    private int _bestScore;

    // �������� ���������� �����
    private Animation _scoreIncreaseAnimation;

    public void AddScore(int value)
    {
        // ������� � ����� SetScore() ����� _score � value
        SetScore(_score + value);

        PlayScoreIncreaseAnimation();
    }

    public void Restart()
    {
        // ������� � ����� SetScore() �������� 0 (���������� ����)
        SetScore(0);
    }

    public int GetScore()
    {
        // ���������� ������� ����
        return _score;
    }

    public int GetBestScore()
    {
        // ���������� ������ ����
        return _bestScore;
    }

    public void SetBestScore(int value)
    {
        // ����������� ������� ����� �������� value
        _bestScore = value;

        // ������� � ����� SaveBestScore() �������� value
        SaveBestScore(value);
    }

    private void Start()
    {
        // �������� ����� FillComponents()
        FillComponents();

        // ������� � ����� SetScore() �������� 0 (���������� ����)
        SetScore(0);

        // �������� ����� LoadBestScore()
        LoadBestScore();
    }

    private void FillComponents()
    {
        // ������� ��������� TextMeshProUGUI � �������� �������� ���� �������, �� ������� ����� ������ (� ��� ��� ����� Score), � ����������� �������� ���������� ���������� _scoreText
        _scoreText = GetComponentInChildren<TextMeshProUGUI>();

        // �����: �������� ��������� Animation �������, �� ������� ��������� ������ (� ��� ��� Score)
        _scoreIncreaseAnimation = GetComponent<Animation>();
    }

    private void PlayScoreIncreaseAnimation()
    {
        // ����������� �������� �����
        _scoreIncreaseAnimation.Play(PlayMode.StopAll);
    }

    private void SetScore(int value)
    {
        // ����������� �������� ����� �������� value
        _score = value;

        // ������� � ����� SetScoreText() �������� value
        SetScoreText(value);
    }

    private void SetScoreText(int value)
    {
        // ����������� value � ������ � ����������� ��� �������� text ���������� _scoreText
        _scoreText.text = value.ToString();
    }

    private void LoadBestScore()
    {
        // ����������� _bestScore ��������, ���������� � PlayerPrefs � ������ BestScoreKey
        _bestScore = PlayerPrefs.GetInt(BestScoreKey);
    }

    private void SaveBestScore(int value)
    {
        // ��������� value � PlayerPrefs � ������ BestScoreKey
        PlayerPrefs.SetInt(BestScoreKey, value);
    }
}
