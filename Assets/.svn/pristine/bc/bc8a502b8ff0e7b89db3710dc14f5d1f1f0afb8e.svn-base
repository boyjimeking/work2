Shader "BehindWall2" { 
  
 Properties { 
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "grey" {} 
 _Color ("Color",Color) = (1,1,1,1) 
 _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1) 
 _TH ("Range TH",Range (0, 1))  = 0 
 } 
  
 CGINCLUDE 
  
 #include "UnityCG.cginc" 
  
 uniform float4x4 _CameraToWorld; 


  
 half3 VertexLightsWorldSpace (half3 WP, half3 WN) 
 { 
 half3 lightColor = half3(0.0,0.0,0.0); 


 // preface & optimization 
 //light 0 
 half3 toLight0 = mul(_CameraToWorld, unity_LightPosition[0] * half4(1,1,-1,1)).xyz - WP; 
  
 half lengthSq = dot(toLight0, toLight0); 


 half atten = 1.0 + lengthSq * unity_LightAtten[0].z; 
  
 atten = 1.0 / atten; 
  


 // light #0 
 half diff = saturate (dot (WN, normalize(toLight0))); 
 lightColor += unity_LightColor[0].rgb * (diff * atten); 


 return lightColor; 
 } 
  
 ENDCG 
  
  
 SubShader { 
 LOD 190 
 Lighting on 
 Tags { "RenderType"="Opaque" "Reflection" = "RenderReflectionOpaque" "Queue"="Geometry+1" } 
  
 Pass 
        { 
            Cull back 
			Lighting Off 
			ZTest Greater 
			ZWrite Off 
			Blend One OneMinusSrcAlpha 
			Name "BASE" 
			BindChannels 
			{ 
			Bind "Vertex", vertex 
			Bind "texcoord", texcoord0 
			Bind "normal", normal 
			} 
            CGPROGRAM 
                #pragma vertex vert 
                #pragma fragment frag 
               
                #pragma fragmentoption ARB_fog_exp2 
                #pragma fragmentoption ARB_precision_hint_fastest 
               
                #include "UnityCG.cginc" 
               
                uniform sampler2D _MainTex; 
                uniform float4 _MainTex_ST; 
                uniform float4 _Color; 
                uniform float4 _AtmoColor; 
				uniform half _TH; 
               
                struct v2f 
                { 
                    float4 pos : SV_POSITION; 
                    float3 normal : TEXCOORD0; 
                    float3 worldvertpos : TEXCOORD1; 
                    float2 texcoord : TEXCOORD2; 
					 float amount : TEXCOORD3; 
                }; 


                v2f vert(appdata_base v) 
                { 
                    v2f o; 
                   
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex); 
					 //o.pos.z -= 0.03; 
                    o.normal = mul (UNITY_MATRIX_MV, float4(v.normal,0));//mul (UNITY_MATRIX_MVP,v.normal.xyzz).xyz; 
					o.normal = normalize(o.normal); 
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex); 
                    o.amount =  saturate((o.normal.x*o.normal.x + o.normal.y*o.normal.y - o.normal.z*o.normal.z)); 
					//o.amount *= o.amount; 
                    return o; 
                } 
               
                float4 frag(v2f i) : COLOR 
                { 


                    //float4 color = tex2D(_MainTex, i.texcoord) * _Color; 
                    float4 color = lerp(float4(0,0,0,0), _AtmoColor, i.amount); 
               
                    return color; 
                } 
            ENDCG 
        } //end pass 
  
 Pass { 
		 Zwrite On 
		 Blend One OneMinusSrcAlpha 
		 CGPROGRAM 
  
		 #pragma vertex vert 
		 #pragma fragment frag 
		 #pragma fragmentoption ARB_precision_hint_fastest 
  
		 uniform half4 _MainTex_ST; 
		 uniform sampler2D _MainTex; 
		 uniform fixed4 _Color; 


		 struct v2f 
		 { 
		 half4 pos : POSITION; 
		 half3 color : TEXCOORD0; 
		 half2 uv : TEXCOORD1; 
		 }; 
  
		 v2f vert (appdata_base v) 
		 { 
		 v2f o; 
		 o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
  
		 half3 worldPos = mul(_Object2World, v.vertex).xyz; 
		 half3 worldNormal = mul((half3x3)_Object2World, v.normal.xyz); 
  
		 o.color = VertexLightsWorldSpace(worldPos, worldNormal); 
  
		 o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex); 




		 return o; 
		 } 
  
		 fixed4 frag (v2f i) : COLOR 
		 { 
		 fixed4 tex = tex2D(_MainTex, i.uv.xy); 
		 half4 outColor = tex; 
		 outColor.rgb += i.color; 
		 return _Color*outColor*2.0; 
		 } 
  
 ENDCG 
 } 
  
 } 
}