V2f Vertex(AppData v)
{
	V2f output = (V2f)0;
	output.uv.xy = v.uv.xy;
	output.uv.z = _Time.x;
	output.uv.w = 0;
	float3 positionWS = mul(unity_ObjectToWorld, v.vert.xyz);
	output.position = UnityObjectToClipPos(v.vert);
	output.screenPos = ComputeScreenPos(output.position);
	output.normal = UnityObjectToWorldNormal(v.normal);
	output.wPos = positionWS.xyz;
	return output;
}