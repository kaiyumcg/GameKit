// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Kaiyum/Path"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        _AdditiveColor("Additive Color", Color) = (0,0,0,0)
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

                kcolor4 _BaseColor;
                kcolor4 _AdditiveColor;
                
                kcolor4 _KAMBIENT;
                kcolor4 _KLightColor0;

                struct AppData
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    kcolor4 col : TEXCOORD0;
                };

                v2f vert(AppData i)
                {
                    v2f output;
                    UNITY_SETUP_INSTANCE_ID(i);
                    kdata3 normalDirection = normalize(mul(kdata4(i.normal, 0.0), unity_WorldToObject).xyz);
                    kdata3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

                    //START
                    kdata3 lambertLight = saturate(dot(normalDirection, lightDirection)) * _KLightColor0.rgb;
                    lambertLight += _AdditiveColor.rgb;
                    kdata3 oneMinus = kdata3(1.0, 1.0, 1.0) - lambertLight;
                    kdata3 result = (lambertLight * lambertLight + oneMinus) * _BaseColor.rgb;
                    result += _KAMBIENT.rgb;
                    //END

                    output.col = kcolor4(result, 1.0);
                    output.pos = UnityObjectToClipPos(i.vertex);
                    return output;
                }

                kcolor4 frag(v2f i) : COLOR
                {
                    return i.col;
                }
            ENDCG
        }
    }
}