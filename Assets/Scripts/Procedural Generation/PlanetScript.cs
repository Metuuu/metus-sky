using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlanetScript : MonoBehaviour {


    // -- INIT --
    Transform myTransform;
    public Transform player;
    public GPUPlanetData planetData;

    public Material mat;
    //LOD_settings.LevelOfDetails lod;
    //public float radius;

    List<LodObject> lodObjects = new List<LodObject>();
    LodObject[] allChildObjs;
    string[] allChildNames;
    Vector3[] verticesMiddlePos;

    // .SetColor("_EmissionColor", rend.material.color * Mathf.LinearToGammaSpace(0.02f)); <- etäisyyden mukaan ehkä emissiota

    Vector3[] sidePositions;
    int fullCount;
    int sideCount;
    int quarterCount;
    int lodsCount;

    // Distance check timer
    private static float CHECK_DISTANCE_EVERY = 0.1f; // sec
    private float nextDistanceCheckTime = 0.0f;

    // Muuttujat jotka jätetään muistiin suosiolla ettei garbage collecor kuluta niihin turhaan aikaan
    LodObject lodObj;
    string childName;
    int childArrayIndex;
    Vector3 localPositionM;
    float removeDistanceClose;
    float removeDistanceFar;
	Vector3 pointA;
    float distanceToPlayer;
    float distanceToClosePoint;
    float distanceToFarPoint;

	int asdf = 0;


    class LodObject {

        private string name;
        private GameObject gameObj;
        private List<RenderTexture> renderTextures;
        private float removeDistanceClose;
        private float removeDistanceFar;
        private Vector3 localPosition;
		private QualitySettings.LODSide lodSide;

		public QualitySettings.LODSide LODSide { get { return lodSide; } }
        public Vector3 LocalPosition { get { return localPosition; } }
        public float RemoveDistanceClose { get { return removeDistanceClose; } }
        public float RemoveDistanceFar { get { return removeDistanceFar; } }
        public string Name { get { return name; } }
        public GameObject GameObj { get { return gameObj; } }


        public LodObject(string name, GameObject gameObj, Vector3 localPosition, float removeDistanceClose, float removeDistanceFar, QualitySettings.LODSide lodSide, List<RenderTexture> renderTextures) {
            this.name = name;
            this.gameObj = gameObj;
            this.localPosition = localPosition;
            this.removeDistanceClose = removeDistanceClose;
            this.removeDistanceFar = removeDistanceFar;
            this.renderTextures = renderTextures;
            this.lodSide = lodSide;
        }

        public void unallocateRenderTextures() {
            for (int i = renderTextures.Count-1; i >= 0; --i) {
                renderTextures[i].Release();
            }
        }
        
    }


    // -- FUNCTIONS --

    // - Awake -
    void Start() {

        myTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Tekee arrayt kaikelle että olis nopee hakea niitä sieltä getArrayPosition():in avulla joka palauttaa nimen perusteella palasen array paikan
        fullCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Full);
        sideCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Side);
        quarterCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Quarter);
        allChildObjs = new LodObject[fullCount + sideCount + quarterCount];
        allChildNames = new string[fullCount + sideCount + quarterCount];
        verticesMiddlePos = new Vector3[fullCount + sideCount + quarterCount];

		lodsCount = QualitySettings.CurrentLOD.LodsCount;

        sidePositions = new Vector3[] { new Vector3(0, -planetData.planetSize / 2, 0), new Vector2(-planetData.planetSize / 2, 0), new Vector3(0,0, -planetData.planetSize / 2), new Vector2(planetData.planetSize / 2, 0), new Vector2(0, planetData.planetSize / 2), new Vector3(0, 0, planetData.planetSize / 2) };

    }
    
    
    // - Update -
    void Update() {

		#region [ - Atmosphere - ]
		if (planetData.hasAtmosphere) {
			planetData.atmosphere.Update();
			
			if (Vector3.Distance(transform.position, player.position) > planetData.radius * 1.17f) {
			//if (Vector3.Distance(transform.position, player.position) > planetData.radius * 1.17f) {
				planetData.atmosphere.atmoFromSpaceTransform.gameObject.SetActive(true);
				planetData.atmosphere.atmoFromGroundTransform.gameObject.SetActive(false);
			} else {
				planetData.atmosphere.atmoFromSpaceTransform.gameObject.SetActive(false);
				planetData.atmosphere.atmoFromGroundTransform.gameObject.SetActive(true);
			}

		}
		#endregion


		#region [ - LOD - ]

		if (nextDistanceCheckTime > 0) {
			nextDistanceCheckTime -= Time.deltaTime;
			return;
		}

		// Deaktivoi viimekerralla aktivoituneina olleet childit
		if (nextDistanceCheckTime <= 0) {
			nextDistanceCheckTime += CHECK_DISTANCE_EVERY;

			for (int i = lodObjects.Count - 1; i >= 0; --i) {
				lodObjects[i].GameObj.SetActive(false);

				pointA = myTransform.position + lodObjects[i].LocalPosition;

				//if (lodObjects[i].Name.Length > 1) {
				//	Debug.DrawLine(pointA, pointA + (player.transform.position - pointA).normalized * planetData.planetSize * lodObjects[i].RemoveDistanceFar, Color.red, 0.1f); // TODO: säädä näitä valueita
				//}
				//Debug.DrawLine(pointA, pointA + (player.transform.position - pointA).normalized * (planetData.planetSize) * lodObjects[i].RemoveDistanceClose, Color.green, 0.1f);


				distanceToPlayer = Vector3.Distance(pointA, player.transform.position);
                distanceToClosePoint = Vector3.Distance(pointA, pointA + (player.transform.position - pointA).normalized * (planetData.planetSize) * lodObjects[i].RemoveDistanceClose);
                distanceToFarPoint = Vector3.Distance(pointA, pointA + (player.transform.position - pointA).normalized * planetData.planetSize * lodObjects[i].RemoveDistanceFar);

                if (distanceToClosePoint >= distanceToPlayer || distanceToFarPoint < distanceToPlayer) {
                    lodObjects[i].unallocateRenderTextures();
                    Destroy(lodObjects[i].GameObj);
                    lodObjects.RemoveAt(i);
                }
            }
		}


		// Hillitön LOD koodi
        for (int i = 0; i < lodsCount; ++i) {

            if (QualitySettings.CurrentLOD.lodSide[i] == QualitySettings.LODSide.Quarter) { return; } // Jos on quarter niin lopettaa tähän koska !! kattoo quarterit sivujen ja quarterejen sisällä rekursiivisesti !!

            bool nextLod = (i != lodsCount); // onko seuraavan tason lod tarkkuutta olemassa


            switch (QualitySettings.CurrentLOD.lodSide[i]) {

                // - Planeetta kokonaisena -
                case QualitySettings.LODSide.Full:

                    localPositionM = Vector3.zero;

                    childName = "F" + QualitySettings.CurrentLOD.lod[i].ToString();
                    childArrayIndex = getArrayPosition(childName);
                    lodObj = allChildObjs[childArrayIndex];
                    removeDistanceFar = 1 + QualitySettings.CurrentLOD.distance[(i)]*10;
                    // Jos on seuraavasta lodista kauempana niin aktivoi ittensä
                    if (!nextLod || Vector3.Distance(player.position, myTransform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize) {
                        if (lodObj == null) { // Jos objektia ei ole niin generoi sen
							List<RenderTexture> renderTextures = new List<RenderTexture>();
							lodObj = new LodObject(childName, planetData.GeneratePlanetChunk(QualitySettings.CurrentLOD.gridSize[i], myTransform.position, ref renderTextures, QualitySettings.CurrentLOD.hasHeight[i], QualitySettings.CurrentLOD.hasCollider[i], childName), localPositionM, 0, removeDistanceFar, QualitySettings.LODSide.Full, renderTextures);
                            lodObj.GameObj.transform.parent = myTransform;
                            allChildObjs[childArrayIndex] = lodObj;
                            lodObjects.Add(lodObj);
                        } else { // Jos objekti on jo olemassa
                            lodObj.GameObj.SetActive(true);
                        }
                        return;
                    }
                    break;
                // - Joku kuudesta sivusta -
                case QualitySettings.LODSide.Side:

                    // Looppaa jokaisen sivun läpi
                    for (int side = 0; side < 6; ++side) {

                        childName = side.ToString();
                        childArrayIndex = getArrayPosition(childName);
                        lodObj = allChildObjs[childArrayIndex];

                        localPositionM = sidePositions[side];
                        if (QualitySettings.CurrentLOD.LodsCount > (i + 1)) {
                            removeDistanceClose = (QualitySettings.CurrentLOD.distance[(i)] - QualitySettings.CurrentLOD.distance[(i + 1)]);
                        } else {
                            removeDistanceClose = 0;
                        }
                        removeDistanceFar = (QualitySettings.CurrentLOD.distance[(i)] + QualitySettings.CurrentLOD.distance[(i - 1)]);

                        // Jos on seuraavasta lodista kauempana niin aktivoi ittensä
                        if (!nextLod || Vector3.Distance(player.position, myTransform.position + sidePositions[side]) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize) {

                            if (lodObj == null || lodObj.GameObj == null) { // Jos objektia ei ole niin generoi sen
								List<RenderTexture> renderTextures = new List<RenderTexture>();
								lodObj = new LodObject(childName, planetData.GeneratePlanetChunk(QualitySettings.CurrentLOD.gridSize[i], myTransform.position, ref renderTextures, QualitySettings.CurrentLOD.hasHeight[i], QualitySettings.CurrentLOD.hasCollider[i], childName), localPositionM, removeDistanceClose, removeDistanceFar, QualitySettings.LODSide.Side, renderTextures);
                                lodObj.GameObj.transform.parent = myTransform;
                                allChildObjs[childArrayIndex] = lodObj;
                                lodObjects.Add(lodObj);
                            } else { // Jos objekti on jo olemassa
                                lodObj.GameObj.SetActive(true);
                            }
                        } else { // Jos on tarkemman lodin sisäpuolella niin quartereitten tarkistus
                            LODQuarters(childName, i + 1);
                        }

                    }
                    return;
            }


        }
        #endregion


    }


	// - Quit -
	void OnApplicationQuit() {
		// Clean RenderTextures
		foreach (LodObject lodObject in lodObjects) {
			lodObject.unallocateRenderTextures();
		}
	}


	IEnumerator SaveMesh(MeshFilter mf) {
        yield return new WaitForSeconds(1);
        var savePath = "Assets/" + ++asdf + ".asset";
        Debug.Log("Saved Mesh to:" + savePath);
        AssetDatabase.CreateAsset(mf.mesh, savePath);
    }


    // - Sivujen/neljäsosien neljäsosat rekursiivisesti -
    void LODQuarters(string side, int lodIndex) {
        
        int i = lodIndex;
        bool nextLod = (i != lodsCount); // onko seuraavan tason lod tarkkuutta olemassa
            
        // Looppaa jokaisen neljäsosan läpi
        for (int quarter = 0; quarter < 4; ++quarter) {
            childName = side + quarter.ToString();
            childArrayIndex = getArrayPosition(childName);
            lodObj = allChildObjs[childArrayIndex];

			localPositionM = SphericalCubeGenerator.getMiddleVerticePosition(childName) * (planetData.planetSize);

			if (QualitySettings.CurrentLOD.LodsCount > (i + 1)) {
                removeDistanceClose = (QualitySettings.CurrentLOD.distance[(i + 1)]); // TODO: säädä näitä valueita
            } else {
                removeDistanceClose = 0;
            }
            removeDistanceFar = (QualitySettings.CurrentLOD.distance[(i)] + QualitySettings.CurrentLOD.distance[(i-1)]*5f); // TODO: säädä näitä valueita

			//Debug.DrawLine(myTransform.position + SphericalCubeGenerator.getLTopVerticePosition(childName) * (planetData.planetSize), player.transform.position, Color.red, 1f);

			if (!nextLod
				|| (
					Vector3.Distance(myTransform.position + localPositionM, player.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize // Middle
					&& Vector3.Distance(myTransform.position + SphericalCubeGenerator.getLBottomVerticePosition(childName) * (planetData.planetSize), player.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize // Left Bottom
					&& Vector3.Distance(myTransform.position + SphericalCubeGenerator.getRBottomVerticePosition(childName) * (planetData.planetSize), player.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize // Right Bottom
					&& Vector3.Distance(myTransform.position + SphericalCubeGenerator.getLTopVerticePosition(childName) * (planetData.planetSize), player.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize // Left Top
					&& Vector3.Distance(myTransform.position + SphericalCubeGenerator.getRTopVerticePosition(childName) * (planetData.planetSize), player.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize // Right Top
				)) { // Jos on seuraavasta lodista kauempana niin aktivoi ittensä

                if (lodObj == null || lodObj.GameObj == null) { // Jos objektia ei ole niin generoi sen
					List<RenderTexture> renderTextures = new List<RenderTexture>();
					lodObj = new LodObject(childName, planetData.GeneratePlanetChunk(QualitySettings.CurrentLOD.gridSize[i], myTransform.position, ref renderTextures, QualitySettings.CurrentLOD.hasHeight[i], QualitySettings.CurrentLOD.hasCollider[i], childName), localPositionM, removeDistanceClose, removeDistanceFar, QualitySettings.LODSide.Quarter, renderTextures);
                    lodObj.GameObj.transform.parent = myTransform;
                    allChildObjs[childArrayIndex] = lodObj;
                    lodObjects.Add(lodObj);

                    //StartCoroutine(SaveMesh(lodObj.GameObj.GetComponent<MeshFilter>()));
                } else {
                    lodObj.GameObj.SetActive(true);
                }
            }
            else { // Jos on tarkemman lodin jatkaa rekursiota
                LODQuarters(childName, i + 1);
            }

        }


    }
    
    // Antaa array positionin nimestä
    int getArrayPosition(string name) {
        int pos = 0, len = childName.Length;

        // Full
        if (childName[0] == 'F') {
            return (int)char.GetNumericValue(name[1]);
        }
        pos += fullCount;
        
        // Side
        if (len == 1) {
            return pos + (int)char.GetNumericValue(name[0]);
        }
        pos += sideCount-1;

        
        switch (name[0]) {
            case '0': // pohja
                break;
            case '1': // vas
                pos += quarterCount / 6;
                break;
            case '2': // keski
                pos += quarterCount / 6 * 2;
                break;
            case '3': // oik
                pos += quarterCount / 6 * 3;
                break;
            case '4': // ylä
                pos += quarterCount / 6 * 4;
                break;
            case '5': // taka
                pos += quarterCount / 6 * 5;
                break;
        }
        // Quarters
        for (int i = 1; i < len; ++i) {
            switch (name[i]) {
                case '0': // vasenala
                    pos += (int)Mathf.Pow(4, i-1);
                    break;
                case '1': // oikeeala
                    pos += 2 * (int)Mathf.Pow(4, i-1);
                    break;
                case '2': // vasenylä
                    pos += 3 * (int)Mathf.Pow(4, i-1);
                    break;
                case '3': // oikeeylä
                    pos += 4 * (int)Mathf.Pow(4, i-1);
                    break;
            }
        }
        return pos;
    }



    // Antaa keskimmäisen verticen positionin
    Vector3 getMiddleVerticePos(string name) {
        
        int pos = getArrayPosition(name);
        
        if (verticesMiddlePos[pos] == Vector3.zero) {
            Vector3[] vertices = lodObj.GameObj.GetComponent<MeshFilter>().mesh.vertices;
            verticesMiddlePos[pos] = lodObj.GameObj.transform.localRotation * (myTransform.localToWorldMatrix.MultiplyPoint3x4(vertices[(vertices.Length / 2)]) - myTransform.position) * planetData.planetSize; // todo alkuun myTransform.rotation * .. että tukee koko objectin rotaatiota myös
            return verticesMiddlePos[pos];
        }
        
        return verticesMiddlePos[pos];
    }
    

    private void RemoveTextures(Renderer renderer) {
        Object[] textures = EditorUtility.CollectDependencies(new UnityEngine.Object[] { renderer });
        for (int i = textures.Length-1; i >= 0; --i) {
            Destroy(textures[i]);
        }
    }


}



















/*

    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour {


    // -- INIT --
    Transform myTransform;
    public Transform player;
    public GPUPlanetData planetData;

    public Material mat;
    //LOD_settings.LevelOfDetails lod;
    public float radius;

    List<GameObject> lodObjects = new List<GameObject>();
    GameObject[] allChildObjs;
    string[] allChildNames;
    Vector3[] verticesMiddlePos;

    // .SetColor("_EmissionColor", rend.material.color * Mathf.LinearToGammaSpace(0.02f)); <- etäisyyden mukaan ehkä emissiota

    int fullCount;
    int sideCount;
    int quarterCount;

    // Muuttujat jotka jätetään muistiin suosiolla ettei garbage collecor kuluta niihin turhaan aikaan
    GameObject lodObj;
    string childName;
    int childArrayIndex;



    // -- FUNCTIONS --

    // - Awake -
    void Awake() {

        myTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Tekee arrayt kaikelle että olis nopee hakea niitä sieltä getArrayPosition():in avulla joka palauttaa nimen perusteella palasen array paikan
        fullCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Full);
        sideCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Side);
        quarterCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Quarter);
        allChildObjs = new GameObject[fullCount + sideCount + quarterCount];
        allChildNames = new string[fullCount + sideCount + quarterCount];
        verticesMiddlePos = new Vector3[fullCount + sideCount + quarterCount];
        
    }


    // - Update -
    void Update() {

        #region [ - Atmosphere - ]
        if (planetData.hasAtmosphere) {
            planetData.atmosphere.Update();
        }

        #endregion


        #region [ - LOD - ]

        // Deaktivoi viimekerralla aktivoituneina olleet childit
        for (int i = lodObjects.Count-1; i > 0; --i) {
            lodObjects[i].SetActive(false);
            lodObjects.RemoveAt(i);
        }

        // Hillitön LOD koodi
        bool breakkaa = false; // breakkaa variable hoitelee, ettei looppaa turhaan pitemmälle mitä pitäis

        for (int i = 0, len = QualitySettings.CurrentLOD.LodsCount; i < len; ++i) {

            if (QualitySettings.CurrentLOD.lodSide[i] == QualitySettings.LODSide.Quarter) { return; } // Jos on quarter niin lopettaa tähän koska !! kattoo quarterit sivujen ja quarterejen sisällä rekursiivisesti !!

            bool nextLod = (i != len); // onko seuraavan tason lod tarkkuutta olemassa


            switch (QualitySettings.CurrentLOD.lodSide[i]) {

                // - Planeetta kokonaisena -
                case QualitySettings.LODSide.Full:
                    
                    childName = "F" + QualitySettings.CurrentLOD.lod[i].ToString();
                    childArrayIndex = getArrayPosition(childName);
                    lodObj = allChildObjs[childArrayIndex];

                    if (lodObj == null) { // Jos objektia ei ole niin generoi sen
                        lodObj = planetData.GeneratePlanetChunk(childName, QualitySettings.CurrentLOD.gridSize[i], myTransform.position, childName);
                        //lodObj = CreateLodObject(childName, "", LOD_settings.CurrentLOD.gridSize[i], myTransform.position);
                        lodObj.transform.parent = myTransform;
                        allChildObjs[childArrayIndex] = lodObj;
                        lodObjects.Add(lodObj);


                    } else { // Jos objekti on jo olemassa


                        if (nextLod) { // Jos voi olla tarkempi lod
                            if (Vector3.Distance(player.position, lodObj.transform.position) >= QualitySettings.CurrentLOD.distance[i] * planetData.planetSize) { // Jos on seuraavasta lodista kauempana niin aktivoi ittensä
                                lodObj.SetActive(true);
                                lodObjects.Add(lodObj);
                                breakkaa = true;
                            } else { // Jos on tarkemman lodin sisäpuolella niin disablettaa ittensä
                                lodObj.SetActive(false);
                            }
                        }
                    }
                    break;
                // - Joku kuudesta sivusta -
                case QualitySettings.LODSide.Side:

                    // Looppaa jokaisen sivun läpi
                    for (int side = 0; side < 6; ++side) {
                        childName = side.ToString();
                        childArrayIndex = getArrayPosition(childName);
                        lodObj = allChildObjs[childArrayIndex];
                        if (lodObj == null) { // Jos objektia ei ole niin generoi sen
                            lodObj = planetData.GeneratePlanetChunk(childName, QualitySettings.CurrentLOD.gridSize[i], myTransform.position, childName);
                            //lodObj = CreateLodObject(childName, childName, LOD_settings.CurrentLOD.gridSize[i], myTransform.position);
                            lodObj.transform.parent = myTransform;
                            allChildObjs[childArrayIndex] = lodObj;
                            lodObjects.Add(lodObj);
                        }
                        else { // Jos objekti on jo olemassa

                            if (nextLod) { // Jos voi olla tarkempi lod
                                
                                // TODO: tee tähän ettei kato keskeltä vaan että katsoo keskustan ja kulmien välistä
                                if (Vector3.Distance(player.position, myTransform.position + getMiddleVerticePos(childName, i)) < QualitySettings.CurrentLOD.distance[i] * planetData.planetSize) { // Jos on tarkemman lodin sisäpuolella niin disablettaa ittensä
                                    lodObj.SetActive(false);
                                    // ---------------- QUARTERIEN TARKISTUS ----------------
                                    // Jos seuraava lod on quarter niin tekee tässä quarterit
                                    if (QualitySettings.CurrentLOD.lodSide[i + 1] == QualitySettings.LODSide.Quarter) {
                                        LODQuarters(childName, i + 1);
                                    }
                                    // ------------------------------------------------------
                                    
                                }
                                else { // Jos on seuraavasta lodista kauempana..
                                    lodObj.SetActive(true);
                                    lodObjects.Add(lodObj);
                                    breakkaa = true;
                                }
                            }

                        }

                    }
                    break;

            }
            if (breakkaa) { return; }


        }
        #endregion



    }




    // - Sivujen/neljäsosien neljäsosat rekursiivisesti -
    void LODQuarters(string side, int lodIndex) {
        
        for (int i = lodIndex, len = QualitySettings.CurrentLOD.LodsCount; i < len; ++i) {
            
            bool nextLod = false; // onko seuraavan tason lod tarkkuutta olemassa
            if (i != len) { nextLod = true; }

            // Looppaa jokaisen neljäsosan läpi
            
            bool breakkaa = false; // breakkaa variable hoitelee, ettei looppaa turhaan pitemmälle mitä pitäis




            for (int quarter = 0; quarter < 4; ++quarter) {
                childName = side + quarter.ToString();
                childArrayIndex = getArrayPosition(childName);
                lodObj = allChildObjs[childArrayIndex];



                if (lodObj == null) { // Jos objektia ei ole niin generoi sen
                    lodObj = planetData.GeneratePlanetChunk(childName, QualitySettings.CurrentLOD.gridSize[i], myTransform.position, childName);
                    //lodObj = CreateLodObject(childName, childName, LOD_settings.CurrentLOD.gridSize[i], myTransform.position);
                    lodObj.transform.parent = myTransform;
                    allChildObjs[childArrayIndex] = lodObj;
                    lodObjects.Add(lodObj);
                }
                else { // Jos objekti on jo olemassa

                    if (nextLod == true) { // Jos voi olla tarkempi lod

                        if (Vector3.Distance(player.position, myTransform.position + getMiddleVerticePos(childName, i)) < QualitySettings.CurrentLOD.distance[i] * planetData.planetSize) { // Jos on tarkemman lodin sisäpuolella niin disablettaa ittensä

                            // -- TÄSSÄ TARKISTAA QUARTERIN ... QUARTERIT REKURSIIVISESTI --
                            LODQuarters(childName, i + 1);
                            // ---------------------------------------------------------
                            breakkaa = true;
                        }
                        else { // Jos on seuraavasta lodista kauempana..
                            lodObj.SetActive(true);
                            lodObjects.Add(lodObj);
                        }
                    }

                }




            }
            

            if (breakkaa) { return; }


        }


    }
    
    // Antaa array positionin nimestä
    int getArrayPosition(string name) {
        int pos = 0, len = childName.Length;

        // Full
        if (childName[0] == 'F') {
            return (int)char.GetNumericValue(name[1]);
        }
        pos += fullCount;
        
        // Side
        if (len == 1) {
            return pos + (int)char.GetNumericValue(name[0]);
        }
        pos += sideCount-1;

        
        switch (name[0]) {
            case '0': // pohja
                break;
            case '1': // vas
                pos += quarterCount / 6;
                break;
            case '2': // keski
                pos += quarterCount / 6 * 2;
                break;
            case '3': // oik
                pos += quarterCount / 6 * 3;
                break;
            case '4': // ylä
                pos += quarterCount / 6 * 4;
                break;
            case '5': // taka
                pos += quarterCount / 6 * 5;
                break;
        }
        // Quarters
        for (int i = 1; i < len; ++i) {
            switch (name[i]) {
                case '0': // vasenala
                    pos += (int)Mathf.Pow(4, i-1);
                    break;
                case '1': // oikeeala
                    pos += 2 * (int)Mathf.Pow(4, i-1);
                    break;
                case '2': // vasenylä
                    pos += 3 * (int)Mathf.Pow(4, i-1);
                    break;
                case '3': // oikeeylä
                    pos += 4 * (int)Mathf.Pow(4, i-1);
                    break;
            }
        }
        return pos;
    }



    // Antaa keskimmäisen verticen positionin
    Vector3 getMiddleVerticePos(string name, int lodIndex) {
        
        int pos = getArrayPosition(name);
        
        if (verticesMiddlePos[pos] == Vector3.zero) {
            Vector3[] vertices = lodObj.GetComponent<MeshFilter>().mesh.vertices;

verticesMiddlePos[pos] = lodObj.transform.localRotation* (myTransform.localToWorldMatrix.MultiplyPoint3x4(vertices[(vertices.Length / 2)]) - myTransform.position) * planetData.planetSize; // todo alkuun myTransform.rotation * .. että tukee koko objectin rotaatiota myös
            return verticesMiddlePos[pos];
        }
        
        return verticesMiddlePos[pos];
    }

    

}

    */
