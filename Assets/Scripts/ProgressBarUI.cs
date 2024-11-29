using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;


    private IHasProgress hasProgress;


    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {

            Debug.LogError("Game object" + hasProgressGameObject + "does not implements IHasProgress");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        Debug.Log(e.progressNormalized);
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        }
        else {
            Show();
        }
    }


    private void Show() {
        gameObject.SetActive(true);
    }


    private void Hide() {
        gameObject.SetActive(false);
    }
}
