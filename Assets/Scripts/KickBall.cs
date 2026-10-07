using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KickBall : MonoBehaviour
{
    // =========================================================
    // BALL
    // =========================================================

    [Header("Bóng")]
    [SerializeField] private RectTransform ball;

    // Các điểm trong khung thành
    [SerializeField] private RectTransform[] goalTargets;

    // 4 nút A B C D
    [SerializeField] private Button[] answerButtons;


    // =========================================================
    // GOALKEEPER
    // =========================================================

    [Header("Thủ môn")]
    [SerializeField] private RectTransform goalkeeper;

    [SerializeField] private Animator goalkeeperAnimator;


    // =========================================================
    // TEXT
    // =========================================================

    [Header("Kết quả")]
    [SerializeField] private GameObject goalText;

    [SerializeField] private GameObject missText;


    // =========================================================
    // BALL SETTINGS
    // =========================================================

    [Header("Cài đặt bóng")]
    [SerializeField] private float flyTime = 0.85f;

    [SerializeField] private float arcHeight = 120f;

    [SerializeField] private float endScale = 0.5f;


    // =========================================================
    // MISS SETTINGS
    // =========================================================

    [Header("Khi sút trượt")]
    [SerializeField]
    private Vector2 missOffset =
        new Vector2(450f, 150f);


    // =========================================================
    // RUNTIME
    // =========================================================

    private Vector2 startPos;

    private Vector3 startScale;

    private bool busy;


    public bool IsBusy => busy;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (ball != null)
        {
            startPos =
                ball.anchoredPosition;

            startScale =
                ball.localScale;
        }

        // Tắt text lúc đầu
        if (goalText != null)
        {
            goalText.SetActive(false);
        }

        if (missText != null)
        {
            missText.SetActive(false);
        }
    }


    // =========================================================
    // CORRECT ANSWER
    // =========================================================

    // Trả lời đúng
    // Bóng bay vào gôn
    public void Goal()
    {
        if (!busy)
        {
            StartCoroutine(
                KickRoutine(true)
            );
        }
    }


    // =========================================================
    // WRONG ANSWER
    // =========================================================

    // Trả lời sai
    // Bóng bay trượt
    public void Miss()
    {
        if (!busy)
        {
            StartCoroutine(
                KickRoutine(false)
            );
        }
    }


    // =========================================================
    // KICK
    // =========================================================

    private IEnumerator KickRoutine(
        bool scored
    )
    {
        busy = true;

        SetButtons(false);


        // =====================================================
        // CHỌN VỊ TRÍ BÓNG BAY
        // =====================================================

        int targetIndex =
            Random.Range(
                0,
                goalTargets.Length
            );


        Vector2 end =
            goalTargets[targetIndex]
                .anchoredPosition;


        // =====================================================
        // NẾU SAI -> BÓNG BAY RA NGOÀI
        // =====================================================

        if (!scored)
        {
            float side =
                Random.value < 0.5f
                    ? -1f
                    : 1f;


            end +=
                new Vector2(
                    side * missOffset.x,
                    missOffset.y
                );
        }


        // =====================================================
        // THỦ MÔN ĐỔ NGƯỜI
        // =====================================================

        TriggerGoalkeeper(
            end,
            scored
        );


        // Thủ môn phản ứng trước bóng một chút
        yield return
            new WaitForSeconds(0.08f);


        // =====================================================
        // HƯỚNG XOAY BÓNG
        // =====================================================

        float dir =
            end.x >= startPos.x
                ? 1f
                : -1f;


        // =====================================================
        // BÓNG BAY
        // =====================================================

        for (
            float t = 0;
            t < 1f;
            t += Time.deltaTime / flyTime
        )
        {
            // Ease Out
            float e =
                1f
                - (1f - t)
                * (1f - t);


            Vector2 pos =
                Vector2.Lerp(
                    startPos,
                    end,
                    e
                );


            // Tạo đường cong
            pos.y +=
                Mathf.Sin(
                    t * Mathf.PI
                )
                * arcHeight;


            ball.anchoredPosition =
                pos;


            // Bóng nhỏ dần khi bay xa
            ball.localScale =
                Vector3.Lerp(
                    startScale,
                    startScale * endScale,
                    e
                );


            // Bóng xoay
            ball.localRotation =
                Quaternion.Euler(
                    0,
                    0,
                    -dir * 720f * t
                );


            yield return null;
        }


        // =====================================================
        // ĐẶT CHÍNH XÁC Ở ĐIỂM CUỐI
        // =====================================================

        ball.anchoredPosition =
            end;


        // =====================================================
        // KẾT QUẢ
        // =====================================================

        if (scored)
        {
            if (goalText != null)
            {
                goalText.SetActive(true);
            }
        }
        else
        {
            if (missText != null)
            {
                missText.SetActive(true);
            }
        }


        yield return
            new WaitForSeconds(1f);


        // =====================================================
        // RESET
        // =====================================================

        if (goalText != null)
        {
            goalText.SetActive(false);
        }


        if (missText != null)
        {
            missText.SetActive(false);
        }


        ball.anchoredPosition =
            startPos;


        ball.localScale =
            startScale;


        ball.localRotation =
            Quaternion.identity;


        SetButtons(true);


        busy = false;
    }


    // =========================================================
    // GOALKEEPER ANIMATION
    // =========================================================

    private void TriggerGoalkeeper(
        Vector2 ballTarget,
        bool scored
    )
    {
        if (
            goalkeeper == null
            ||
            goalkeeperAnimator == null
        )
        {
            return;
        }


        // =====================================================
        // XÁC ĐỊNH BÓNG BAY TRÁI / PHẢI
        // =====================================================

        bool ballGoesRight =
            ballTarget.x
            >= goalkeeper.anchoredPosition.x;


        bool keeperDiveRight;


        // =====================================================
        // ĐÚNG -> THỦ MÔN ĐỔ NGƯỢC HƯỚNG
        // để bóng vào gôn
        // =====================================================

        if (scored)
        {
            keeperDiveRight =
                !ballGoesRight;
        }

        // =====================================================
        // SAI -> THỦ MÔN ĐỔ THEO HƯỚNG BÓNG
        // =====================================================

        else
        {
            keeperDiveRight =
                ballGoesRight;
        }


        // =====================================================
        // TRIGGER ANIMATION
        // =====================================================

        if (keeperDiveRight)
        {
            goalkeeperAnimator
                .ResetTrigger(
                    "DiveLeft"
                );


            goalkeeperAnimator
                .SetTrigger(
                    "DiveRight"
                );
        }
        else
        {
            goalkeeperAnimator
                .ResetTrigger(
                    "DiveRight"
                );


            goalkeeperAnimator
                .SetTrigger(
                    "DiveLeft"
                );
        }
    }


    // =========================================================
    // BUTTONS
    // =========================================================

    private void SetButtons(
        bool on
    )
    {
        foreach (
            Button b
            in answerButtons
        )
        {
            if (b != null)
            {
                b.interactable =
                    on;
            }
        }
    }
}