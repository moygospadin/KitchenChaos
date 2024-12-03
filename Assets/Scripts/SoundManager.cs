using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour {


    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    [SerializeField] private AudioRefSO audioRefsSO;
    public static SoundManager Instance {
        get; private set;
    }
    private float volume = 1f;
    private void Awake() {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
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

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }


    public void PlayFootStepSound(Vector3 position, float volume) {
        PlaySound(audioRefsSO.footstep, position, volume);
    }
    public void PlayCountDownSound() {
        PlaySound(audioRefsSO.warning, Vector3.zero, volume);
    }

    public void PlayWarningSound(Vector3 position) {
        PlaySound(audioRefsSO.warning, position, volume);
    }
    public void ChangeVolume() {

        volume += .1f;
        if (volume > 1f) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {

        return volume;
    }
}
