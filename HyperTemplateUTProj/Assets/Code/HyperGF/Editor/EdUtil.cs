using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class EdUtil
{
    public static T TryLoadAsset<T>(string searchName, string typeName = "Material") where T : UnityEngine.Object
    {
        T result = null;
        string[] guids1 = AssetDatabase.FindAssets("t:" + typeName, null);

        foreach (string guid1 in guids1)
        {
            var a_path = AssetDatabase.GUIDToAssetPath(guid1);
            var obj_target = AssetDatabase.LoadAssetAtPath<T>(a_path);
            if (obj_target.name.Contains(searchName))
            {
                result = obj_target;
                break;
            }
        }
        return result;
    }

    public static void GetAllGameobjectsWitMatchingName(ref List<GameObject> lst, string wName)
    {
        lst = new List<GameObject>();
        var all = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        if (all != null && all.Length > 0)
        {
            for (int i = 0; i < all.Length; i++)
            {
                var g = all[i];
                if (g == null) { continue; }
                GetObjects(ref lst, g, wName);
            }
        }

        void GetObjects(ref List<GameObject> _lst, GameObject g, string _wName)
        {
            if (g == null) { return; }
            if (g.name.Contains(_wName) && _lst.Contains(g) == false) { _lst.Add(g); }
            if (g.transform.childCount > 0)
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    var chObj = g.transform.GetChild(i).gameObject;
                    GetObjects(ref _lst, chObj, _wName);
                }
            }
        }
    }

    public static GameObject GetScriptObjectFromScene<T>() where T : MonoBehaviour
    {
        T result = null;
        var all = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        if (all != null && all.Length > 0)
        {
            foreach (var g in all)
            {
                if (g == null) { continue; }
                GetObjects(ref result, g);
            }
        }
        return result == null ? null : result.gameObject;

        void GetObjects(ref T _result, GameObject g)
        {
            if (g == null) { return; }
            if (g.GetComponent<T>() != null)
            {
                _result = g.GetComponent<T>();
                return;
            }
            if (g.transform.childCount > 0)
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    var chObj = g.transform.GetChild(i).gameObject;
                    GetObjects(ref _result, chObj);
                }
            }
        }
    }

    public static void DestroyAllImmediateChildsOf(GameObject obj)
    {
        if (obj == null) { return; }
        var lst = new List<GameObject>();

        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                lst.Add(obj.transform.GetChild(i).gameObject);
            }
        }

        foreach (var g in lst)
        {
            UnityEngine.Object.DestroyImmediate(g);
        }
    }

    public static GameObject GetImmediateChildWithName(GameObject obj, string wName)
    {
        GameObject result = null;
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (obj.transform.GetChild(i).gameObject.name.Contains(wName))
                {
                    result = obj.transform.GetChild(i).gameObject;
                    break;
                }
            }
        }
        return result;
    }

    public static void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    public enum SpecialFieldType { No = 0, IsLayer = 1, IsPassword = 2, IsTag = 3}

    public static void DrawField<T>(ref bool useFeature, string useFeatureTxt, ref T field,
        bool overrideFieldName = false, string fieldName = "",
        SpecialFieldType spType = SpecialFieldType.No)
    {
        useFeature = EditorGUILayout.Toggle(useFeatureTxt, useFeature);
        if (useFeature)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            var fName = overrideFieldName ? fieldName : nameof(field);
            EditorGUILayout.LabelField(fName);
            if (field.GetType() == typeof(int) && spType == SpecialFieldType.No)
            {
                field = (T)(object)EditorGUILayout.IntField("", (int)(object)field);
            }
            else if (field.GetType() == typeof(bool))
            {
                field = (T)(object)EditorGUILayout.Toggle("", (bool)(object)field);
            }
            else if (field.GetType() == typeof(string) && spType == SpecialFieldType.No)
            {
                field = (T)(object)EditorGUILayout.TextField("", (string)(object)field);
            }
            else if (field.GetType() == typeof(float))
            {
                field = (T)(object)EditorGUILayout.FloatField("", (float)(object)field);
            }
            else if (field.GetType() == typeof(Color))
            {
                field = (T)(object)EditorGUILayout.ColorField("", (Color)(object)field);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(field.GetType()))
            {
                field = (T)(object)EditorGUILayout.ObjectField((UnityEngine.Object)(object)field, typeof(T), true);
            }
            else if (field.GetType() == typeof(Bounds))
            {
                field = (T)(object)EditorGUILayout.BoundsField("", (Bounds)(object)field);
            }
            else if (field.GetType() == typeof(BoundsInt))
            {
                field = (T)(object)EditorGUILayout.BoundsIntField("", (BoundsInt)(object)field);
            }
            else if (field.GetType() == typeof(AnimationCurve))
            {
                field = (T)(object)EditorGUILayout.CurveField("", (AnimationCurve)(object)field);
            }
            else if (field.GetType() == typeof(double))
            {
                field = (T)(object)EditorGUILayout.DoubleField("", (double)(object)field);
            }
            else if (field.GetType() == typeof(long))
            {
                field = (T)(object)EditorGUILayout.LongField("", (long)(object)field);
            }
            else if (field.GetType() == typeof(Gradient))
            {
                field = (T)(object)EditorGUILayout.GradientField("", (Gradient)(object)field);
            }
            else if (field.GetType() == typeof(int) && spType == SpecialFieldType.IsLayer)
            {
                field = (T)(object)EditorGUILayout.LayerField("", (int)(object)field);
            }
            else if (field.GetType() == typeof(string) && spType == SpecialFieldType.IsPassword)
            {
                field = (T)(object)EditorGUILayout.PasswordField("", (string)(object)field);
            }
            else if (field.GetType() == typeof(string) && spType == SpecialFieldType.IsTag)
            {
                field = (T)(object)EditorGUILayout.TagField("", (string)(object)field);
            }
            else if (field.GetType() == typeof(Rect))
            {
                field = (T)(object)EditorGUILayout.RectField("", (Rect)(object)field);
            }
            else if (field.GetType() == typeof(RectInt))
            {
                field = (T)(object)EditorGUILayout.RectIntField("", (RectInt)(object)field);
            }
            else if (field.GetType() == typeof(Vector2))
            {
                field = (T)(object)EditorGUILayout.Vector2Field("", (Vector2)(object)field);
            }
            else if (field.GetType() == typeof(Vector2Int))
            {
                field = (T)(object)EditorGUILayout.Vector2IntField("", (Vector2Int)(object)field);
            }
            else if (field.GetType() == typeof(Vector3))
            {
                field = (T)(object)EditorGUILayout.Vector3Field("", (Vector3)(object)field);
            }
            else if (field.GetType() == typeof(Vector3Int))
            {
                field = (T)(object)EditorGUILayout.Vector3IntField("", (Vector3Int)(object)field);
            }
            else if (field.GetType() == typeof(Vector4))
            {
                field = (T)(object)EditorGUILayout.Vector4Field("", (Vector4)(object)field);
            }
            else if (field.GetType() == typeof(System.Enum))
            {
                field = (T)(object)EditorGUILayout.EnumPopup("", (System.Enum)(object)field);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
    }
}
