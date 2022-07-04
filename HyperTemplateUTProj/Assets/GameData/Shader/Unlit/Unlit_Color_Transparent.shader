// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Kaiyum/Unlit/Transparent/Color"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission", Color) = (0,0,0,0)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma target 2.0

            #pragma vertex vert  
            #pragma fragment frag 
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            #define kcolor4 fixed4
            #define kdata4 half4
            #define kdata3 half3
            #define kdata2 half2
            #define kdata half

                kcolor4 _EmissionColor;
                kcolor4 _BaseColor;

                struct AppData
                {
                    float4 vertex : POSITION;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                v2f vert(AppData i)
                {
                    v2f output;
                    UNITY_SETUP_INSTANCE_ID(i);
                    output.pos = UnityObjectToClipPos(i.vertex);
                    return output;
                }

                kcolor4 frag(v2f i) : COLOR
                {
                    return kcolor4(_BaseColor.rgb + _EmissionColor.rgb, _BaseColor.a);
                }
            ENDCG
        }
    }
}