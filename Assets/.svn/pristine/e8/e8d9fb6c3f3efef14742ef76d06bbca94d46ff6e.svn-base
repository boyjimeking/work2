Shader "ShadowSoftener" {
Properties {

}

SubShader {
	LOD 300

	CGINCLUDE
	
		#define SHADOW_COLLECTOR_PASS
		#include "UnityCG.cginc"
		
		struct appdata {
			float4 vertex : POSITION;
		};
		
		struct v2f {
			V2F_SHADOW_COLLECTOR;
		};
		
		
		v2f vert (appdata v)
		{
			v2f o;
			TRANSFER_SHADOW_COLLECTOR(o)
			return o;
		}
	
	ENDCG
	
	Pass {
		Name "PCF2X2"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF2x2
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF3X3"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF3x3
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF4X4"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF4x4
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF5X5"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF5x5
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF6X6"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF6x6
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF7X7"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF7x7
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
	Pass {
		Name "PCF8X8"
		Tags { "LightMode" = "ShadowCollector" }
		
		Fog {Mode Off}
		ZWrite On ZTest LEqual

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector
			
			#define SOFTENER_FILTER PCF8x8
			#pragma target 3.0
			#include "Assets/Shadow Softener/Shaders/ShadowSoftener.cginc"
			
			fixed4 frag (v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
		ENDCG
	}
}

}
