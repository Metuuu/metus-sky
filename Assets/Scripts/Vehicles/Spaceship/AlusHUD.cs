using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class AlusHUD : MonoBehaviour {


    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    Vehicle vehicle;
    AlusController alusController;
    Rigidbody rb;

    // UI
    Text speedText;
    Text thrusterText;

    public Slider glideSlider;
    public Toggle holdForwardSpeedToggle;
    public Toggle inverseYToggle;

    // taking off / landing gear
    public Text currentAltitudeText;
    public Text floatHeightText;
    public Text floatAngleText;
    public Text floatDirectionText;
    public Slider floatHeightSlider;
    public Slider floatAngleSlider;
    public Slider floatDirectionSlider;

    bool heightSliderInUse;

    bool initDir;



    // Use this for initialization
    void Start() {
        vehicle = GetComponent<Vehicle>();
        rb = vehicle.rigidbody;
        alusController = GetComponent<AlusController>();
        holdForwardSpeedToggle.interactable = false;
        alusController.rotationLocked = false;
    }

    // Update is called once per frame
    void Update() {

        if (vehicle.engineOn) {

            GetHUDValues();

            UpdateHUD();

        }

        if (vehicle.inUse) {

            if (Input.GetKeyDown(KeyCode.LeftControl)) {

                if (Cursor.lockState != CursorLockMode.None) {
                    ShowHand();
                }
                else {
                    HideHand();
                }
            }

        }
        

        
    }


    #region [ - HUD Hand - ]

    void ShowHand() {
        alusController.rotationLocked = false;
        Cursor.lockState = CursorLockMode.None;
        SetCursorPos(Screen.width / 2, Mathf.RoundToInt(Screen.height / 2.5f + Screen.height / 2));
        Cursor.visible = true;
    }

    public void HideHand() {
        alusController.rotationLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    #endregion



    // Gear changed
    public void GearChanged(AlusController.Gear.HUD hud) {

        hud.HudToEnable.SetActive(true);
        speedText = hud.speedText;
        thrusterText = hud.thrusterText;

        switch (alusController.gear.name) {

            // TakeOff/Landing
            case AlusController.SpaceshipGear.LandingGear:
                floatHeightSlider.minValue = vehicle.gravityBody.distanceToClosestPlanetSurface - 50f;
                floatHeightSlider.maxValue = vehicle.gravityBody.distanceToClosestPlanetSurface + 50f;
                floatHeightSlider.value = (vehicle.gravityBody.distanceToClosestPlanetSurface) * 10f - 1f;
                initDir = true;
                // En ymmärtäny miks en saanu toimimaan tällä systeemillä diriä...
                /*float alusDir = alusController.AngleSigned(alusController.startForwardDirection, transform.forward, transform.up) + 180;
                floatDirectionSlider.minValue = alusDir;
                floatDirectionSlider.maxValue = alusDir + 360;
                floatDirectionSlider.value = alusDir;
                */
                floatAngleSlider.value = 0; //vehicle.transform.rotation.eulerAngles.x;
                break;
            // Null
            default:
                break;
        }


    }



    // Get HUD values
    void GetHUDValues() {

        switch (alusController.gear.name) {

            // TakeOff/Landing
            case AlusController.SpaceshipGear.LandingGear:
                currentAltitudeText.text = "Current altitude:\n" + (vehicle.gravityBody.distanceToClosestPlanetSurface * 10f).ToString("0.0") + "m";
                break;

            // Planetary Drive
            case AlusController.SpaceshipGear.PlanetaryDrive:
                alusController.gear.normalGlideValue = glideSlider.value;
                alusController.invertY = inverseYToggle.isOn;
                break;

            // Space Drive
            case AlusController.SpaceshipGear.SpaceDrive:
                alusController.gear.normalGlideValue = glideSlider.value;
                alusController.invertY = inverseYToggle.isOn;
                break;

            // Hyper Drive
            case AlusController.SpaceshipGear.HyperDrive:
                break;

            // Null
            default:
                break;
        }


    }

    // Set HUD texts
    void UpdateHUD() {

        switch (alusController.gear.name) {

            // TakeOff/Landing
            case AlusController.SpaceshipGear.LandingGear:
                if ((vehicle.gravityBody.distanceToClosestPlanetSurface * 10f) >= 0 && (vehicle.gravityBody.distanceToClosestPlanetSurface * 10f) <= vehicle.gravityBody.closestPlanetRadius * 0.2f - 50) {
                    floatHeightSlider.minValue = Mathf.Clamp((vehicle.gravityBody.distanceToClosestPlanetSurface * 10f) - 50, -10, 1000000);
                    floatHeightSlider.maxValue = (vehicle.gravityBody.distanceToClosestPlanetSurface * 10f) + 50;
                }
                floatHeightText.text = floatHeightSlider.value.ToString("0.0") + "m";
                floatAngleText.text = floatAngleSlider.value + "°";

                float overTheEdge = floatDirectionSlider.minValue;
                float lastValue = floatDirectionSlider.value;
                float alusDir = MathfCustom.AngleSigned(alusController.startForwardDirection, transform.forward, transform.up) + 180;
                floatDirectionSlider.minValue = alusDir;
                floatDirectionSlider.maxValue = alusDir + 360;
                if (overTheEdge - floatDirectionSlider.minValue > 300) {
                    floatDirectionSlider.value = lastValue - 360;
                }
                else if (overTheEdge - floatDirectionSlider.minValue < -300) {
                    floatDirectionSlider.value = lastValue + 360;
                }

                if (initDir) {
                    floatDirectionSlider.value = floatDirectionSlider.minValue + 180;
                    initDir = false;
                }

                break;

            // Planetary Drive
            case AlusController.SpaceshipGear.PlanetaryDrive:
                alusController.gear.normalGlideValue = glideSlider.value;
                alusController.invertY = inverseYToggle.isOn;
                speedText.text = "Speed: " + (rb.velocity.magnitude * 10f).ToString("0") + " m/sec";
                thrusterText.text = "Thruster\n" + alusController.thruster.heat.ToString("0") + "/" + alusController.thruster.maxHeat;
                break;

            // Space Drive
            case AlusController.SpaceshipGear.SpaceDrive:
                alusController.gear.normalGlideValue = glideSlider.value;
                alusController.invertY = inverseYToggle.isOn;
                speedText.text = "speed: " + (rb.velocity.magnitude * 10f / 1000f).ToString("0") + " km/sec";
                thrusterText.text = "Thruster\n" + alusController.thruster.heat.ToString("0") + "/" + alusController.thruster.maxHeat;
                break;

            // Hyper Drive
            case AlusController.SpaceshipGear.HyperDrive:
                break;

            // Null
            default:
                break;
        }


    }
    


}
