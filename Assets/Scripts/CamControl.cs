using UnityEngine;
using System.Collections;

public class CamControl : MonoBehaviour {

    public float horizontalSpeed = 10.0f;
    public float verticalSpeed = -10.0f;
    private float scrollSpeed = 0.1f;
    private float upSpeed = 0.01f;
    private float limit = 0;
    
    //private Camera myCam;
    private Transform myTrans;
    
    // Use this for initialization
    void Start () {
        //myCam = camera;
        myTrans = transform;
        myTrans.LookAt(Vector3.zero,-Vector3.forward);
    }
    
  void Update () {
    //rotation
    if(Input.GetMouseButton(0) && GUIUtility.hotControl == 0 && myTrans.parent == null && !Input.GetKey("z")){
      float h = horizontalSpeed * Input.GetAxis ("Mouse X");
      float v = verticalSpeed * Input.GetAxis ("Mouse Y");
      myTrans.RotateAround (Vector3.zero, myTrans.up, h);
      myTrans.RotateAround (Vector3.zero, myTrans.right, v);
    }
    //scroll
    if(Input.GetAxis("Mouse ScrollWheel") != 0 && !Input.GetKey(KeyCode.LeftControl) && myTrans.parent == null){
      float p = scrollSpeed * Input.GetAxis("Mouse ScrollWheel");
      if(limit != 0){
        if(p<0 || myTrans.position.magnitude > limit){
          myTrans.Translate(Vector3.forward * p);
        }
      }
      else{
        myTrans.Translate(Vector3.forward * p);
      }
    }
    if(Input.GetAxis("Vertical") != 0){
      float p = upSpeed * Input.GetAxis("Vertical");
      myTrans.Translate(Vector3.forward * p);
    }
  }
}