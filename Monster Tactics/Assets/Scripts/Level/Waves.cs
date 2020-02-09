using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField]
    private Vector2Int Size;

    private GameObject[,] waveTiles;

    [SerializeField]
    private GameObject WaterTile;

    [SerializeField]
    private Vector2 perlinOffset;

    [SerializeField, Range(0, .5f), LabelText("Intensity")]
    private float stupidOffsetMultiplier;

    [SerializeField]
    private float maxHeight = .3f;

    [SerializeField]
    private Vector2 speed;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    private void Start()
    {
        waveTiles = new GameObject[Size.x, Size.y];

        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                waveTiles[x, y] = Instantiate(WaterTile, new Vector3(x - Size.x / 2, 0, y - Size.y / 2),
                    Quaternion.Euler(90, 0, 0), transform);
            }
        }
    }

    private void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            perlinOffset += speed;

            for (int x = 0; x < waveTiles.GetLength(0); x++)
            {
                for (int y = 0; y < waveTiles.GetLength(1); y++)
                {
                    GameObject waveTile = waveTiles[x, y];

                    Vector2 offset = perlinOffset + new Vector2(x, y);

                    Vector3 position = waveTile.transform.localPosition;
                    position = new Vector3(position.x,
                        Mathf.PerlinNoise(offset.x * stupidOffsetMultiplier, offset.y * stupidOffsetMultiplier) *
                        maxHeight,
                        position.z);
                    waveTile.transform.localPosition = position;
                }
            }
        }
    }
}