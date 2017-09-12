using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Quantizer : MMMSingleton<Quantizer> {

    public Texture2D inputTexture;
    internal Texture2D outputTexture;
    public bool trigger = false;
    public List<Color> colors;
    public List<Renderer> targetRenderers;
    public int hueBuckets = 5;
    private Dictionary<int, Color> bucketColors;
    public int width = 64;
    public List<RawImage> paletteRawImages;

    public delegate void paletteUpdate(List<Color> colors);
    public event paletteUpdate OnPaletteUpdate;

    void Start () {
	
	}

    void Trigger()
    {

        bucketColors = new Dictionary<int, Color>();
        colors = new List<Color>();

        foreach (var pixelColor in outputTexture.GetPixels())
        {
            // Get Hue & Bucket
            Vector3 hsv = RGBToVec3(pixelColor);
            float hue = hsv.x;
            if (hsv.z < 0.5f || hsv.z > 0.9f)
                continue;

            int bucketIndex = Mathf.RoundToInt(Mathf.Lerp(0, hueBuckets + 1, hue));
            //    Debug.LogFormat("Bucket Index for <color={0}>XXX</color> is {1}, hue is {2}", ColorToHex(pixelColor), bucketIndex, hue);
            // If Bucket is empty, add color. Otherwise, average it
            if (bucketColors.ContainsKey(bucketIndex))
            {
                // Average with existing color
                Color existingColor = bucketColors[bucketIndex];
                Color averageColor = Color.Lerp(existingColor, pixelColor, 0.5f);
                float existingColorValue = RGBToVec3(existingColor).z;
                Color maxColor = (existingColorValue > hsv.z) ? existingColor : pixelColor;
                bucketColors[bucketIndex] = maxColor;
            }
            else
            {
                bucketColors[bucketIndex] = pixelColor;
            }
        }

        foreach (var item in bucketColors)
        {
            colors.Add(item.Value);
        }

        MMM.MMMColors.Instance.palette = colors;

        // set target material textures
        foreach (var item in targetRenderers)
        {
            item.material.mainTexture = inputTexture;
        }

        GenerateAndSetPaletteTexture();
    }

    void GenerateAndSetPaletteTexture()
    {
        // generate palette texture and set rawimage
        int paletteTextureWidth = 10;
        int paletteTextureHeight = 2048;
        Texture2D texture = new Texture2D(paletteTextureWidth, paletteTextureHeight, TextureFormat.RGB24, false);
        int stripeHeight = Mathf.CeilToInt((float)paletteTextureHeight / (float)colors.Count);
        for (int i = 0; i < colors.Count; i++)
        {
            Color color = colors[i];
            for (int col = 0; col < paletteTextureWidth; col++)
            {
                int startRow = stripeHeight * i;
                int endRow = (stripeHeight * (i + 1)) - 1;
                for (int row = startRow; row <= endRow; row++)
                {
                    if (row == startRow || row == endRow)
                        texture.SetPixel(col, row, Color.black);
                    else
                        texture.SetPixel(col, row, color);
                }
            }
        }
        texture.Apply();
        foreach (var item in paletteRawImages)
        {
            item.texture = texture;
        }
    }

    // Update is called once per frame
    void Update () {
        if (trigger)
        {
            trigger = false;
            Downsample();
            Trigger();
        }
	}

    void Downsample()
    {
        Color32[] result = ScaleUnityTexture.ScalePoint(inputTexture.GetPixels32(0), inputTexture.width, width, width);
        outputTexture = new Texture2D(width, width);
        outputTexture.filterMode = FilterMode.Point;
        outputTexture.SetPixels32(result);
        outputTexture.Apply();
    }

    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    string ColorToHex(Color32 color)
    {
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Vector3 RGBToVec3(Color rgbColor)
    {
        float h = 0;
        float s = 0;
        float v = 0;
        RGBToHSV(rgbColor, out h, out s, out v);
        return new Vector3(h, s, v);
    }

    public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
    {
        if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
        {
            RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
        }
        else
        {
            if (rgbColor.g > rgbColor.r)
            {
                RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
            }
            else
            {
                RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
            }
        }
    }

    private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
    {
        V = dominantcolor;
        if (V != 0f)
        {
            float num = 0f;
            if (colorone > colortwo)
            {
                num = colortwo;
            }
            else
            {
                num = colorone;
            }
            float num2 = V - num;
            if (num2 != 0f)
            {
                S = num2 / V;
                H = offset + (colorone - colortwo) / num2;
            }
            else
            {
                S = 0f;
                H = offset + (colorone - colortwo);
            }
            H /= 6f;
            if (H < 0f)
            {
                H += 1f;
            }
        }
        else
        {
            S = 0f;
            H = 0f;
        }
    }

    internal Color GetRandomColor()
    {
        if(colors != null & colors.Count > 0)
        {
            return colors[UnityEngine.Random.Range(0, colors.Count)];
        } else
        {
            return Color.white;
        }
    }
}
