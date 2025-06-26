using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NVC
{
    public static class Helper
    {
        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                int j = Random.Range(i, n); // Random index from i to n-1
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}

