// Updated for Shadow Softener 1.2.2
Shader "Shadow Softener/Samples/Diffuse/PCF8x8 (Full Forward)" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM

// Use addshadow when not using UsePass "ShadowSoftener/..."
// Use fullforwardshadows to enable Forward Rendered Spot and Point shadows.
#pragma surface surf Lambert addshadow fullforwardshadows

// Shadow Softener requires Shader Model 3.0
#pragma target 3.0

// Define the Shadow Filter you wish to use:
#define SOFTENER_FILTER PCF8x8

// Include Shadow Softener:
#include "../../ShadowSoftener.cginc"

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
