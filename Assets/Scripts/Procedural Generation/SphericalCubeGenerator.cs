using UnityEngine;
using System.Collections;
using System.Linq;



// TODO: järjestele tää koko filu uuestaan kunnolla

public static class SphericalCubeGenerator {

    
    private static Vector3[] osanKoordinaatit = { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) };
    private static Quaternion[] sideRotations = { Quaternion.Euler(new Vector2(270, 0)), Quaternion.Euler(new Vector2(0, 90)), Quaternion.Euler(new Vector2(0, 0)), Quaternion.Euler(new Vector2(0, -90)), Quaternion.Euler(new Vector2(90, 0)), Quaternion.Euler(new Vector2(180, 0)) };
    //sideRotations[(int)char.GetNumericValue(side[0])] *

    private static int fullCount;
    private static int sideCount;
    private static int quarterCount;
    private static Mesh[] sideMeshes;
    private static Vector3[] middleVerticePositions;



    // TODO: generate all sides on game start. nyt tekee tässä constructorissa vaan
    static SphericalCubeGenerator() {
        
        quarterCount = QualitySettings.GetLODSideCount(QualitySettings.LODSide.Quarter);
        sideMeshes = new Mesh[quarterCount / 6 + 1];
        middleVerticePositions = new Vector3[sideMeshes.Length * 6];
        /*
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        */
        sideMeshes[getArrayPosition("0")] = GenerateSphericalMesh("0", QualitySettings.CurrentLOD.gridSize[0]);
        GenerateAllMeshDataRecursively(QualitySettings.CurrentLOD.LodsCount, 0, "0"); // TODO: halutaanko ladata aina vaan kaikki, että voi aina suoraan hakea osoitteella meshit ja positionit ettei tarttee erillisen methodin kautta

        /*Debug.Log(sw.ElapsedMilliseconds);
        sw.Stop();*/

    }

    
    
    // Get Mesh
    public static Mesh GetMesh(int gridSize, string side = "") { // HUOM ! GRID SIZEn pitää olla jaollinen quarterilla tai jotain sinnepäin
        
        int arrayPos = getArrayPosition(side);
        if (sideMeshes[arrayPos] != null) {
            return sideMeshes[arrayPos];
        }
        
        // Ei haluta tässä enää tallentaa meshejä koska ladataan alussa vaan sen verran mitä halutaan ladatuksi ja sen jälkeen on hyvä
     
        return GenerateSphericalMesh(side, gridSize);
    }


    // Get Middle Vertice Position
    public static Vector3 getMiddleVerticePosition(string side) {
        int pos = getArrayPosition(side);
        return sideRotations[(int)char.GetNumericValue(side[0])] * sideMeshes[pos].vertices[sideMeshes[pos].vertexCount / 2];
    }



    // Generate All Mesh Data Recursively
    static void GenerateAllMeshDataRecursively(int quarterQualityLevels, int currentLevel, string side) {
        if (QualitySettings.CurrentLOD.lodSide[currentLevel] == QualitySettings.LODSide.Quarter) {
            for (int i = 0; i < 4; ++i) {
                if (currentLevel != quarterQualityLevels - 1) {
                    GenerateAllMeshDataRecursively(quarterQualityLevels, currentLevel + 1, side + i);
                }
                int position = getArrayPosition(side + i);
                sideMeshes[position] = GenerateSphericalMesh(side + i, QualitySettings.CurrentLOD.gridSize[currentLevel]);
                Vector3[] vertices = sideMeshes[position].vertices;
                middleVerticePositions[position] = vertices[vertices.Length / 2];
            }
        } else {
            GenerateAllMeshDataRecursively(quarterQualityLevels, currentLevel + 1, side);
        }
    }


     

    // - Generate Spherical cube side mesh -
    static Mesh GenerateSphericalMesh(string side, int gridSize) {

        float radius = 0.5f;
        int xSize = gridSize;
        int ySize = gridSize;

        Vector3[] vertices;
        Vector3[] normals;
        int[] triangles;
        Vector2[] uvs;

        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        uvs = new Vector2[vertices.Length];
        triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
        normals = new Vector3[vertices.Length];

        
        // Laskee kertoimen että saa oikeen paikan quarterin
        Vector3 quarterVec3 = Vector3.zero;

        // Huom! Eka kirjain on sivu ja muut quartereja eli eka numero ollaan tässä huomiotta
        int len = side.Length - 1;


        // Calculate side
        int c, kerroin;
        int zAkseli = -2 * (-1 + (int)Mathf.Pow(2, len));

        for (int index = 0; index < len; ++index) {
            c = (int)char.GetNumericValue(side[index + 1]);
            kerroin = (int)Mathf.Pow(2, len - index);

            quarterVec3 += new Vector3(osanKoordinaatit[c].x * kerroin, osanKoordinaatit[c].y * kerroin);

        }

        quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, quarterVec3.y / 2 + 1, zAkseli / 2 + 1);

        // Create vertices
        int v = -1;
        for (int y = 0; y <= gridSize; ++y) {
            for (int x = 0; x <= gridSize; ++x) {

                Vector3 vec = new Vector3(x, y, gridSize) * 2f / gridSize - quarterVec3;
                normals[++v] = -vec.normalized;
                vertices[v] = normals[v] * radius;
                uvs[v] = new Vector2(x, y);
                
            }
        }


        // Create triangles
        triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; ++y, ++vi) {
            for (int x = 0; x < xSize; ++x, ti += 6, ++vi) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }


		// Create mesh
        Mesh mesh = new Mesh {
            name = side,
            vertices = vertices,
            triangles = triangles,
            normals = normals,
            uv = uvs
        };

        return mesh;

    }




    // Antaa array positionin nimestä
    private static int getArrayPosition(string side) {
        int pos = 0, len = side.Length;

        // Quarters
        for (int i = 1; i < len; ++i) {
            switch (side[i]) {
                case '0': // vasenala
                    pos += (int)Mathf.Pow(4, i - 1);
                    break;
                case '1': // oikeeala
                    pos += 2 * (int)Mathf.Pow(4, i - 1);
                    break;
                case '2': // vasenylä
                    pos += 3 * (int)Mathf.Pow(4, i - 1);
                    break;
                case '3': // oikeeylä
                    pos += 4 * (int)Mathf.Pow(4, i - 1);
                    break;
            }
        }

        return pos;
    }








    // tee custom sized grid cube joskus ehkä esim asteroideihin
    /*public static SphereMeshData GenerateCube(string name, float xSize, float ySize, float zSize, int gridSize) {

        Vector3 localScale = new Vector3(1f / gridSize * xSize, 1f / gridSize * ySize, 1f / gridSize * zSize);

        SphereMeshData sphereMeshData = new SphereMeshData(gridSize, xSize, ySize, zSize);

        sphereMeshData.CreateMeshDataForSphericalCube();

        return sphereMeshData;
    }
    */












    // TODO: täällä on paljon arvokasta dataa jota pitää vielä hyödyntää, mutta en halunnu että pitää tehä erillinen objecti kun meshiä luodaan
    private class SphereMeshData {

        Vector3[] vertices;
        int[] triangles;
        int[] trianglesBO;
        int[] trianglesLE;
        int[] trianglesFR;
        int[] trianglesRI;
        int[] trianglesTO;
        int[] trianglesBA;

        Vector3[] normals;
        Vector2[] uvs;

        int gridSize;
        float radius;
        int xSize;
        int ySize;
        int zSize;
        int quads;

        Vector3[] osanKoordinaatit = { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) };  // 0 - vasen ala, 1 - oikee ala, 2 - vasen ylä, 3 - oikee ylä


        /*  eka merkki:
            "" - Full, 0 - bottom, 1 - left, 2 - front, 3 - right, 4 - top, 5 - bottom
            loput merkit:
            0 - vasen ala, 1 - oikee ala, 2 - vasen ylä, 3 - oikee ylä  */
        string side;


        // -- Spherical cube --

        // Mesh Data initialization
        public SphereMeshData(int gridSize, string side) {
            this.side = side;
            this.radius = 0.5f;
            this.gridSize = gridSize;
            this.xSize = gridSize;
            this.ySize = gridSize;
            this.zSize = gridSize;

            // Full spherical cube
            if (side == "") {
                int cornerVertices = 8;
                int edgeVertices = (gridSize + gridSize + gridSize - 3) * 4;
                int faceVertices = (
                    (gridSize - 1) * (gridSize - 1) +
                    (gridSize - 1) * (gridSize - 1) +
                    (gridSize - 1) * (gridSize - 1)) * 2;
                vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
                uvs = null;
                quads = (gridSize * gridSize) * 6;
                triangles = new int[quads * 6];
                trianglesBO = new int[quads];
                trianglesLE = new int[quads];
                trianglesFR = new int[quads];
                trianglesRI = new int[quads];
                trianglesTO = new int[quads];
                trianglesBA = new int[quads];
            }
            // Spherical cube side
            else {
                vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
                uvs = new Vector2[vertices.Length];
                triangles = new int[gridSize * gridSize * 2];
                //triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
            }

            normals = new Vector3[vertices.Length];


        }


        // Create mesh data
        Mesh GenerateMesh() {

            if (side == "") { // Full
                CreateSphericalCube();              // vertices, normals
                CreateTriangles();                  // triangles
            } else { // Side and quarters
                GenerateSphericalCubeSide();
                //CreateSphericalCubeSide();          // vertices, normals, uvs
                CreateTrianglesForPlane();          // triangles
            }

            return CreateMesh();
        }

        // Spherical cube
        void CreateSphericalCube() { // tarkista tämän järjestykset jossain vaiheessa

            int v = -1;
            for (int y = 0; y <= gridSize; ++y) {
                for (int x = 0; x <= gridSize; ++x) {
                    SetVertex(++v, x, y, 0);
                }
                for (int z = 1; z <= gridSize; ++z) {
                    SetVertex(++v, gridSize, y, z);
                }
                for (int x = gridSize - 1; x >= 0; --x) {
                    SetVertex(++v, x, y, gridSize);
                }
                for (int z = gridSize - 1; z > 0; --z) {
                    SetVertex(++v, 0, y, z);
                }
            }
            for (int z = 1; z < gridSize; ++z) {
                for (int x = 1; x < gridSize; ++x) {
                    SetVertex(++v, x, gridSize, z);
                }
            }
            for (int z = 1; z < gridSize; ++z) {
                for (int x = 1; x < gridSize; ++x) {
                    SetVertex(++v, x, 0, z);
                }
            }
        }






        // Generoi vain etuseinän jota sitten käännellään
        void GenerateSphericalCubeSide() {


            // Laskee kertoimen että saa oikeen paikan quarterin
            Vector3 quarterVec3 = Vector3.zero;

            // Huom! Eka kirjain on sivu ja muut quartereja eli eka numero ollaan tässä huomiotta
            int len = side.Length - 1;


            int c, kerroin;
            int zAkseli = -2 * (-1 + (int)Mathf.Pow(2, len));

            for (int index = 0; index < len; ++index) {
                c = (int)System.Char.GetNumericValue(side[index + 1]);
                kerroin = (int)Mathf.Pow(2, len - index);

                quarterVec3 += new Vector3(osanKoordinaatit[c].x * kerroin, osanKoordinaatit[c].y * kerroin);

            }

            quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, quarterVec3.y / 2 + 1, zAkseli / 2 + 1);


            // Sen jäljkeen tekee vertices
            int v = -1;
            for (int y = 0; y <= gridSize; ++y) {
                for (int x = 0; x <= gridSize; ++x) {
                    SetVertexAdvanced(++v, x, y, gridSize, quarterVec3);
                    //SetVertex(++v, x, y, quarterVec3);
                }
            }



        }






        


        // TODO: tee tää joskus ehkä loppuun mut tätä ei enää käytetä koska miks tehä identtiset eripuolille kun voi kaikilla käyttää samaa ja tallentaa muistiin heti pelin aluks niin ei tarttee generoida koskaan uusia näitä ja voi vaan käännellä ne oikeisiin paikkoihin. ja tekstuuri laskee ittensä oikeinpäin 3d spherical cubeksi
        // Spherical cube side ---------------------------------------
        void CreateSphericalCubeSide() {

            // Laskee kertoimen että saa oikeen paikan quarterin
            Vector3 quarterVec3 = Vector3.zero;

            // Huom! Eka kirjain on sivu ja muut quartereja eli eka numero ollaan tässä huomiotta
            int len = side.Length - 1;

            if (len != 0) {

                int c, kerroin;
                int zAkseli = -2 * (-1 + (int)Mathf.Pow(2, len));

                for (int index = 0; index < len; ++index) {
                    c = (int)System.Char.GetNumericValue(side[index + 1]);
                    kerroin = (int)Mathf.Pow(2, len - index);

                    quarterVec3 += new Vector3(osanKoordinaatit[c].x * kerroin, osanKoordinaatit[c].y * kerroin);

                }

                switch (side[0]) { // Eri sivuille oikeet x, y ja z
                    case '2': // Front
                        quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, quarterVec3.y / 2 + 1, zAkseli / 2 + 1);
                        break;
                    case '5': // Back
                        quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, -quarterVec3.y / 2 + 1, -zAkseli / 2 + 1);
                        break;
                    case '4': // Top
                        quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, -zAkseli / 2 + 1, quarterVec3.y / 2 + 1);
                        break;
                    case '0': // Bottom
                        quarterVec3 = new Vector3(quarterVec3.x / 2 + 1, zAkseli / 2 + 1, -quarterVec3.y / 2 + 1);
                        break;
                    case '1': // Left
                        quarterVec3 = new Vector3(zAkseli / 2 + 1, quarterVec3.y / 2 + 1, -quarterVec3.x / 2 + 1);
                        break;
                    case '3': // Right
                        quarterVec3 = new Vector3(-zAkseli / 2 + 1, quarterVec3.y / 2 + 1, quarterVec3.x / 2 + 1);
                        break;
                }

            } else {
                quarterVec3 = Vector3.one;
            }

            // Sen jäljkeen tekee vertices ja muuttaa quarterVec3 vektorit oikeiks eri sivuille
            int v = -1;
            switch (side[0]) {
                case '2': // Front
                    for (int y = 0; y <= gridSize; ++y) {
                        for (int x = 0; x <= gridSize; ++x) {
                            SetVertex(++v, x, y, gridSize, quarterVec3);
                        }
                    }
                    break;
                case '5': // Back
                    for (int y = gridSize; y >= 0; --y) {
                        for (int x = 0; x <= gridSize; ++x) {
                            SetVertex(++v, x, y, 0, quarterVec3);
                        }
                    }
                    break;
                case '4': // Top
                    for (int z = 0; z <= gridSize; ++z) {
                        for (int x = 0; x <= gridSize; ++x) {
                            SetVertex(++v, x, 0, z, quarterVec3);
                        }
                    }
                    break;
                case '0': // Bottom
                    for (int z = gridSize; z >= 0; --z) {
                        for (int x = 0; x <= gridSize; ++x) {
                            SetVertex(++v, x, gridSize, z, quarterVec3);
                        }
                    }
                    break;
                case '1': // Left
                    for (int y = 0; y <= gridSize; ++y) {
                        for (int z = gridSize; z >= 0; --z) {
                            SetVertex(++v, gridSize, y, z, quarterVec3);
                        }
                    }
                    break;
                case '3': // Right
                    for (int y = 0; y <= gridSize; ++y) {
                        for (int z = 0; z <= gridSize; ++z) {
                            SetVertex(++v, 0, y, z, quarterVec3);
                        }
                    }
                    break;
            }
        }



        // Set spherical cube vertex. Mutta tehdään aina vaan x ja y suunnassa
        /*void SetVertex(int i, int x, int y, Vector3 quarterVec3) {
        
            Vector3 v = new Vector3(x, y, gridSize) * 2f / gridSize - quarterVec3;

            normals[i] = v.normalized;
  
            vertices[i] = normals[i] * radius;

            uvs[i] = new Vector2(x, y);
            
        }*/

        //SetVertex 'advanced'
        private void SetVertexAdvanced(int i, int x, int y, int z, Vector3 quarterKerroinVector3) {
            Vector3 v = new Vector3(x, y, z) * 2f / gridSize - quarterKerroinVector3;
            //float x2 = v.x * v.x;
            //float y2 = v.y * v.y;
            //float z2 = v.z * v.z; // ei haluta koskee z akseliin nyt koska tehdään kaikki vaan x y akselille
            normals[i] = -v.normalized;
            /*Vector3 s; // tää sydeemi ei toiminu mut se ei haittaa kosa 
            s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f); 
            s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
            s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);*/
            //normals[i] = s;
            vertices[i] = -normals[i] * radius;
            uvs[i] = new Vector2(x, y);
        }




        // Set spherical cube vertex (Alkuperäinen toimiva ainakin melkein)
        void SetVertex(int i, int x, int y, int z, Vector3? quarterKerroinVector3 = null) {

            if (quarterKerroinVector3 == null) {
                quarterKerroinVector3 = Vector3.one;
            }
            Vector3 vec3 = (Vector3)quarterKerroinVector3;

            Vector3 v = new Vector3(x, y, z) * 2f / gridSize - vec3;

            if (side != "") {
                normals[i] = -v.normalized;
            } else {
                normals[i] = v.normalized;
            }


            vertices[i] = normals[i] * radius;
            
            if (uvs != null) {
                if (x == gridSize) {
                    uvs[i] = new Vector2(z, y);
                } else if (y == gridSize) {
                    uvs[i] = new Vector2(x, z);
                } else if (z == gridSize) {
                    uvs[i] = new Vector2(x, y);
                }
            }

        }



        // Create triangles for plane
        void CreateTrianglesForPlane() {

            triangles = new int[xSize * ySize * 6];
            for (int ti = 0, vi = 0, y = 0; y < ySize; ++y, ++vi) {
                for (int x = 0; x < xSize; ++x, ti += 6, ++vi) {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }

        }




        // TODO: -- Custom grid sized cube -- ..jos jaksan tehä tämmöstä koskaan ):
        #region Cube
        // Mesh Data initialization
        /*public SphereMeshData(string name, int xsize, int ysize, int zsize) {
            xSize = xsize;
            ySize = ysize;
            zSize = zsize;
            quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
            vertices = new Vector3[gridSize * gridSize]; // gridsize+1, gridsize+1
            uvs = new Color32[gridSize * gridSize];
            //triangles = new int[(gridSize - 1) * (gridSize - 1) * 6];
            normals = new Vector3[gridSize * gridSize];

        }
        */

        // Create vertices for cube
        void CreateVertices() {

            /*
            int cornerVertices = 8;
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            int faceVertices = (
                (xSize - 1) * (ySize - 1) +
                (xSize - 1) * (zSize - 1) +
                (ySize - 1) * (zSize - 1)) * 2;

            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            */

            int v = -1;
            for (int y = 0; y <= ySize; ++y) { // korkeus
                for (int x = 0; x <= xSize; ++x) { //etu
                    vertices[++v] = new Vector3(x, y, 0);
                }
                for (int z = 1; z <= zSize; ++z) { // oikea
                    vertices[++v] = new Vector3(xSize, y, z);
                }
                for (int x = xSize - 1; x >= 0; --x) { // taka
                    vertices[++v] = new Vector3(x, y, zSize);
                }
                for (int z = zSize - 1; z > 0; --z) { // vasen
                    vertices[++v] = new Vector3(0, y, z);
                }
            }
            for (int z = 1; z < zSize; ++z) { // pohja
                for (int x = 1; x < xSize; ++x) {
                    vertices[++v] = new Vector3(x, ySize, z);
                }
            }
            for (int z = 1; z < zSize; ++z) { // katto
                for (int x = 1; x < xSize; ++x) {
                    vertices[++v] = new Vector3(x, 0, z);
                }
            }

        }

        // Create triangles for cube
        void CreateTriangles() {

            /*trianglesLE = new int[gridSize * gridSize * 6];
            trianglesRI = new int[gridSize * gridSize * 6];
            trianglesTO = new int[gridSize * gridSize * 6];
            trianglesBO = new int[gridSize * gridSize * 6];
            trianglesFR = new int[gridSize * gridSize * 6];
            trianglesBA= new int[gridSize * gridSize * 6];*/

            int ring = (xSize + zSize) * 2;
            int tFR = 0, tBA = 0, tLE = 0, tRI = 0, tTO = 0, tBO = 0, v = 0;

            for (int y = 0; y < ySize; ++y, ++v) {

                for (int q = 0; q < xSize; ++q, ++v) {
                    tFR = SetQuad(trianglesFR, tFR, v, v + 1, v + ring, v + ring + 1);
                }
                for (int q = 0; q < zSize; ++q, ++v) {
                    tRI = SetQuad(trianglesRI, tRI, v, v + 1, v + ring, v + ring + 1);
                }
                for (int q = 0; q < xSize; ++q, ++v) {
                    tBA = SetQuad(trianglesBA, tBA, v, v + 1, v + ring, v + ring + 1);
                }
                for (int q = 0; q < zSize - 1; ++q, ++v) {
                    tLE = SetQuad(trianglesLE, tLE, v, v + 1, v + ring, v + ring + 1);
                }

                tLE = SetQuad(trianglesLE, tLE, v, v - ring + 1, v + ring, v + 1);



            }
            tTO = CreateTopFace(trianglesTO, tTO, ring);
            tBO = CreateBottomFace(trianglesBO, tBO, ring);

        }
        /*void CreateTriangles() {

            int ring = (xSize + zSize) * 2;
            int t = 0, v = 0;


            for (int y = 0; y < ySize; y++, v++) {
                for (int q = 0; q < ring - 1; q++, v++) {
                    t = SetQuad(t, v, v + 1, v + ring, v + ring + 1);
                }
                t = SetQuad(t, v, v - ring + 1, v + ring, v + 1);
            }

            t = CreateTopFace(t, ring);
            t = CreateBottomFace(t, ring);

        }*/
        // Top face
        int CreateTopFace(int[] triangles, int t, int ring) {
            int v = ring * ySize;
            for (int x = 0; x < xSize - 1; x++, v++) {
                t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
            }
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

            int vMin = ring * (ySize + 1) - 1;
            int vMid = vMin + 1;
            int vMax = v + 2;

            for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
                t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
                for (int x = 1; x < xSize - 1; x++, vMid++) {
                    t = SetQuad(
                        triangles,
                        t,
                        vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
            }

            int vTop = vMin - 2;
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMin - 2);
            for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
                t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
            }
            t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

            return t;
        }
        // Bottom face
        int CreateBottomFace(int[] triangles, int t, int ring) {
            int v = 1;
            int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
            t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
            for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
                t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
            }
            t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

            int vMin = ring - 2;
            vMid -= xSize - 2;
            int vMax = v + 2;

            for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
                t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
                for (int x = 1; x < xSize - 1; x++, vMid++) {
                    t = SetQuad(
                        triangles,
                        t,
                        vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
                }
                t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
            }

            int vTop = vMin - 1;
            t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
            for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
                t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

            return t;
        }


        // Set Quad
        int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11) {
            triangles[i] = v00;
            triangles[i + 1] = triangles[i + 4] = v01;
            triangles[i + 2] = triangles[i + 3] = v10;
            triangles[i + 5] = v11;
            return i + 6;
        }
        #endregion




        // -- Create Mesh --
        public Mesh CreateMesh() {
            Mesh mesh = new Mesh();
            mesh.name = side;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;

            // Side
            if (uvs != null) {
                mesh.uv = uvs;
            }
            // Full
            else {
                mesh.subMeshCount = 6;
                mesh.SetTriangles(trianglesBO, 0);
                mesh.SetTriangles(trianglesLE, 1);
                mesh.SetTriangles(trianglesFR, 2);
                mesh.SetTriangles(trianglesRI, 3);
                mesh.SetTriangles(trianglesTO, 4);
                mesh.SetTriangles(trianglesBA, 5);
            }

            return mesh;
        }


    }

    


}


