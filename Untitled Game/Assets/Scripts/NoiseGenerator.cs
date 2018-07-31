using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour {
    private void Start() {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        int width = 128, height = 128;
        Texture2D texture = new Texture2D(width, height);
        float[,] values = new float[height, width];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float random = Random.value;
                random = random > 0.8f ? 1.0f : 0.0f;
                values[y, x] = random;
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float value = values[y, x];
                texture.SetPixel(x, y, new Color(value, value, value));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        meshRenderer.material.mainTexture = texture;
    }

    private float[,] WhiteNoise(int width, int height) {
        float[,] result = new float[height, width];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                result[y, x] = Random.value;
            }
        }
        return result;
    }
}
