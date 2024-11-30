using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioRefSO audioRefsSO;
    public static SoundManager Instance {
        get; private set;
    }

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSmth += Player_OnPickSmth;
        BaseCounter.OnAnyPlace += BaseCounter_OnAnyPlace;
        TrashCounter.OnanyObjectTrashed += TrashCounter_OnAnyPlace;
    }

    private void TrashCounter_OnAnyPlace(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        if (trashCounter != null) {
            PlaySound(audioRefsSO.trash, trashCounter.transform.position);
        }
        else {
            Debug.Log("InvalidCastException trashCounter ");
        }
    }

    private void BaseCounter_OnAnyPlace(object sender, System.EventArgs e) {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(audioRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickSmth(object sender, System.EventArgs e) {
        Player player = (Player)sender;
        PlaySound(audioRefsSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(audioRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioRefsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }


    public void PlayFootStepSound(Vector3 position, float volume) {
        PlaySound(audioRefsSO.footstep, position, volume);
    }

}
