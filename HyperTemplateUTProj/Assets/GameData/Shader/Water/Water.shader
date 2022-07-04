Shader "Kaiyum/Water"
{
	Properties
	{
		_AnimationParams("XY=Direction, Z=Speed", Vector) = (1,1,1,0)

		[HDR]_BaseColor("Deep Color", Color) = (0, 0.44, 0.62, 1)
		[HDR]_ShallowColor("Shallow Color", Color) = (0.1, 0.9, 0.89, 0.02)
		[HDR]_HorizonColor("Horizon Color", Color) = (0.84, 1, 1, 0.15)
		_HorizonDistance("Horizon Distance", Range(0.01 , 32)) = 8
		_Depth("Color Depth", Range(0.01 , 8)) = 1
		_DepthExp("Exponential Depth Blend", Range(0 , 1)) = 1

		[MaterialEnum(Camera Depth,0,Vertex Color,1,Both,2)] _IntersectionSource("Intersection source", Float) = 0
		[MaterialEnum(None,0,Sharp,1,Smooth,2)] _IntersectionStyle("Intersection style", Float) = 1

		[NoScaleOffset][SingleLineTexture]_IntersectionNoise("Intersection noise", 2D) = "white" {}
		_IntersectionColor("Color", Color) = (1,1,1,1)
		_IntersectionLength("Distance", Range(0.01 , 5)) = 2
		_IntersectionClipping("Cutoff", Range(0.01, 1)) = 0.5
		_IntersectionFalloff("Falloff", Range(0.01 , 1)) = 0.5
		_IntersectionTiling("Noise Tiling", float) = 0.2
		_IntersectionSpeed("Speed multiplier", float) = 0.1
		_IntersectionRippleDistance("Intersection Ripple distance", float) = 32
		_IntersectionRippleStrength("Intersection Ripple Strength", Range(0 , 10)) = 0.5
		
		[NoScaleOffset][SingleLineTexture]_FoamTex("Foam Mask", 2D) = "black" {}
		_FoamColor("Color", Color) = (1,1,1,1)
		_FoamSize("Cutoff", Range(0.01 , 0.999)) = 0.01
		_FoamSpeed("Speed multiplier", float) = 0.1
		_FoamWaveMask("Wave mask", Range(0 , 1)) = 0
		_FoamTiling("Tiling", float) = 0.1
		[Toggle(ENABLE_EXAMPLE_FEATURE)] _ExampleFeatureEnabled ("Enable single texture for foam?", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
		LOD 0
		
		Pass
		{	
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma target 2.0
			#pragma shader_feature_local _SHARP_INERSECTION
			#pragma shader_feature_local _SMOOTH_INTERSECTION
			#pragma multi_compile __ ENABLE_EXAMPLE_FEATURE

			#pragma vertex Vertex
			#pragma fragment Pixel
		    
			#include "UnityCG.cginc"
			#include "Includes/Data.cginc"
			#include "Includes/Util.cginc"
			#include "Includes/Vertex.cginc"
			#include "Includes/Pixel.cginc"
			ENDCG
		}	
	}
	Fallback "Hidden/InternalErrorShader"
}