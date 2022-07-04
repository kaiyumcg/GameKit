#define TIME ((input.uv.z * _AnimationParams.z) * _AnimationParams.xy)

half4 Pixel(V2f input) : SV_Target
{
	half3 finalColor = 0;
	float alpha = 1;

	//Vertex normal in world-space
	float3 normalWS = normalize(input.normal.xyz);
	float3 wPos = input.wPos;
	//Not normalized for depth-pos reconstruction. Normalization required for lighting (otherwise breaks on mobile)
	float3 viewDir = (_WorldSpaceCameraPos - wPos);
	float3 viewDirNorm = normalize(viewDir);
	float2 uv = wPos.xz;
	float4 ScreenPos = input.screenPos;
	float3 opaqueWorldPos = wPos;
	float opaqueDist = 1;
	float aborptionDist = opaqueDist;
	
	DepthData depth = SampleDepth(ScreenPos, wPos);
	opaqueWorldPos = ReconstructViewPos(ScreenPos, viewDir, depth);

	//Invert normal when viewing backfaces
	float normalSign = ceil(dot(viewDirNorm, normalWS));
	normalSign = normalSign == 0 ? -1 : 1;

	opaqueDist = DepthDistance(wPos, opaqueWorldPos, normalWS * normalSign);
	aborptionDist = opaqueDist;

	float AbsorptionDepth = 1;
	AbsorptionDepth = saturate(lerp(aborptionDist / _Depth, 1-(exp(-aborptionDist) / _Depth), _DepthExp));

	float intersection = 0;
	float interSecGradient = 1 - saturate(exp(opaqueDist) / _IntersectionLength);

	if (_IntersectionSource == 1) interSecGradient = 0.0;
	if (_IntersectionSource == 2) interSecGradient = saturate(interSecGradient);

	intersection = SampleIntersection(uv.xy, interSecGradient, TIME * _IntersectionSpeed);
	intersection *= _IntersectionColor.a;

	float foam = 0;
	float foamWaveMask = 1 - _FoamWaveMask;
	foam = SampleFoam(uv * _FoamTiling, TIME, _FoamSize, foamWaveMask);
	foam *= saturate(_FoamColor.a);

	half4 baseColor = lerp(_ShallowColor, _BaseColor, AbsorptionDepth);
	finalColor.rgb = baseColor.rgb;
	alpha = baseColor.a;

	finalColor.rgb = lerp(finalColor.rgb, _FoamColor.rgb, foam);

	alpha = saturate(alpha + intersection + foam);
	half VdotN = 1.0 - saturate(dot(viewDirNorm, normalWS));
	float fresnel = saturate(pow(VdotN, _HorizonDistance));
	finalColor.rgb = lerp(finalColor.rgb, _HorizonColor.rgb, fresnel * 1.5 * _HorizonColor.a);
	half4 color = half4(finalColor.rgb, alpha);
	return color;
}