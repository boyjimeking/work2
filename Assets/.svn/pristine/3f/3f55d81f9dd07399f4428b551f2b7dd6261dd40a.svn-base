#ifdef CBUFFER_START
	#define UNITY4_PLUS
	#define const_if_u4 const
#else
	#define const_if_u4
#endif

//#define SOFTENER_SAMPLE_BIAS_OFF

// Detect Light Type
#if defined(SHADOW_COLLECTOR_PASS) || (defined(DIRECTIONAL) && !defined(SHADOWS_OFF))
	#undef SOFTENER_DIRECTIONAL
	#undef SOFTENER_SPOT
	#undef SOFTENER_POINT
	#define SOFTENER_DIRECTIONAL
#elif defined(SPOT) && defined(SHADOWS_DEPTH)
	#undef SOFTENER_DIRECTIONAL
	#undef SOFTENER_SPOT
	#undef SOFTENER_POINT
	#define SOFTENER_SPOT
#elif (defined(POINT) || defined(POINT_COOKIE)) && defined(SHADOWS_CUBE)
	#undef SOFTENER_DIRECTIONAL
	#undef SOFTENER_SPOT
	#undef SOFTENER_POINT
	#define SOFTENER_POINT
#endif

#if defined(SOFTENER_PREPASS) || defined(SOFTENER_DIRECTIONAL) || defined(SOFTENER_SPOT) || defined(SOFTENER_POINT)
	#define SHADOW_SOFTENER
	
	uniform float4 _ShadowMapTexture_TexelSize;

	// Configure Bias ON/OFF
	#if !defined(SOFTENER_SAMPLE_BIAS_OFF)
		#undef SOFTENER_SAMPLE_BIAS_ON
		#define SOFTENER_SAMPLE_BIAS_ON
	#endif
	#if !defined(SOFTENER_SPOT_BIAS_SCALE)
		#define SOFTENER_SPOT_BIAS_SCALE 0.01
	#endif
	#if !defined(SOFTENER_POINT_BIAS_BASE)
		#define SOFTENER_POINT_BIAS_BASE 0.03
	#endif
	#if !defined(SOFTENER_POINT_BIAS_SCALE)
		#define SOFTENER_POINT_BIAS_SCALE 0.01
	#endif
	#if !defined(SOFTENER_POINT_KERNEL)
		#define SOFTENER_POINT_KERNEL 1.0
	#endif
	
	// Helper Macros
	#define CenterShadowTexel(coord) (floor(coord * _ShadowMapTexture_TexelSize.zw) * _ShadowMapTexture_TexelSize.xy + _ShadowMapTexture_TexelSize.xy * 0.5)
	
	#if defined(SOFTENER_POINT)
		// Replace Point Filtering...
		#define SOFTENER_SAMPLE_SHADOW(vec) SampleCubeDistance(vec)
	#else
		#if !defined(UNITY4_PLUS) && defined (SHADOWS_NATIVE) && !defined (SHADER_API_OPENGL)
			#define SOFTENER_SAMPLE_SHADOW(coordDepth) (tex2D(_ShadowMapTexture, (coordDepth)).r)
		#elif defined(UNITY4_PLUS) && defined (SHADOWS_NATIVE)
			#define SOFTENER_SAMPLE_SHADOW(coordDepth) UNITY_SAMPLE_SHADOW(_ShadowMapTexture, (coordDepth))
		#else
			#define SOFTENER_SAMPLE_SHADOW(coordDepth) UNITY_SAMPLE_DEPTH(tex2D( _ShadowMapTexture, (coordDepth).xy))
		#endif
	#endif
	#if defined(SOFTENER_POINT)
		#define SOFTENER_SAMPLES_LIT(samples, depth, type) (samples < (type)depth ? (type)_LightShadowData.r : 1.0f)
	#else
		#if defined (SHADOWS_NATIVE) && !defined(SOFTENER_POINT)
			#define SOFTENER_SAMPLES_LIT(samples, depth, type) step((type)0.5, samples)
		#else
			#define SOFTENER_SAMPLES_LIT(samples, depth, type) step((type)(depth), samples)
		#endif
	#endif
	#define SOFTENER_1_SAMPLE_LIT(samples, depth) SOFTENER_SAMPLES_LIT(samples, depth, float)
	#define SOFTENER_2_SAMPLES_LIT(samples, depth) SOFTENER_SAMPLES_LIT(samples, depth, float2)
	#define SOFTENER_3_SAMPLES_LIT(samples, depth) SOFTENER_SAMPLES_LIT(samples, depth, float3)
	#define SOFTENER_4_SAMPLES_LIT(samples, depth) SOFTENER_SAMPLES_LIT(samples, depth, float4)
	
	#ifdef SOFTENER_SAMPLE_BIAS_ON
		#if defined(SOFTENER_DIRECTIONAL)
			#define SOFTENER_BIAS_TERM unity_LightShadowBias.x
		#elif defined(SOFTENER_SPOT)
			#define SOFTENER_BIAS_TERM unity_LightShadowBias.x * SOFTENER_SPOT_BIAS_SCALE
		#else
			#define SOFTENER_BIAS_TERM 0.0
		#endif
		#if defined(SOFTENER_POINT)
			#define SOFTENER_SAMPLE_BIAS(depth, offsetLength) ((depth) * ((1.0 - SOFTENER_POINT_BIAS_BASE) - offsetLength * SOFTENER_POINT_BIAS_SCALE))
		#else
			#define SOFTENER_SAMPLE_BIAS(depth, offsetLength) ((depth) - (offsetLength) * SOFTENER_BIAS_TERM)
		#endif
	#else
		#if defined(SOFTENER_POINT)
			#define SOFTENER_SAMPLE_BIAS(depth, offsetLength) ((depth) * (1.0 - SOFTENER_POINT_BIAS_BASE))
		#else
			#define SOFTENER_SAMPLE_BIAS(depth, offsetLength) (depth)
		#endif
	#endif
	
	
	#define NO_FILTER 0
	#define CUSTOM_FILTER 1
	#define PCF2x2 2
	#define PCF3x3 3
	#define PCF4x4 4
	#define PCF5x5 5
	#define PCF6x6 6
	#define PCF7x7 7
	#define PCF8x8 8
	
	#define PCF4 104
	#define PCF8 108
	#define PCF12 112
	#define PCF16 116
	#define PCF20 120
	#define PCF24 124
	#define PCF28 128
	#define PCF32 132
	#define PCF36 136
	#define PCF40 140
	#define PCF44 144
	#define PCF48 148
	#define PCF52 152
	#define PCF56 156
	#define PCF60 160
	#define PCF64 164	
	
	#if !defined(SOFTENER_FILTER)
		#define SOFTENER_FILTER NO_FILTER
	#endif
	#if !defined(SOFTENER_FILTER_DIRECTIONAL)
		#define SOFTENER_FILTER_DIRECTIONAL SOFTENER_FILTER
	#endif
	#if !defined(SOFTENER_FILTER_SPOT)
		#define SOFTENER_FILTER_SPOT SOFTENER_FILTER
	#endif
	#if !defined(SOFTENER_FILTER_POINT)
		#define SOFTENER_FILTER_POINT SOFTENER_FILTER
	#endif

	//Custom Point Filters are not currently supported:
	#if defined(SOFTENER_POINT) && SOFTENER_FILTER_POINT == CUSTOM_FILTER && !defined(SOFTENER_PREPASS)
		#undef SOFTENER_CUSTOM_FILTER
		#undef SOFTENER_FILTER_POINT
		#define SOFTENER_FILTER_POINT PCF32
	#endif
	
	#if !defined(SOFTENER_CUSTOM_FILTER)
		#if defined(SOFTENER_POINT)
			#define SOFTENER_LIGHTX_FILTER SOFTENER_FILTER_POINT
			#if SOFTENER_LIGHTX_FILTER == PCF4 || SOFTENER_LIGHTX_FILTER == PCF2x2
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 1)
			#elif SOFTENER_LIGHTX_FILTER == PCF8 || SOFTENER_LIGHTX_FILTER == PCF3x3
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 2)
			#elif SOFTENER_LIGHTX_FILTER == PCF12
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 3)
			#elif SOFTENER_LIGHTX_FILTER == PCF16 || SOFTENER_LIGHTX_FILTER == PCF4x4
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 4)
			#elif SOFTENER_LIGHTX_FILTER == PCF20
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 5)
			#elif SOFTENER_LIGHTX_FILTER == PCF24 || SOFTENER_LIGHTX_FILTER == PCF5x5
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 6)
			#elif SOFTENER_LIGHTX_FILTER == PCF28
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 7)
			#elif SOFTENER_LIGHTX_FILTER == PCF32
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 8)
			#elif SOFTENER_LIGHTX_FILTER == PCF36 || SOFTENER_LIGHTX_FILTER == PCF6x6
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 9)
			#elif SOFTENER_LIGHTX_FILTER == PCF40
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 10)
			#elif SOFTENER_LIGHTX_FILTER == PCF44
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 11)
			#elif SOFTENER_LIGHTX_FILTER == PCF48 || SOFTENER_LIGHTX_FILTER == PCF7x7
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 12)
			#elif SOFTENER_LIGHTX_FILTER == PCF52
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 13)
			#elif SOFTENER_LIGHTX_FILTER == PCF56
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 14)
			#elif SOFTENER_LIGHTX_FILTER == PCF60
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 15)
			#elif SOFTENER_LIGHTX_FILTER == PCF64 || SOFTENER_LIGHTX_FILTER == PCF8x8
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 16)
			#else
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterCube(data, 4)
			#endif
		#else
			#if defined(SOFTENER_DIRECTIONAL)
				#define SOFTENER_LIGHTX_FILTER SOFTENER_FILTER_DIRECTIONAL
			#elif defined(SOFTENER_SPOT)
				#define SOFTENER_LIGHTX_FILTER SOFTENER_FILTER_SPOT
			#endif
			#if SOFTENER_LIGHTX_FILTER == PCF2x2
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCF2x2(data)
			#elif SOFTENER_LIGHTX_FILTER == PCF3x3
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCF3x3(data)
			#elif SOFTENER_LIGHTX_FILTER == PCF4x4
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCF4x4(data)
			#elif SOFTENER_LIGHTX_FILTER == PCF5x5
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCFx(data, 5)
			#elif SOFTENER_LIGHTX_FILTER == PCF6x6
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCFx(data, 6)
			#elif SOFTENER_LIGHTX_FILTER == PCF7x7
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCFx(data, 7)
			#elif SOFTENER_LIGHTX_FILTER == PCF8x8
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCFx(data, 8)
			#else
				#define SOFTENER_CUSTOM_FILTER(data) ShadowFilterPCF4x4(data)
			#endif
		#endif
	#endif
	
	//Surface Shader Haxies:
	#if !defined(SOFTENER_PREPASS)
		#if defined(SOFTENER_DIRECTIONAL) && SOFTENER_FILTER_DIRECTIONAL != NO_FILTER
			#if defined(SHADOW_COLLECTOR_PASS)
				#undef SAMPLE_SHADOW_COLLECTOR_SHADOW
				#define SAMPLE_SHADOW_COLLECTOR_SHADOW(coord) float shadow = _LightShadowData.r + SOFTENER_CUSTOM_FILTER(coord.xyz) * (1-_LightShadowData.r);
			#elif (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
				#undef unitySampleShadow
				#define unitySampleShadow(coord) SOFTENER_CUSTOM_FILTER((coord).xyz/(coord).w)
			#endif
		#elif defined(SOFTENER_SPOT) && SOFTENER_FILTER_SPOT != NO_FILTER
			#undef unitySampleShadow
			#define unitySampleShadow(coord) (_LightShadowData.r + SOFTENER_CUSTOM_FILTER((coord).xyz/(coord).w) * (1-_LightShadowData.r))
		#elif defined(SOFTENER_POINT) && SOFTENER_FILTER_POINT != NO_FILTER
			#undef unityCubeShadow
			#define unityCubeShadow(coord) (_LightShadowData.r + SOFTENER_CUSTOM_FILTER(float4(coord, length(coord) * _LightPositionRange.w)) * (1-_LightShadowData.r))
		#endif
	#endif	
	
	
	
	fixed ShadowFilterPCF2x2(float3 shadowCoordDepth)
	{
		//2x2 isn't centered nicely :)
		shadowCoordDepth.xy -= _ShadowMapTexture_TexelSize.xy * 0.5;
		
		//Read from the center of texels to avoid floating point error when reading neighbors...
		float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);
		
		float4 samples;
		samples.x = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord, shadowCoordDepth.z));
		samples.y = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(1, 0), shadowCoordDepth.z));
		samples.z = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(0, 1), shadowCoordDepth.z));
		samples.w = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy, shadowCoordDepth.z));
	
		fixed4 inLight = SOFTENER_4_SAMPLES_LIT(samples, shadowCoordDepth.z);
	
		fixed4 vLerps = frac(_ShadowMapTexture_TexelSize.zw * shadowCoordDepth.xy).xyxy;
		vLerps.zw = 1.0 - vLerps.zw;
	
		return dot(inLight, vLerps.zxzx*vLerps.wwyy);
	}

	fixed ShadowFilterPCF3x3(float3 shadowCoordDepth)
	{
		// Edge tap smoothing
		fixed4 fracWeights = frac(shadowCoordDepth.xy * _ShadowMapTexture_TexelSize.zw).xyxy;
		fracWeights.zw = 1.0-fracWeights.xy;
	
		//Read from the center of texels to avoid floating point error when reading neighbors...
		float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);
	
		fixed3 shadowTerm;
		for (int y = 0; y < 3; y++)
		{
			const float offsetCenter = 0.0, offsetEdge = 1.0, offsetCorner = 1.414213562373095; // 0x0, 0x1, 1x1 - 0.5
			float3 offsetLengths;
			if(y==0 || y==2)	offsetLengths = float3(offsetCorner, offsetEdge, offsetCorner);
			else				offsetLengths = float3(offsetEdge, offsetCenter, offsetEdge);
			
			float3 samples;
			samples.x = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(-1, y - 1), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.x)));
			samples.y = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2( 0, y - 1), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.y)));
			samples.z = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2( 1, y - 1), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.z)));
	
			fixed3 inLight = SOFTENER_3_SAMPLES_LIT(samples, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.zzz, offsetLengths));
			
			shadowTerm[y] = dot(inLight, fixed3(fracWeights.z, 1, fracWeights.x));
		}        
		
		//We multiply by 0.25 unintuitively because the frac() wittles off 1 weight from width and height, making the sum 2x2=4... so 1/4th
		return dot(shadowTerm, fixed3(fracWeights.w, 1, fracWeights.y) * 0.25);
	}
	fixed ShadowFilterPCF4x4(float3 shadowCoordDepth)
	{
		//4x4 isn't centered nicely :)
		shadowCoordDepth.xy -= _ShadowMapTexture_TexelSize.xy * 0.5;
	
		// Edge tap smoothing
		fixed4 fracWeights = frac(shadowCoordDepth.xy * _ShadowMapTexture_TexelSize.zw).xyxy;
		fracWeights.zw = 1.0-fracWeights.xy;
	
		//Read from the center of texels to avoid floating point error when reading neighbors...
		float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);
	
		fixed4 shadowTerm = 0;
		for (int y = 0; y < 2; y++)
		{
			for (int x = 0; x < 2; x++)
			{
				// Hard-coded bias values, because this filter uses a quirky sample pattern to optimize texture fetches...
				const float offsetInner = 0.707106781186548, offsetOuter = 1.58113883008419, offsetCorner = 2.121320343559642; //0.5x0.5, 1.5x0.5, 1.5x1.5
				float4 offsetLengths;
				if(x==0 && y==0)		offsetLengths = float4(offsetCorner, offsetOuter, offsetOuter, offsetInner);
				else if(x==1 && y==0)	offsetLengths = float4(offsetOuter, offsetCorner, offsetInner, offsetOuter);
				else if(x==0 && y==1)	offsetLengths = float4(offsetOuter, offsetInner, offsetCorner, offsetOuter);
				else					offsetLengths = float4(offsetInner, offsetOuter, offsetOuter, offsetCorner);
				
				float4 samples;
				samples.x = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(x*2-1, y*2-1), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.x)));
				samples.y = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(x*2  , y*2-1), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.y)));
				samples.z = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(x*2-1, y*2  ), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.z)));
				samples.w = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + _ShadowMapTexture_TexelSize.xy * float2(x*2  , y*2  ), SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLengths.w)));
				
				fixed4 inLight = SOFTENER_4_SAMPLES_LIT(samples, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.zzzz, offsetLengths));
				
				if(x==0)
				{
					shadowTerm[y*2] = dot(inLight.xy, float2(fracWeights.z, 1));
					shadowTerm[y*2+1] = dot(inLight.zw, float2(fracWeights.z, 1));
				}
				else
				{
					shadowTerm[y*2] += dot(inLight.xy, float2(1, fracWeights.x));
					shadowTerm[y*2+1] += dot(inLight.zw, float2(1, fracWeights.x));
				}
			}
		}
		//We multiply by 0.11111 unintuitively because the frac() wittles off 1 weight from width and height, making the sum 3x3=9... so 1/9th
		return dot(shadowTerm, fixed4(fracWeights.w, 1, 1, fracWeights.y) * 0.11111);
	}
	float ShadowFilterPCFx(float3 shadowCoordDepth, uniform int sqrtSamples)
	{
		shadowCoordDepth.xy -= _ShadowMapTexture_TexelSize.xy * (sqrtSamples - 1.0) * 0.5;
	    
		// Edge tap smoothing
		float4 fracWeights = frac(shadowCoordDepth.xy * _ShadowMapTexture_TexelSize.zw).xyxy;
		fracWeights.zw = 1-fracWeights.zw;
		
	
		//Hack to center texel sampling...
		float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);
	
		const_if_u4 int sqrtSamplesMinOne = sqrtSamples - 1;
		const_if_u4 int samples = sqrtSamples * sqrtSamples;

		float shadowTerm = 0.0; 
		
		const_if_u4 float2 centerPoint = (float2)sqrtSamples / 2.0;
	     
		for (int y = 0; y < sqrtSamples; y++)
		{
			for (int x = 0; x < sqrtSamples; x++)
			{
				float offsetLength = length(float2(x, y) - centerPoint);
				
				float sample = SOFTENER_SAMPLE_SHADOW(float3(sampleCoord + float2(x, y) * _ShadowMapTexture_TexelSize.xy, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength)));
				
				float inLight = SOFTENER_1_SAMPLE_LIT(sample, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.z, offsetLength));
	            
				// Edge tap smoothing
				float2 weight = 1;
				if (x == 0)
					weight.x = fracWeights.z;
				else if (x == sqrtSamplesMinOne)
					weight.x = fracWeights.x;
	                
				if (y == 0)
					weight.y = fracWeights.w;
				else if (y == sqrtSamplesMinOne)
					weight.y = fracWeights.y;
	                
				shadowTerm += inLight * weight.x * weight.y;
			}                                            
		}        
	    
		return shadowTerm / (sqrtSamplesMinOne * sqrtSamplesMinOne);
	}
	
	float ShadowFilterCube(float4 shadowCoordDepth, uniform int samplesX4)
	{
		float4 shadows = 0;
		for(int i=0; i<samplesX4; i++)
		{
			float4 samples;
			float2 z = float2(i+1.0, -i-1.0) / 128.0 * SOFTENER_POINT_KERNEL;
			if((i%2)==1) z = -z;
			samples.x = SOFTENER_SAMPLE_SHADOW(shadowCoordDepth.xyz + z.xxx);
			samples.y = SOFTENER_SAMPLE_SHADOW(shadowCoordDepth.xyz + z.yyx);
			samples.z = SOFTENER_SAMPLE_SHADOW(shadowCoordDepth.xyz + z.yxy);
			samples.w = SOFTENER_SAMPLE_SHADOW(shadowCoordDepth.xyz + z.xyy);
			shadows += SOFTENER_4_SAMPLES_LIT(samples, SOFTENER_SAMPLE_BIAS(shadowCoordDepth.w, (float)i));
		}
		return dot(shadows, 0.25 / (float)samplesX4);
	}
#endif