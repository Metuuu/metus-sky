using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HeadBobber : MonoBehaviour
{

    private float timer = 0.0f;
    [SerializeField]private float bobbingSpeed;
    [SerializeField]private float bobbingAmount;

    private float realBobbingSpeed;
    private float realBobbingAmount;

    [SerializeField]private float midpoint;

    private Movement movementScript;
    [SerializeField]private PlanetLocalDirections PLDir;
    private Vector3 localPos;
    private bool stepped = false;
    private float lastTC = 0;

    // Stepped event
    public delegate void SteppedEventHandler(); //object source, System.EventArgs args
    public event SteppedEventHandler Stepped;

    protected virtual void OnStepped() {
        if (Stepped != null) {
            Stepped(); //this, System.EventArgs.Empty
        }
    }



    // - Start -
    void Start() {
        transform.localPosition = new Vector3(0,midpoint,0);
        movementScript = Character.MovementScript;
    }


    // - Update -
    void Update() {

        if (movementScript.grounded) {

            float waveslice = 0.0f;
            float horizontal = PLDir.forwardVel.magnitude*2;

            

            realBobbingSpeed = bobbingSpeed * PLDir.forwardVel.magnitude*1.2f;
            realBobbingAmount = bobbingAmount * PLDir.forwardVel.magnitude * 1.2f; ;

            localPos = transform.localPosition;

            if (Mathf.Abs(horizontal) == 0) {
                timer = 0.0f;
            }
            else {
                waveslice = Mathf.Sin(timer);
                timer = timer + realBobbingSpeed;
                if (timer > Mathf.PI * 2) {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveslice != 0) {
                float translateChange = waveslice * realBobbingAmount;
                float totalAxes = Mathf.Abs(horizontal);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                localPos.y = midpoint + translateChange;
                
                if (!stepped && translateChange < 0) {
                    stepped = true;
                    OnStepped();
                }
                else if (stepped && translateChange >= 0) {
                    stepped = false;
                }
                lastTC = translateChange;

            }
            else {
                localPos.y = midpoint;
            }

            
            /*if (!stepped && lastYPos > localPos.y) {
                stepped = true;
                Debug.Log("asdfasfd");
            } else if (stepped && lastYPos < localPos.y) {
                stepped = false;
            }*/

            transform.localPosition = localPos;

        }
        else {
            localPos.y = midpoint;
        }

    }





}