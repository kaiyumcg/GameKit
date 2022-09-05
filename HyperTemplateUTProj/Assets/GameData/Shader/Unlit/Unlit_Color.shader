// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Kaiyum/Unlit/Color"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        [HDR]_EmissionColor("Emission", Color) = (0,0,0,0)
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma target 2.0
            #include "Lighting.cginc"
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            #pragma vertex vert  
            #pragma fragment frag 
            #pragma multi_compile_instancing
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

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
                    SHADOW_COORDS(0)
                };

                v2f vert(AppData v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    o.pos = UnityObjectToClipPos(v.vertex);
                    TRANSFER_SHADOW(o)
                    return o;
                }

                kcolor4 frag(v2f i) : COLOR
                {
                    kcolor4 fcol = _BaseColor + _EmissionColor;
                    fixed shadow = SHADOW_ATTENUATION(i);

                    fcol *= shadow;
                    return fcol;
                }
            ENDCG
        }

        Pass
        {
            Tags {"LightMode" = "ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}