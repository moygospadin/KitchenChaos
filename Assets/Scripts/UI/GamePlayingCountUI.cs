using UnityEngine;
using UnityEngine.UI;

public class GamePlayingCountUI : MonoBehaviour {
    [SerializeField] private Image timerImage;

    private void Update() {
        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
