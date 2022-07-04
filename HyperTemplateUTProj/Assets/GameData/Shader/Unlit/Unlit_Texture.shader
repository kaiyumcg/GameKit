// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Kaiyum/Unlit/Texture"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission", Color) = (0,0,0,0)
        _BaseMap ("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
        LOD 100

        Pass
        {
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

                uniform sampler2D _BaseMap;

                struct AppData
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 uv : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    kdata4 uv : TEXCOORD0;
                };

                v2f vert(AppData i)
                {
                    v2f output;
                    UNITY_SETUP_INSTANCE_ID(i);
                    output.pos = UnityObjectToClipPos(i.vertex);
                    output.uv = i.uv;
                    return output;
                }

                kcolor4 frag(v2f i) : COLOR
                {
                    kcolor4 fcol = tex2D(_BaseMap, i.uv) * _BaseColor;
                    fcol += _EmissionColor;
                    return fcol;
                }
            ENDCG
        }
    }
}