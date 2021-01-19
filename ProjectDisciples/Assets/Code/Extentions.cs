using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Extentions
{
    public static T ReturnRandom<T>(this T[] array)
    {
        int ChosenOne = UnityEngine.Random.Range(0, array.Length * System.DateTime.Now.Millisecond) % array.Length;
        return array[ChosenOne];
    }

    public static T ReturnRandom<T>(this List<T> List)
    {
        int ChosenOne = UnityEngine.Random.Range(0, List.Count * System.DateTime.Now.Millisecond) % List.Count;
        return List[ChosenOne];
    }
}
