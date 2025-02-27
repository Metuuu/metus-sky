﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImageEffect : MonoBehaviour {

	public Shader shader;
	Material material;

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (material == null) {
			material = new Material(shader);
		}

		Graphics.Blit(source, destination, material);
	}

}
