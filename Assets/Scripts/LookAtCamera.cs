using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        cameraForward,
         cameraForwardInverted
    }
    [SerializeField] private Mode mode;

    // Update is called once per frame
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.cameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.cameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
