using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _cam;

    private void Start() {
        _cam = Camera.main;
    }

    private void LateUpdate() {
        transform.forward = _cam.transform.forward;
    }
}
