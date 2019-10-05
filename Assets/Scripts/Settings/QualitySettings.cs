using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QualitySettings {


    // -- INIT --

    // Enums
    public enum QualityLevel { Low = 0, Medium = 1, High = 2, Ultra = 3 }
    public enum LODSide { Full, Side, Quarter }

    // Variables
    static QualityLevel LODsetting;
    private static LevelOfDetail[] levelOfDetails = new LevelOfDetail[System.Enum.GetNames(typeof(QualityLevel)).Length];
    private static LevelOfDetail currentLOD;
    public static LevelOfDetail CurrentLOD { get { return currentLOD; } }

    // - Constructor -
    static QualitySettings() {
        InitLODs();
        ChangeLOD(QualityLevel.Medium);
    }
    
        


    // -- BLUEPRINTS --

    // LOD blueprint
    public class LevelOfDetail { // Level of details inside presets

        private int lodsCount;
        public int LodsCount { get { return lodsCount; } }

        public QualityLevel qualityLevel;
        public int[] lod;
        public float[] distance;
        public int[] gridSize;
        public int[] tesselation;
        public bool[] hasHeight;
        public bool[] hasCollider;
        public LODSide[] lodSide;

        public LevelOfDetail(int[] lod, float[] distance, int[] gridSize, int[] tesselation, bool[] hasHeight, bool[] hasCollider, LODSide[] lodSide) {
            this.lodsCount = lod.Length;
            this.lod = lod;
            this.distance = distance;
            this.gridSize = gridSize;
            this.tesselation = tesselation;
            this.hasHeight = hasHeight;
            this.hasCollider = hasCollider;
            this.lodSide = lodSide;
        }


    }
    


    // -- FUNCTIONS --

    // Get LOD
    public static LevelOfDetail GetLOD(QualityLevel qualityLevel) {
        return levelOfDetails[(int)qualityLevel];
    }

    // Change LOD setting
    public static void ChangeLOD(QualityLevel qualityLevel) {
        currentLOD = levelOfDetails[(int)qualityLevel];
    }


    // Init level of details
    public static void InitLODs() {


		//int[] lod = { 0, 1, 2, 3 };
		//float[] distance = { 5, 2, 0.5f, 0 }; // Viimeinen pitää olla 0. vertaa siis aina edelliseen
		//int[] gridSize = { 8, 8, 8, 8 };
		//int[] tesselation = { 1, 1, 1, 1  };
		//bool[] hasHeight = { false, false, false, false};
		//bool[] hasCollider = { false, false, false, true };
		//LODSide[] lodSide = { LODSide.Full, LODSide.Side, LODSide.Quarter, LODSide.Quarter};

		//levelOfDetails[(int)QualityLevel.Medium] = new LevelOfDetail(lod, distance, gridSize, tesselation, hasHeight, hasCollider, lodSide);



		//int[] lod = { 0, 1, 2 };
		//float[] distance = { 10, 0.001f, 0 }; // Viimeinen pitää olla 0. vertaa siis aina edelliseen
		//int[] gridSize = { 8, 8, 16 };
		//int[] tesselation = { 0, 0, 0 };
		//bool[] hasCollider = { false, false, true };
		//LODSide[] lodSide = { LODSide.Full, LODSide.Side, LODSide.Quarter };

		//levelOfDetails[(int)QualityLevel.Medium] = new LevelOfDetail(lod, distance, gridSize, tesselation, hasCollider, lodSide);




		// TÄÄ ON HYVÄ TÄLLÄHETKELLÄ !!!!
		int[] lod = { 0, 1, 2, 3, 4, 5, 6 };
		float[] distance = { 5, 2, 0.5f, 0.25f, 0.1f, 0.05f, 0 }; // Viimeinen pitää olla 0. vertaa siis aina edelliseen
		int[] gridSize = { 8, 8, 8, 32, 128, 128, 128 };
		int[] tesselation = { 1, 1, 1, 1, 1, 1, 1 };
		bool[] hasHeight = { false, false, true, true, true, true, true };
		bool[] hasCollider = { false, false, false, false, false, false, true };
		LODSide[] lodSide = { LODSide.Full, LODSide.Side, LODSide.Quarter, LODSide.Quarter, LODSide.Quarter, LODSide.Quarter, LODSide.Quarter };

		levelOfDetails[(int)QualityLevel.Medium] = new LevelOfDetail(lod, distance, gridSize, tesselation, hasHeight, hasCollider, lodSide);


		//int[] lod = { 0, 1, 2, 3 };
		//float[] distance = { 5, 2, 0.5f, 0f }; // Viimeinen pitää olla 0. vertaa siis aina edelliseen
		//int[] gridSize = { 64, 64, 64, 128 };
		//int[] tesselation = { 1, 1, 1, 1, 1 };
		//bool[] hasHeight = { false, false, true, true };
		//bool[] hasCollider = { false, false, false, true };
		//LODSide[] lodSide = { LODSide.Full, LODSide.Side, LODSide.Quarter, LODSide.Quarter, LODSide.Quarter, LODSide.Quarter };

		//levelOfDetails[(int)QualityLevel.Medium] = new LevelOfDetail(lod, distance, gridSize, tesselation, hasHeight, hasCollider, lodSide);



		// TODO: oikeet lod settingssit

		// - Rubbish -


		// - Low -


		// - Medium -


		// - High -


		// - Ultra -


		// - Future -


	}


	// Get LOD side count
	public static int GetLODSideCount(LODSide lodSide) {
        
        int currentQuarter = 0;
        int count = 0;

        for (int i = 0, len = currentLOD.LodsCount; i < len; ++i) {
            switch (currentLOD.lodSide[i]) {
                case LODSide.Full:
                    if (lodSide == LODSide.Full) {
                        ++count;
                    }
                    break;
                case LODSide.Side: // Tällä hetkellä saa ollakkin vaan 1 kpl side lod että tää on turha nyt mut niih.
                    if (lodSide == LODSide.Side) {
                        count += 6;
                    }
                    break;
                case LODSide.Quarter:
                    if (lodSide == LODSide.Quarter) {
                        count += 24 * (int)Mathf.Pow(4, currentQuarter++);
                    }
                    break;
            }
        }

        return count;

    }


    
}
