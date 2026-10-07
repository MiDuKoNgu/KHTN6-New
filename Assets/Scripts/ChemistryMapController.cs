using UnityEngine;
using UnityEngine.UI;

public class ChemistryMapController : MonoBehaviour
{
    [Header("Màn bản đồ Hóa học")]
    public GameObject mapPanel;

    [Header("Các chặng Hóa học")]
    public Button[] chapterButtons;

    [Header("Panel lý thuyết")]
    public TheoryPanel theoryPanel;

    [Header("Nút quay lại trong theory_chemistry")]
    public Button backTheoryButton;

    [Header("Môn (khớp cột monID trong sheet câu hỏi)")]
    public string subjectName = "Hóa học";

    void Start()
    {
        if (mapPanel == null)
            mapPanel = gameObject;

        for (int i = 0; i < chapterButtons.Length; i++)
        {
            int chapter = i + 1;

            if (chapterButtons[i] != null)
            {
                chapterButtons[i].onClick.AddListener(() =>
                {
                    OpenTheory(chapter);
                });
            }
        }

        if (backTheoryButton != null)
        {
            backTheoryButton.onClick.AddListener(CloseTheory);
        }
    }

    void OpenTheory(int chapter)
    {
        string id = "CH_CHM_0" + chapter;

        Debug.Log("Đang mở bài: " + id);

        // Báo cho game biết môn và chương người chơi vừa chọn
        GameSession.Subject = subjectName;
        GameSession.Chapter = "Chương " + chapter;

        if (mapPanel != null)
            mapPanel.SetActive(false);

        if (theoryPanel != null)
            theoryPanel.Show(id);
    }

    void CloseTheory()
    {
        if (theoryPanel != null)
        {
            theoryPanel.gameObject.SetActive(false);
        }

        if (mapPanel != null)
        {
            mapPanel.SetActive(true);
        }
    }
}