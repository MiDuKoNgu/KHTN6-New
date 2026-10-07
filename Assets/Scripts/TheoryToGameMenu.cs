using UnityEngine;
using UnityEngine.UI;

public class TheoryToGameMenu : MonoBehaviour
{
    [Header("Màn lý thuyết")]
    public GameObject theoryScreen;

    [Header("Màn chọn game")]
    public GameObject gameMenu;

    [Header("Nút vào chơi")]
    public Button playButton;

    [Header("Nút quay lại map")]
    public Button backButton;
    public GameObject mapPanel;

    [Header("Môn của màn này")]
    public string subjectName = "Hóa học";   // đúng như cột monID trong sheet câu hỏi

    void Start()
    {
        if (theoryScreen == null)
            theoryScreen = gameObject;

        if (playButton != null)
        {
            playButton.onClick.AddListener(OpenGameMenu);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToMap);
        }
    }

    void OpenGameMenu()
    {
        GameSession.Subject = subjectName;   // báo cho game biết đang chơi môn nào

        if (theoryScreen != null)
            theoryScreen.SetActive(false);

        if (gameMenu != null)
            gameMenu.SetActive(true);
    }

    void BackToMap()
    {
        if (theoryScreen != null)
            theoryScreen.SetActive(false);

        if (mapPanel != null)
            mapPanel.SetActive(true);
    }
}