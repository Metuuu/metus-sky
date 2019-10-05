using System.Collections;
using UnityEngine;

public static class AtmosphereGenerator {

    public static AtmosphereData GenerateAtmosphere(Transform planet, float innerRadius, float scaleDepth) {
        return new AtmosphereData(planet, innerRadius, scaleDepth);
    }


}


[System.Serializable]
public class AtmosphereData {
    
    #region [ - Init - ]

    Transform planet;

    public Transform atmoFromGroundTransform;
    public Transform atmoFromSpaceTransform;

    //Atmospheric Scattering Variables
    public Transform m_sun;
    public Material m_groundMaterial;
    public Material m_skyGroundMaterial;
    public Material m_skySpaceMaterial;
    public Material m_groundfromgroundMaterial;

	public Vector4 m_skyGroundMaterialCShift = new Vector4(9f, 9f, 9f, 3f);
	public Vector4 m_skySpaceMaterialCShift = new Vector4(3 * 1.8f, 3 * 1.8f, 3 * 1.8f, 3 * 1f);
	//public Vector4 m_skyGroundMaterialCShift = new Vector4(3f, 3f, 3f, 1f);
	//public Vector4 m_skySpaceMaterialCShift = new Vector4(1.8f, 1.8f, 1.8f, 1f);

	public float m_hdrExposure = 0.8f;
    public Vector3 m_waveLength = new Vector3(0.65f, 0.57f, 0.475f); // Wave length of sun light
    Vector3 invWaveLength4;
    public float m_ESun = 20.0f; 	// Sun brightness constant
    public float m_kr = 0.0025f; 	// Rayleigh scattering constant
    public float m_km = 0;//0.0010f; 	// Mie scattering constant
    public float m_g = -0.990f;     // The Mie phase asymmetry factor, must be between 0.999 to -0.999

    const float m_outerScaleFactor = 1.025f; // Difference between inner and ounter radius. Must be 2.5%
    public float m_innerRadius;	// Radius of the ground sphere
    float m_outerRadius;	// Radius of the sky sphere    m_innerRadius * m_outerScaleFactor
    public float m_scaleDepth;     // The scale depth (i.e. the altitude at which the atmosphere's average density is found)   0.25f
    float scale;
    #endregion


    // - ATMOSPHERE DATA -
    public AtmosphereData(Transform planet, float innerRadius, float scaleDepth) {

        this.planet = planet;
        m_innerRadius = innerRadius;
        m_outerRadius = m_innerRadius * m_outerScaleFactor * 1.15f;
        m_scaleDepth = scaleDepth;

        CreateAtmosphere();

    }


    // Create Atmosphere
    void CreateAtmosphere() {

        m_sun = GameObject.Find("Sun").transform;
        Shader groundfromSpace = Shader.Find("Atmosphere/GroundFromSpace");
        Shader skyFromSpace = Shader.Find("Atmosphere/SkyFromSpace");
        Shader skyFromGround = Shader.Find("Atmosphere/SkyFromAtmosphere");

        m_skyGroundMaterial = new Material(Shader.Find("Atmosphere/SkyFromAtmosphere"));
        m_skySpaceMaterial = new Material(Shader.Find("Atmosphere/SkyFromSpace"));
        //m_groundMaterial = new Material(Shader.Find("Atmosphere/GroundFromSpace"));
        //m_groundfromgroundMaterial = new Material(Shader.Find("DiffusePlanet"));

        InitializeMaterial(m_skyGroundMaterial, m_skyGroundMaterialCShift);
        InitializeMaterial(m_skySpaceMaterial, m_skySpaceMaterialCShift);
        //InitializeMaterial(m_groundfromgroundMaterial, Vector3.one);
        //InitializeMaterial(m_groundMaterial, Vector3.one);

        // - Atmosphere from ground -
        Transform atmoFromGround = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Atmosphere")).transform;
        atmoFromGround.name = "AtmosphereFromGround";
        atmoFromGround.position = planet.position;
        atmoFromGround.localScale = new Vector3(m_outerRadius, m_outerRadius, m_outerRadius);

        atmoFromGroundTransform = atmoFromGround.transform.Find("SphereObject");
        atmoFromGroundTransform.GetComponent<Renderer>().material = m_skyGroundMaterial;
        atmoFromGround.transform.parent = planet;

        // - Atmosphere from space -
        Transform atmoFromSpace = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Atmosphere")).transform;
        atmoFromSpace.name = "AtmosphereFromSpace";
        atmoFromSpace.position = planet.position;
        atmoFromSpace.localScale = new Vector3(m_outerRadius, m_outerRadius, m_outerRadius);

        atmoFromSpaceTransform = atmoFromSpace.transform.Find("SphereObject");
        atmoFromSpaceTransform.GetComponent<Renderer>().material = m_skySpaceMaterial;
        atmoFromSpace.transform.parent = planet;

    }


