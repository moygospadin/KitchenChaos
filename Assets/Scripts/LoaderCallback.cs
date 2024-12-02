using UnityEngine;

public class LoaderScript : MonoBehaviour {


    private bool isFirstUpdate = true;

    void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
