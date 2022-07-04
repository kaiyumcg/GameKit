float DepthDistance(float3 wPos, float3 viewPos, float3 normal)
{
	return length((wPos - viewPos) * normal);
}

float4 PackedUV(float2 sourceUV, float2 time, float speed)
{
	float2 uv1 = sourceUV.xy + (time.xy * speed);
	float2 uv2 = (sourceUV.xy * 0.5) + ((1 - time.xy) * speed * 0.5);
	return float4(uv1.xy, uv2.xy);
}

//Depth routine
DepthData SampleDepth(float4 screenPos, float3 wPos)
{
	DepthData depth = (DepthData)0;
	depth.raw = 0; //todo
	depth.eye = 0; //todo
	depth.linear01 = 1; //todo
	return depth;
}

//Remake view-space position from depth value
float3 ReconstructViewPos(float4 screenPos, float3 viewDir, DepthData depth)
{
#if defined(ORTHOGRAPHIC_SUPPORT)
	//View to world position
	float4 viewPos = float4((screenPos.xy / screenPos.w) * 2.0 - 1.0, depth.raw, 1.0);
	float4x4 viewToWorld = UNITY_MATRIX_I_VP;
	float4 viewWorld = mul(viewToWorld, viewPos);
	float3 viewWorldPos = viewWorld.xyz / viewWorld.w;
#endif

	//Projection to world position
	float3 camPos = _WorldSpaceCameraPos.xyz;
	float3 worldPos = depth.eye * (viewDir / screenPos.w) - camPos;
	float3 perspWorldPos = -worldPos;
	return perspWorldPos;
}

float SampleIntersection(float2 uv, float gradient, float2 time)
{
	float inter = 0;
#if _SHARP_INERSECTION
	float sine = sin(time.y * 10 - (gradient * _IntersectionRippleDistance)) * _IntersectionRippleStrength;
	float2 nUV = float2(uv.x, uv.y) * _IntersectionTiling;
	float2 _uv = nUV + time.xy;
	float noise = tex2D(_IntersectionNoise, _IntersectionNoise_ST.xy * _uv + _IntersectionNoise_ST.zw).r;
	float dist = saturate(gradient / _IntersectionFalloff);
	noise = saturate((noise + sine) * dist + dist);
	inter = step(_IntersectionClipping, noise);
#endif

#if _SMOOTH_INTERSECTION
	float2 uv1 = (float2(uv.x, uv.y) * _IntersectionTiling) + (time.xy);
	float2 uv2 = (float2(uv.x, uv.y) * (_IntersectionTiling * 1.5)) - (time.xy);
	float noise1 = tex2D(_IntersectionNoise, _IntersectionNoise_ST.xy * uv1 + _IntersectionNoise_ST.zw).r;
	float noise2 = tex2D(_IntersectionNoise, _IntersectionNoise_ST.xy * uv2 + _IntersectionNoise_ST.zw).r;
	float dist2 = saturate(gradient / _IntersectionFalloff);
	inter = saturate(noise1 + noise2 + dist2) * dist2;
#endif
	return saturate(inter);
}

float SampleFoam(float2 uv, float2 time, float clipping, float mask)
{
	float4 uvs = PackedUV(uv, time, _FoamSpeed);
	float f1 = tex2D(_FoamTex, _FoamTex_ST.xy * uvs.xy + _FoamTex_ST.zw).r;
#if !defined(ENABLE_EXAMPLE_FEATURE)
	float f2 = tex2D(_FoamTex, _FoamTex_ST.xy * uvs.zw + _FoamTex_ST.zw).r;
#endif

#if defined(ENABLE_EXAMPLE_FEATURE)
	float foam = saturate(f1) * mask;
#else
	float foam = saturate(f1 + f2) * mask;
#endif
	foam = smoothstep(clipping, 1, foam);
	return foam;
}