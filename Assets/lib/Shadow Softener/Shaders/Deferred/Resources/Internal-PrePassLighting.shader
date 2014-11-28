// Updated for Shadow Softener 1.2.2
Shader "Hidden/Internal-PrePassLighting" {
Properties {
	_LightTexture0 ("", any) = "" {}
	_LightTextureB0 ("", 2D) = "" {}
	_ShadowMapTexture ("", any) = "" {}
}
SubShader {

CGINCLUDE

#if defined(CBUFFER_START)
	#include "Internal-PrePassLighting-40.cginc"
#else
	#include "Internal-PrePassLighting-35.cginc"
#endif

ENDCG

/*Pass 1: LDR Pass - Lighting encoded into a subtractive ARGB8 buffer*/
Pass {
	ZWrite Off Fog { Mode Off }
	Blend DstColor Zero
	
CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers noprepass
#pragma glsl_no_auto_normalization
#pragma multi_compile_lightpass
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

fixed4 frag (v2f i) : COLOR
{
	return exp2(-CalculateLight(i));
}

ENDCG
}

/*Pass 2: HDR Pass - Lighting additively blended into floating point buffer*/
Pass {
	ZWrite Off Fog { Mode Off }
	Blend One One
	
CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers noprepass
#pragma glsl_no_auto_normalization
#pragma multi_compile_lightpass
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

fixed4 frag (v2f i) : COLOR
{
	return CalculateLight(i);
}

ENDCG
}

/*Pass 3: Xenon HDR Specular Pass - 10-10-10-2 buffer means we need seperate specular buffer*/
Pass {
	ZWrite Off Fog { Mode Off }
	Blend One One
	
CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers noprepass
#pragma glsl_no_auto_normalization
#pragma multi_compile_lightpass
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

fixed4 frag (v2f i) : COLOR
{
	return CalculateLight(i).argb;
}

ENDCG
}

}
Fallback Off
}