    // Initialize Material (Shader)
    void InitializeMaterial(Material mat, Vector4 cshift) {

        invWaveLength4 = new Vector3(1.0f / Mathf.Pow(m_waveLength.x, 4.0f), 1.0f / Mathf.Pow(m_waveLength.y, 4.0f), 1.0f / Mathf.Pow(m_waveLength.z, 4.0f));
        scale = 1.0f / (m_outerRadius - m_innerRadius);

        mat.SetVector("_ColorShift", cshift);
        mat.SetVector("_PlanetPos", planet.position);
        mat.SetVector("v3LightPos", m_sun.forward * -1.0f);
        mat.SetVector("v3InvWavelength", invWaveLength4);
        mat.SetFloat("fOuterRadius", m_outerRadius);
        mat.SetFloat("fOuterRadius2", m_outerRadius * m_outerRadius);
        mat.SetFloat("fInnerRadius", m_innerRadius);
        mat.SetFloat("fInnerRadius2", m_innerRadius * m_innerRadius);
        mat.SetFloat("fKrESun", m_kr * m_ESun);
        mat.SetFloat("fKmESun", m_km * m_ESun);
        mat.SetFloat("fKr4PI", m_kr * 4.0f * Mathf.PI);
        mat.SetFloat("fKm4PI", m_km * 4.0f * Mathf.PI);
        mat.SetFloat("fScale", scale);
        mat.SetFloat("fScaleDepth", m_scaleDepth);
        mat.SetFloat("fScaleOverScaleDepth", scale / m_scaleDepth);
        mat.SetFloat("fHdrExposure", m_hdrExposure);
        mat.SetFloat("g", m_g);
        mat.SetFloat("g2", m_g * m_g);

    }




    // Update
    public void Update() {

        invWaveLength4 = new Vector3(1.0f / Mathf.Pow(m_waveLength.x, 4.0f), 1.0f / Mathf.Pow(m_waveLength.y, 4.0f), 1.0f / Mathf.Pow(m_waveLength.z, 4.0f));
        scale = 1.0f / (m_outerRadius - m_innerRadius);

        UpdateMaterial(m_skyGroundMaterial, m_skyGroundMaterialCShift);
        UpdateMaterial(m_skySpaceMaterial, m_skySpaceMaterialCShift);
        //InitializeMaterial(m_groundfromgroundMaterial, Vector3.one);
        //InitializeMaterial(m_groundMaterial, Vector3.one);
    }


    // Update Material
    void UpdateMaterial(Material mat, Vector4 cshift) {
        
        mat.SetVector("_ColorShift", cshift);
        mat.SetVector("_PlanetPos", planet.position);
        mat.SetVector("v3LightPos", m_sun.forward * -1.0f);
        mat.SetVector("v3InvWavelength", invWaveLength4);
        mat.SetFloat("fOuterRadius", m_outerRadius);
        mat.SetFloat("fOuterRadius2", m_outerRadius * m_outerRadius);
        mat.SetFloat("fInnerRadius", m_innerRadius);
        mat.SetFloat("fInnerRadius2", m_innerRadius * m_innerRadius);
        mat.SetFloat("fKrESun", m_kr * m_ESun);
        mat.SetFloat("fKmESun", m_km * m_ESun);
        mat.SetFloat("fKr4PI", m_kr * 4.0f * Mathf.PI);
        mat.SetFloat("fKm4PI", m_km * 4.0f * Mathf.PI);
        mat.SetFloat("fScale", scale);
        mat.SetFloat("fScaleDepth", m_scaleDepth);
        mat.SetFloat("fScaleOverScaleDepth", scale / m_scaleDepth);
        mat.SetFloat("fHdrExposure", m_hdrExposure);
        mat.SetFloat("g", m_g);
        mat.SetFloat("g2", m_g * m_g);

    }




}
