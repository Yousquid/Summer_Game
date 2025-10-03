using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int width;
    public int height;
    public int[] gridData; 

    public LevelData(int w, int h)
    {
        width = w;
        height = h;
        gridData = new int[w * h];
    }

    public int GetCell(int x, int y)
    {
        return gridData[y * width + x];
    }

    public void SetCell(int x, int y, int value)
    {
        gridData[y * width + x] = value;
    }
}
