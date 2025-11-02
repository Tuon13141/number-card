using System.Collections.Generic;
using UnityEngine;

public static class MatrixHelper 
{
    public static List<Vector2Int> GetOverlappingCells(
      int wA, int hA, int wB, int hB,
      Vector2Int cellA)
    {
        var result = new List<Vector2Int>();

        Vector2 centerA = new Vector2((wA - 1) / 2f, (hA - 1) / 2f);
        Vector2 centerB = new Vector2((wB - 1) / 2f, (hB - 1) / 2f);

        Vector2 worldPosA = new Vector2(cellA.x - centerA.x, cellA.y - centerA.y);

        float axMin = worldPosA.x - 0.5f;
        float axMax = worldPosA.x + 0.5f;
        float ayMin = worldPosA.y - 0.5f;
        float ayMax = worldPosA.y + 0.5f;

        for (int yB = 0; yB < hB; yB++)
        {
            for (int xB = 0; xB < wB; xB++)
            {
                Vector2 worldPosB = new Vector2(xB - centerB.x, yB - centerB.y);

                float bxMin = worldPosB.x - 0.5f;
                float bxMax = worldPosB.x + 0.5f;
                float byMin = worldPosB.y - 0.5f;
                float byMax = worldPosB.y + 0.5f;

                bool overlapX = bxMax > axMin && bxMin < axMax;
                bool overlapY = byMax > ayMin && byMin < ayMax;

                if (overlapX && overlapY)
                    result.Add(new Vector2Int(xB, yB));
            }
        }

        return result;
    }
}
