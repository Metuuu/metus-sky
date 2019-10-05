using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{


    // Shake init
    Vector3 shakePos;
    int[] shakeList;
    int shake_i;


    // Move init
    Vector3 startPosition;
    Vector3 curPos;
    public float maxForwardDistance;
    public float maxBackwardsDistance;
    public float maxUpwardsDistance;
    public float maxDownwardsDistance;
    public float maxRightDistance;
    public float maxLeftDistance;
    float value;
    float min;
    float max;
    float valueClamped;

    // Lerp
    float lastForceUpDown;
    float lastForceForwardBack;
    float lastForceLeftRight;
    [Range(0.0f, 1.0f)]
    public float upDownLerp;
    [Range(0.0f, 1.0f)]
    public float forwardBackLerp;
    [Range(0.0f, 1.0f)]
    public float leftRightLerp;


    // Start
    void Start() {
        shake_i = 0;
        shakeList = new int[3600];
        shakePos = Vector3.zero;
        InvokeRepeating("Shake", 0.05f, 0.05f);

        startPosition = transform.localPosition;
    }


    // Update
    void Update() {
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime);
    }


    // Add shake
    public void ShakeScreen(int intensity, int milliseconds = 0) {
        for (int j = 0, len = Mathf.RoundToInt(milliseconds / 10f); j < len; ++j) {
            shakeList[shake_i + j] += intensity; // TODO: ettei plussaa vaan järkevässä suhteessa lisää. Logaritmi ?
        }
    }

    // Shake
    void Shake() {

        transform.position = Vector3.Lerp(transform.position, transform.position - shakePos, Time.deltaTime * 0.025f);
        shakePos = Vector3.zero;

        if (shakeList[shake_i] != 0) {
            shakePos = new Vector3(Random.Range(-shakeList[shake_i] / 100f, shakeList[shake_i] / 100f), Random.Range(-shakeList[shake_i] / 100f, shakeList[shake_i] / 100f), Random.Range(-shakeList[shake_i] / 100f, shakeList[shake_i] / 100f));
            transform.position = Vector3.Lerp(transform.position, transform.position + shakePos, Time.deltaTime * 0.010f);
            shakeList[shake_i] = 0;
            ++shake_i;
        }
        else {
            shake_i = 0;
        }
    }




    // Siirrä kameraa voimalla x taaksepäin (- merkki eteenpäin)
    // palauttaa true jos on maksimissa
    public bool moveCameraBackwards(float force) {

        lastForceUpDown = Mathf.Lerp(lastForceForwardBack, force, forwardBackLerp);

        curPos = transform.localPosition;

        value = curPos.z - 1 * force;
        min = startPosition.z + shakePos.z - maxBackwardsDistance;
        max = startPosition.z + shakePos.z + maxForwardDistance;
        valueClamped = Mathf.Clamp(value, min, max);

        transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, curPos.y, valueClamped), Time.deltaTime);

        if (valueClamped == min || valueClamped == max) return true;
        return false;
    }

    // Siirrä kameraa voimalla x ylöspäin (- merkki alaspäin)
    // palauttaa true jos on maksimissa
    public bool moveCameraDownwards(float force) {

        lastForceUpDown = Mathf.Lerp(lastForceUpDown, force, upDownLerp);

        curPos = transform.localPosition;

        value = curPos.y - curPos.y + 1 * force;
        min = startPosition.y + shakePos.y - maxDownwardsDistance;
        max = startPosition.y + shakePos.y + maxUpwardsDistance;
        valueClamped = Mathf.Clamp(value, min, max);

        transform.localPosition = Vector3.Lerp(curPos, new Vector3(curPos.x, valueClamped, curPos.z), Time.deltaTime);

        if (valueClamped == min || valueClamped == max) return true;
        return false;
    }

    // Siirrä kameraa voimalla x ylöspäin (- merkki alaspäin)
    // palauttaa true jos on maksimissa
    public bool moveCameraLeft(float force) {

        lastForceLeftRight = Mathf.Lerp(lastForceLeftRight, force, leftRightLerp);

        curPos = transform.localPosition;

        value = curPos.y - curPos.y + 1 * force;
        min = startPosition.x + shakePos.x - maxLeftDistance;
        max = startPosition.x + shakePos.x + maxRightDistance;
        valueClamped = Mathf.Clamp(value, min, max);

        transform.localPosition = Vector3.Lerp(curPos, new Vector3(valueClamped, curPos.y, curPos.z), Time.deltaTime);

        if (valueClamped == min || valueClamped == max) return true;
        return false;
    }



}
