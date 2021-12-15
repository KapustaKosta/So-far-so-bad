using UnityEngine;

/*public enum MapType
{
    Noise,
    Color
}*/

public class NoiseMap : MonoBehaviour
{
    // �������� ������ ��� ������ ���������� ����
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public float scale;

    [SerializeField] public int octaves;
    [SerializeField] public float persistence;
    [SerializeField] public float lacunarity;

    [SerializeField] public int seed;
    [SerializeField] public Vector2 offset;

    // [SerializeField] public MapType type = MapType.Noise;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // ���������� �����
        float[] noiseMap = NoiseMapGenerator.GenerateNoiseMap(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        // ������� ����� � ��������
        NoiseMapRenderer mapRenderer = FindObjectOfType<NoiseMapRenderer>();
        mapRenderer.RenderMap(width, height, noiseMap/*, type*/);
    }
}