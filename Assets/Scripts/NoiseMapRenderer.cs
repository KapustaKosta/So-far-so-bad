using System;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapRenderer : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer = null;
    [SerializeField] private float level1; // �� 0 �� 1, �������� ������ ����� ������ ������� ����
    [SerializeField] private float level2;
    
    // � ����������� �� ���� ������������ ��� ���� � �����-�����, ���� ������� ��������
    public void RenderMap(int width, int height, float[] noiseMap/*, MapType type*/)
    {
            ApplyColorMap(width, height, GenerateNoiseMap(noiseMap));
    }

    // �������� �������� � ������� ��� �����������
    private void ApplyColorMap(int width, int height, Color[] colors)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colors);
        texture.Apply();

        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f); ;
    }

    // ����������� ������ � ������� � ���� � ������ �����-����� ������, ��� �������� � ��������
    private Color[] GenerateNoiseMap(float[] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.Length];
        for (int i = 0; i < noiseMap.Length; i++)
        {
            if (noiseMap[i] < level1)
            {
                colorMap[i] = Color.white;
            }
            else if (noiseMap[i] >= level1 && noiseMap[i] < level2)
            {
                colorMap[i] = Color.red;
            }
            else
            {
                colorMap[i] = Color.black;
            }
        }
        return colorMap;
    }
}