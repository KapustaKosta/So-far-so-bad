using UnityEngine;

public class NoiseMapGenerator
{
    private int width;
    private int height;
    private Vector2 offset;

    private float scale;
    private int octaves;
    private float persistence;
    private float lacunarity;
    private int seed;

    [SerializeField] private float level = 0.5f; // �� 0 �� 1, �������� ������ ����� ������ ������� ����

    public NoiseMapGenerator(int w, int h, Vector2 o, int s): this(w, h, o, 15, 4, 0.5f, 2f, s)
    { }

    public NoiseMapGenerator(int w, int h, Vector2 o, float sc, int oct, float p, float l, int s)
    {
        width = w;
        height = h;
        scale = sc;
        octaves = oct;
        persistence = p;
        lacunarity = l;
        seed = s;
        offset = o;
    }

    public int[,] GenerateNoiseMap()
    {
        // ������ ������ � ��������, ���������� ��� ������� ���������� �� ������ ������ ������������
        float[] noiseMap = new float[width * height];

        // ����������� �������
        System.Random rand = new System.Random(seed);

        // ����� �����, ����� ��� ��������� ���� �� ����� �������� ����� ���������� ��������
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            // ��������� ������� ����� ���������
            float xOffset = rand.Next(-100000, 100000) + offset.x;
            float yOffset = rand.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(xOffset / width, yOffset / height);
        }

        if (scale < 0)
        {
            scale = 0.0001f;
        }

        // ��������� �������� ������ � ������, ��� ����� ��������� ��������� ��������� ��������
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // ���������� ����� �� ����� �����
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // ����� �������� ��� ������ ������
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                float superpositionCompensation = 0;

                // ��������� ��������� �����
                for (int i = 0; i < octaves; i++)
                {
                    // ������������ ���������� ��� ��������� �������� �� ���� �������
                    float xResult = (x - halfWidth) / scale * frequency + octavesOffset[i].x * frequency;
                    float yResult = (y - halfHeight) / scale * frequency + octavesOffset[i].y * frequency;

                    // ��������� ������ �� ����
                    float generatedValue = Mathf.PerlinNoise(xResult, yResult);
                    // ��������� �����
                    noiseHeight += generatedValue * amplitude;
                    // ������������ ��������� �����, ����� �������� � �������� ��������� [0,1]
                    noiseHeight -= superpositionCompensation;

                    // ������ ���������, ������� � ����������� ��� ��������� ������
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    superpositionCompensation = amplitude / 2;
                }

                // ��������� ����� ��� ����� �����
                // ��-�� ��������� ����� ���� ����������� ������ �� ������� ��������� [0,1]
                noiseMap[y * width + x] = Mathf.Clamp01(noiseHeight);
            }
        }

        return GenerateWallsMap(width, height, TransformTo2DMap(noiseMap));
    }

    private float[,] TransformTo2DMap(float[] noiseMap)
    {
        int tbm = MapManager.tilesBeyoundMap;
        float[,] map = new float[height + 2 * tbm, width];
        for (int i = 0; i < height + 2*tbm; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (i < tbm || i >= height + tbm)
                {
                    map[i, j] = -1.0f;
                }
                else
                {
                    int x = j;
                    int y = i - tbm;
                    map[i, j] = noiseMap[y * width + x];
                }
            }
        }
        return map;
    }

    private int[,] GenerateWallsMap(int width, int height, float[,] noiseMap)
    {
        int tbm = MapManager.tilesBeyoundMap;
        int[,] tileMap = new int[height + 2 * tbm, width];
        for (int i = 0; i < height + 2 * tbm; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (noiseMap[i, j] == -1.0f)
                {
                    tileMap[i, j] = MapManager.MAP_BORDER;
                }
                else if (noiseMap[i, j] >= level)
                {
                    tileMap[i, j] = MapManager.WALL;
                }
                else
                {
                    tileMap[i, j] = MapManager.FLOOR;
                }
            }
        }
        return tileMap;
    }
}
