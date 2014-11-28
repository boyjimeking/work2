// Updated for Shadow Softener 1.2.2
Shader "Shadow Softener/Samples/Diffuse/Custom Haze Filter" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM

// Use the addshadow flag to generate the required shadow passes
// Optional: Use fullforwardshadows to enable Forward Rendered Spot and Point shadows.
#pragma surface surf Lambert addshadow fullforwardshadows

// Shadow Softener requires Shader Model 3.0
#pragma target 3.0

// Set the Shadow Filter to Custom
#define SOFTENER_FILTER CUSTOM_FILTER
// Define our custom filter
#define SOFTENER_CUSTOM_FILTER(data) CustomFilterHaze(data)

// Include Shadow Softener:
#include "../../ShadowSoftener.cginc"

// Make sure Shadow Softener is good to go
#ifdef SHADOW_SOFTENER

	// Define a custom Shadow Filter
	fixed CustomFilterHaze(float3 shadowCoordDepth)
	{
		// All samples are 1x1 Texels away from center, so we can use a constant for Sample Biasing
		const_if_u4 float offsetLength = length(2.0);
		
		// Take 4 shadow samples in an X pattern... Rotate it for fun!
		// Using SOFTENER_SAMPLE_BIAS(depth, offsetLength) is recommended, but optional
		// The simplest possible sampling would be:
		// float sample = SOFTENER_SAMPLE_SHADOW(shadowCoordDepth)
		float4 samples;
		samples.x = SOFTENER_SAMPLE_SHADOW(float3(shadowCoordDepth.xy + _ShadowMapTexture_TexelSize.xy * float2(_SinTime.w, _CosTime.w), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength)));
		samples.y = SOFTENER_SAMPLE_SHADOW(float3(shadowCoordDepth.xy + _ShadowMapTexture_TexelSize.xy * float2(_CosTime.w, -_SinTime.w), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength)));
		samples.z = SOFTENER_SAMPLE_SHADOW(float3(shadowCoordDepth.xy + _ShadowMapTexture_TexelSize.xy * float2(-_SinTime.w, -_CosTime.w), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength)));
		samples.w = SOFTENER_SAMPLE_SHADOW(float3(shadowCoordDepth.xy + _ShadowMapTexture_TexelSize.xy * float2(-_CosTime.w, _SinTime.w), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength)));
	
		// Check which samples are in light
		// Depending on the number of samples you have, you can use the following Macros:
		// 		float  SOFTENER_1_SAMPLE_LIT(float, float)
		//		float2 SOFTENER_2_SAMPLES_LIT(float2, float2)
		//		float3 SOFTENER_3_SAMPLES_LIT(float3, float3)
		//		float4 SOFTENER_4_SAMPLES_LIT(float4, float4)
		fixed4 inLight = SOFTENER_4_SAMPLES_LIT(samples, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength));
	
		// Return the average
		return dot(inLight, 0.25);
	}
#endif	

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Diffuse" //Just in case we can't support SM 3.0, fall back to the built-in Diffuse...
}
