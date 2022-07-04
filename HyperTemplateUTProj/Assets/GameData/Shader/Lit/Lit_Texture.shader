// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Kaiyum/Lit/Texture"
{
    Properties
    {
        _LightingAmount("Lighting Amount", Range(1,5)) = 1.99
        _TranslucentColor("Transluent Color", Color) = (1,1,1,1)
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

                kdata _LightingAmount;
                kcolor4 _BaseColor;
                kcolor4 _EmissionColor;
                kcolor4 _TranslucentColor;

                uniform kcolor4 _KLightColor0;
                uniform sampler2D _BaseMap;
                
                kcolor4 _KAMBIENT;

                struct AppData
                {
                    float4 vertex : POSITION;
                    kdata3 normal : NORMAL;
                    kdata4 uv : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    kcolor4 col : TEXCOORD1;
                    kdata4 uv : TEXCOORD0;
                };

                v2f vert(AppData i)
                {
                    v2f output;
                    UNITY_SETUP_INSTANCE_ID(i);
                    kdata3 normalDirection = normalize(mul(kdata4(i.normal, 0.0), unity_WorldToObject).xyz);
                    kdata3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    kdata3 col = _KLightColor0.rgb * _BaseColor.rgb * max(0.0, dot(normalDirection, lightDirection) * _LightingAmount);

                    kdata3 ambientLighting = _TranslucentColor.rgb * _KAMBIENT.rgb;

                    kdata3 posWorld = mul(unity_ObjectToWorld, i.vertex);
                    kdata3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld);
                    kdata silhouetteness = 1.0 - abs(dot(viewDirection, normalDirection));
                    col *= silhouetteness;
                    col += ambientLighting;

                    output.col = kcolor4(col, 1.0);

                    output.pos = UnityObjectToClipPos(i.vertex);
                    output.uv = i.uv;
                    return output;
                }

                kcolor4 frag(v2f i) : COLOR
                {
                    kcolor4 fcol = tex2D(_BaseMap, i.uv);
                    fcol *= i.col;
                    fcol += _EmissionColor;
                    return fcol;
                }
            ENDCG
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}
            ZWrite On
            ColorMask 0

            CGPROGRAM

            //#pragma prefer_hlslcc gles
            //#pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #define kcolor4 fixed4
            #define kdata4 half4
            #define kdata3 half3
            #define kdata2 half2
            #define kdata half

                kdata _LightingAmount;
                kcolor4 _BaseColor;
                kcolor4 _EmissionColor;
                kcolor4 _TranslucentColor;

            struct AppData
            {
                float4 vertex  : POSITION;
            };

            struct v2f
            {
                float4 pos  : SV_POSITION;
            };

            v2f DepthOnlyVertex(AppData i)
            {
                v2f output = (v2f)0;
                output.pos = UnityObjectToClipPos(i.vertex);
                return output;
            }

            half4 DepthOnlyFragment(v2f input) : SV_TARGET
            {
                return 0;
            }
            ENDCG
        }
    }
}