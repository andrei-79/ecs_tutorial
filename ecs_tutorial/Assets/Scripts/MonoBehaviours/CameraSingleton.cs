using UnityEngine;

class CameraSingleton : MonoBehaviour {
    // Start is called before the first frame update
    public static Camera Instance;
    void Awake() {
        Instance = GetComponent<Camera>();
    }
}
