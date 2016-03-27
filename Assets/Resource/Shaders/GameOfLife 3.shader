Shader "Processers/GOL 1-1 finalPlane V4" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "grey" {}
		_RuleTex ("Rules texture", 2D) = "white" {}
		_SpeedZ ("Speed Z", float) = 1
		_SpeedW ("Speed W", float) = 1
	}
	SubShader {
	 
		Tags { "Queue" = "Background" "IgnoreProjector" = "True" }
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			//Offset -1, -1
			Fog { Mode Off }
			//ColorMask RGB
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360 because it uses wrong array syntax (type[size] name)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert_new_frame
			#pragma fragment frag_new_frame
			
			#pragma multi_compile SAMPLES_8 SAMPLES_7 SAMPLES_6 SAMPLES_5 SAMPLES_4 SAMPLES_3 SAMPLES_2 SAMPLES_1, NO_SAMPLES
			
			static int SAMPLES_AMOUNT = 8;
			
			uniform float4 _SampleOffset1;
			uniform float4 _SampleOffset2;
			uniform float4 _SampleOffset3;
			uniform float4 _SampleOffset4;
					
			uniform sampler2D _MainTex;
			uniform sampler2D _RuleTex;
			uniform float _SpeedX;
			uniform float _SpeedZ;
			uniform float _SpeedW;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 offsets[4] : TEXCOORD1;
			};
		#if !defined (NO_SAMPLES)
			fixed2 process_neighbour_sample (float2 texCoord)
			{
				fixed2 sample = tex2D(_MainTex, texCoord).xy;
				sample.x = floor(sample.x);
				sample.y *= sample.x;
				return sample;
			}
			float2 neighbours_sample (float4 texCoords[4])
			{
				float2 count = process_neighbour_sample(texCoords[0].xy);
			#if defined (SAMPLES_1)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[0].zw);
			#if defined (SAMPLES_2)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[1].xy);
			#if defined (SAMPLES_3)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[1].zw);
			#if defined (SAMPLES_4)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[2].xy);
			#if defined (SAMPLES_5)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[2].zw);
			#if defined (SAMPLES_6)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[3].xy);
			#if defined (SAMPLES_7)
				return count;
			#endif
				count += process_neighbour_sample(texCoords[3].zw);
				return count;
			}
		#endif
			// vertex functions
			vertexOutput vert_new_frame (vertexInput v)
			{
				//float2 texOffset = _ScreenParams.zw - 1.0;
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				
				//o.offsets[0] = float4 (v.texcoord + float2(-texOffset.x,texOffset.y), v.texcoord + float2(0.0,texOffset.y));
				//o.offsets[1] = float4 (v.texcoord + float2(texOffset.x,texOffset.y), v.texcoord + float2(texOffset.x,0.0));
				//o.offsets[2] = float4 (v.texcoord + float2(texOffset.x,-texOffset.y), v.texcoord + float2(0.0,-texOffset.y));
				//o.offsets[3] = float4 (v.texcoord + float2(-texOffset.x,-texOffset.y), v.texcoord + float2(-texOffset.x,0.0));
				
				o.offsets[0] = float4 (v.texcoord + _SampleOffset1.xy, v.texcoord + _SampleOffset1.zw);
				o.offsets[1] = float4 (v.texcoord + _SampleOffset2.xy, v.texcoord + _SampleOffset2.zw);
				o.offsets[2] = float4 (v.texcoord + _SampleOffset3.xy, v.texcoord + _SampleOffset3.zw);
				o.offsets[3] = float4 (v.texcoord + _SampleOffset4.xy, v.texcoord + _SampleOffset4.zw);
				
				return o;
			} 
			
			// fragment functions
			float4 frag_new_frame (vertexOutput i) : COLOR
			{
				float4 mainTex = tex2D(_MainTex, i.texcoord);
				
				//float lifetime = mainTex.z * 255.0 + mainTex.w + (_Speed/255); // _Speed is the added lifetime per frame.
				//int intLifeTime = (int)lifetime; // repeat
				//mainTex.z = (float)(intLifeTime % 256) / 255.0; // repeat
				//mainTex.w = lifetime - intLifeTime;
				
				int currentState = floor(mainTex.x);
				float2 count = neighbours_sample(i.offsets);
				float average = count.y/count.x;
				
				
				float2 ruleSample = tex2D(_RuleTex, float2(count.x / SAMPLES_AMOUNT, currentState * mainTex.y + (1 - currentState)* average)).xy;
				// if allive Y: A-255,D-254, if dead X: A-255, D-0
					
					
					
					int ifAlliveState = floor(ruleSample.y);
					
					
					float4 if_allive = float4(
					ruleSample.y,
					mainTex.y,
					mainTex.z + _SpeedZ,  // adds maybe 1 more time when becomes dead, can multiply with  ifAlliveState to prevent
					mainTex.w + _SpeedW);
					
					float4 if_dead = float4(
					mainTex.x - _SpeedX + ruleSample.x, // can subtract more than ruleSample.x adds and still be dead, multiply Speedx with (1-currentState) or multiply ruleSample.x with a high number
					average * ruleSample.x + (1-ruleSample.x) * mainTex.y,
					(1-ruleSample.x)*mainTex.z,
					(1-ruleSample.x)*mainTex.w);

				return saturate(currentState * if_allive + (1 - currentState) * if_dead);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
