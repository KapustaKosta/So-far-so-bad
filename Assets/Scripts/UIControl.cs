using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameplayMenuPanel;
    [SerializeField] private GameObject gameOverMenuPanel;
    [SerializeField] private Image healthIndicator;
    [SerializeField] private Image radiationIndicator;

    private float healthIndicatorStartWidth = 1f;
    private float radiationIndicatorStartWidth = 1f;

    public float healthIndicatorWidth
    {
        get {
            return healthIndicator.rectTransform.rect.width/healthIndicatorStartWidth;
        }
        set
        {
            if(value < 0 || value > 1)
                Debug.LogError("�� � ���� ����������, �� ������� � healthIndicatorWidth �������� ������ �������. ��� ������ ������ healthIndicatorWidth ������ ���� % �������� ������ � ���������� �����.");
            float targetValue = value * healthIndicatorStartWidth;
            Rect rect = healthIndicator.rectTransform.rect;
            healthIndicator.rectTransform.sizeDelta = new Vector2(targetValue, rect.height);
        }
    }
    public float radiationIndicatorWidth
    {
        get
        {
            return radiationIndicator.rectTransform.rect.width/radiationIndicatorStartWidth;
        }
        set
        {
            if (value < 0 || value > 1)
                Debug.LogError("�� � ���� ����������, �� ������� � radiationIndicatorWidth �������� ������ �������. ��� ������ ������ radiationIndicatorWidth ������ ���� % �������� ������ � ���������� �����.");
            float targetValue = value * radiationIndicatorStartWidth;
            Rect rect = radiationIndicator.rectTransform.rect;
            radiationIndicator.rectTransform.sizeDelta = new Vector2(targetValue, rect.height);
        }
    }

    private void Start()
    {
        healthIndicatorStartWidth = healthIndicatorWidth;
        radiationIndicatorStartWidth = radiationIndicatorWidth;
        radiationIndicatorWidth = 0f;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        CloseGameplayMenu();
    }

    public void OpenGameplayMenu()
    {
        Time.timeScale = 1f;
        gameplayMenuPanel.SetActive(true);
        CloseMenu();
    }

    public void OpenGameOverMenu()
    {
        gameOverMenuPanel.SetActive(true);
        CloseGameplayMenu();
        CloseMenu();
    }

    public void CloseGameplayMenu()
    {
        gameplayMenuPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
