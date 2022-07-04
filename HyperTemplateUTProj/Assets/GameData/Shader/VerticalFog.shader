Shader "Kaiyum/Vertical Fog"
{
	Properties
	{
	   _Color("Fog Color (When blending is not used)", Color) = (1, 1, 1, .5)
	   [Toggle(VERTICAL_COLOR_BLEND)] _verticalFogBlend ("Enable vertical color blend?", Float) = 0
	   _ColorUp("Fog Down Color (For blending)", Color) = (1, 1, 1, .5)
	   _ColorDown("Fog Up Color (For blending)", Color) = (1, 1, 1, .5)
	   _AddColor("Additive Color", Color) = (0,0,0,0)
	   _IntersectionThresholdMax("Density Parameter", float) = 0.02
	   [Toggle(FOG_DISTANCE_BLEND)] _distancedFog ("Enable Distanced Fog?", Float) = 0
	   [Toggle(FOG_DISTANCE_BLEND_IN_PIXEL_SHADER)] _distancedFogInPixelShader ("Distanced Fog in pixel shader?", Float) = 0
	   _HorizonColor("Horizon Color", Color) = (0, 1, 0, 1)
	   _HorizonDistance("Horizon distance", Float) = 25
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			#pragma shader_feature_local FOG_DISTANCE_BLEND
			#pragma shader_feature_local VERTICAL_COLOR_BLEND
			#pragma shader_feature_local FOG_DISTANCE_BLEND_IN_PIXEL_SHADER

			struct appdata
			{
				float4 vertex : POSITION;
				#if defined(FOG_DISTANCE_BLEND)
					float3 normal : NORMAL;
				#endif
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD0;

				#if defined(FOG_DISTANCE_BLEND)
					#if defined(FOG_DISTANCE_BLEND_IN_PIXEL_SHADER)
						float3 normal : TEXCOORD1;
						float3 positionInWS : TEXCOORD2;
					#else
						float fresnel : TEXCOORD1;
					#endif
				#endif
			};

			sampler2D _CameraDepthTexture;
			fixed4 _AddColor;
			#if defined(VERTICAL_COLOR_BLEND)
				fixed4 _ColorUp, _ColorDown;
			#else
				fixed4 _Color;
			#endif
			float _IntersectionThresholdMax;

			#if defined(FOG_DISTANCE_BLEND)
				float4 _HorizonColor;
				float _HorizonDistance;
			#endif

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);

				#if defined(FOG_DISTANCE_BLEND)
					#if defined(FOG_DISTANCE_BLEND_IN_PIXEL_SHADER)
						o.normal = v.normal;
						o.positionInWS = mul(unity_ObjectToWorld, v.vertex);
					#else
						float3 normalWS = normalize(v.normal.xyz);
						float3 posWS = mul(unity_ObjectToWorld, v.vertex);
						float3 viewDir = (_WorldSpaceCameraPos - posWS);
						float3 viewDirNorm = normalize(viewDir);
						float VdotN = 1.0 - saturate(dot(viewDirNorm, normalWS));
						float fresnel = saturate(pow(VdotN, _HorizonDistance));
						o.fresnel = fresnel;
					#endif
				#endif

				return o;
			}

			float4 frag(v2f i) : SV_TARGET
			{
				float depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
				float diff = saturate(_IntersectionThresholdMax * (depth - i.scrPos.w));
				float a = lerp(0.0, 1.0, diff);
				#if defined(VERTICAL_COLOR_BLEND)
					float4 fcol = lerp(_ColorUp, _ColorDown, a);
				#else
					float4 fcol = _Color;
				#endif

				#if defined(FOG_DISTANCE_BLEND)
					#if defined(FOG_DISTANCE_BLEND_IN_PIXEL_SHADER)
						float3 normalWS = normalize(i.normal);
					    float3 viewDir = (_WorldSpaceCameraPos - i.positionInWS);
						float3 viewDirNorm = normalize(viewDir);
						float VdotN = 1.0 - saturate(dot(viewDirNorm, normalWS));
						float fresnel = saturate(pow(VdotN, _HorizonDistance));
						fcol.rgb = lerp(fcol.rgb, _HorizonColor.rgb, fresnel * _HorizonColor.a);
					#else
						fcol.rgb = lerp(fcol.rgb, _HorizonColor.rgb, i.fresnel * _HorizonColor.a);
					#endif
				#endif

				fcol.rgb += _AddColor.rgb;
				fcol.a = a;
				return fcol;
			}
			ENDCG
		}
	}
}