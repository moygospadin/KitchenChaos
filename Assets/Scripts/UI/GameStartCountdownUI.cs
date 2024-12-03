using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int prevCountdownNum;
    private const string NUMBER_POPUP = "NumberPopup";
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }


    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountDownToStartActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTime());
        countdownText.text = countdownNumber.ToString();
        if (prevCountdownNum != countdownNumber) {
            prevCountdownNum = countdownNumber;
            animator.SetTrigger("NUMBER_POPUP");
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
