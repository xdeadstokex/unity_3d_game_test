using UnityEngine;

public class cam_util : MonoBehaviour{
    public Transform target;
    public Vector3 offset = new Vector3(0, 30, -30);

    void LateUpdate(){
        transform.position = target.position + offset;
    }
}