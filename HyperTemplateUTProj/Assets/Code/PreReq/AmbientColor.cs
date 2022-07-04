using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientColor : MonoBehaviour
{
    [SerializeField] Color m_ambient = new Color(0.7176471f, 0.705882f, 0.705882f, 1);
    private void Awake()
    {
        Shader.SetGlobalColor("_KAMBIENT", m_ambient);
        var lit = GetComponent<Light>();
        if (lit != null)
        {
            Shader.SetGlobalColor("_KLightColor0", lit.color * lit.intensity);
        }
    }

    private void OnValidate()
    {
        Shader.SetGlobalColor("_KAMBIENT", m_ambient);
        var lit = GetComponent<Light>();
        if (lit != null)
        {
            Shader.SetGlobalColor("_KLightColor0", lit.color * lit.intensity);
        }
    }
}