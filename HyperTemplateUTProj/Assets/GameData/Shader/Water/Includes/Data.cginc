sampler2D _FoamTex;
float4 _FoamTex_ST;
sampler2D _IntersectionNoise;
float4 _IntersectionNoise_ST;
sampler2D _CameraDepthTexture;

half4 _ShallowColor;
half4 _BaseColor;
half4 _IntersectionColor;
float _Depth;
float _DepthExp;
half4 _HorizonColor;
half _HorizonDistance;
float4 _AnimationParams;

/*Foam*/
float _FoamTiling;
half4 _FoamColor;
float _FoamSpeed;
half _FoamSize;
half _FoamWaveMask;

/*Intersection*/
half _IntersectionSource;
half _IntersectionLength;
half _IntersectionFalloff;
half _IntersectionTiling;
half _IntersectionRippleDistance;
half _IntersectionRippleStrength;
half _IntersectionClipping;
float _IntersectionSpeed;

struct DepthData
{
	float raw;
	float linear01;
	float eye;
};

struct AppData
{
	float4 vert 	: POSITION;
	float4 uv 			: TEXCOORD0;
	float3 normal 	: NORMAL;
};

struct V2f
{
	float4 position 	: SV_POSITION;
	float3 normal 		: NORMAL;
	float4 uv 			: TEXCOORD0;
	float3 wPos 		: TEXCOORD1;
	float4 screenPos 	: TEXCOORD2;
};