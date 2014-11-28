// Upgrade NOTE: commented out 'float4 unity_ShadowFadeCenterAndType', a built-in variable

#include "UnityCG.cginc"
struct appdata {
	float4 vertex : POSITION;
	float2 texcoord : TEXCOORD0;
	float3 normal : NORMAL;
};

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 ray : TEXCOORD1;
	
	float4 shadowRayA : TEXCOORD2;
	float4 shadowRayB : TEXCOORD3;
	float4 shadowRayC : TEXCOORD4;
	float4 shadowRayD : TEXCOORD5;
	float4 shadowRayE : TEXCOORD6;
	float4 shadowRayF : TEXCOORD7;
};

sampler2D _CameraDepthTexture;
float4 unity_LightmapFade;

float4x4 _CameraToWorld;
float4x4 unity_World2Shadow[4];
float4 _LightSplitsNear;
float4 _LightSplitsFar;
float4 unity_ShadowSplitSpheres[4];
float4 unity_ShadowSplitSqRadii;
// float4 unity_ShadowFadeCenterAndType;

float4 _LightShadowData;
sampler2D _ShadowMapTexture;

#define SOFTENER_PREPASS
#define SOFTENER_DIRECTIONAL
#include "ShadowSoftenerConfig.cginc"
#include "../../ShadowSoftener.cginc"
#undef UNITY_SAMPLE_SHADOW
#define UNITY_SAMPLE_SHADOW(tex,coord) (_LightShadowData.r + SOFTENER_CUSTOM_FILTER((coord).xyz/(coord).w) * (1-_LightShadowData.r))

v2f vert (appdata v)
{
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	o.ray = v.normal;

	float4 wposNear = mul (_CameraToWorld, float4(0,0,0,1));
	float4 wposFar = mul (_CameraToWorld, float4(o.ray, 1));	
	
	float3 shadowRay = mul (unity_World2Shadow[0], wposNear).xyz;
	o.shadowRayA.xyz = shadowRay;
	o.shadowRayB.xyz = (mul (unity_World2Shadow[0], wposFar).xyz - shadowRay).xyz;
	
	shadowRay = mul (unity_World2Shadow[1], wposNear).xyz;
	o.shadowRayC.xyz = shadowRay.xyz;
	o.shadowRayD.xyz = (mul (unity_World2Shadow[1], wposFar).xyz - shadowRay).xyz;
	
	shadowRay = mul (unity_World2Shadow[2], wposNear).xyz;
	o.shadowRayE.xyz = shadowRay.xyz;
	o.shadowRayF.xyz = (mul (unity_World2Shadow[2], wposFar).xyz - shadowRay).xyz;

	shadowRay = mul (unity_World2Shadow[3], wposNear).xyz;
	o.shadowRayA.w = shadowRay.x;
	o.shadowRayB.w = shadowRay.y;
	o.shadowRayC.w = shadowRay.z;
	shadowRay = mul (unity_World2Shadow[3], wposFar).xyz - shadowRay;
	o.shadowRayD.w = shadowRay.x;
	o.shadowRayE.w = shadowRay.y;
	o.shadowRayF.w = shadowRay.z;
	
	return o;
}

fixed4 frag (v2f i) : COLOR
{
	float depth = UNITY_SAMPLE_DEPTH(tex2D (_CameraDepthTexture, i.uv));
	depth = Linear01Depth (depth);
	float4 vpos = float4(i.ray * depth,1);
	float4 wpos = mul (_CameraToWorld, vpos);	

	float3 sc0 = i.shadowRayA.xyz + i.shadowRayB.xyz * depth;
	float3 sc1 = i.shadowRayC.xyz + i.shadowRayD.xyz * depth;
	float3 sc2 = i.shadowRayE.xyz + i.shadowRayF.xyz * depth;
	float3 sc3 = float3(i.shadowRayA.w, i.shadowRayB.w, i.shadowRayC.w) + float3(i.shadowRayD.w, i.shadowRayE.w, i.shadowRayF.w) * depth;

#if defined (SHADOWS_SPLIT_SPHERES)
	float3 fromCenter0 = wpos.xyz - unity_ShadowSplitSpheres[0].xyz;
	float3 fromCenter1 = wpos.xyz - unity_ShadowSplitSpheres[1].xyz;
	float3 fromCenter2 = wpos.xyz - unity_ShadowSplitSpheres[2].xyz;
	float3 fromCenter3 = wpos.xyz - unity_ShadowSplitSpheres[3].xyz;
	float4 distances2 = float4(dot(fromCenter0,fromCenter0), dot(fromCenter1,fromCenter1), dot(fromCenter2,fromCenter2), dot(fromCenter3,fromCenter3));
	float4 weights = float4(distances2 < unity_ShadowSplitSqRadii);
	weights.yzw = saturate(weights.yzw - weights.xyz);
#else
	float4 zNear = float4( vpos.z >= _LightSplitsNear );
	float4 zFar = float4( vpos.z < _LightSplitsFar );
	float4 weights = zNear * zFar;
#endif
	
	float4 coord = float4(sc0 * weights[0] + sc1 * weights[1] + sc2 * weights[2] + sc3 * weights[3], 1);
	half shadow = UNITY_SAMPLE_SHADOW(_ShadowMapTexture,coord);
	
	float4 res;
	res.x = shadow;
	res.y = 1.0;
	res.zw = EncodeFloatRG (1 - depth);
	return res;	
}

