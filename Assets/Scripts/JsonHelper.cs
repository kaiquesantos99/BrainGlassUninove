using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(FixJson(json));
        return wrapper.Items;
    }

    private static string FixJson(string value)
    {
        return "{\"Items\":" + value + "}";
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
