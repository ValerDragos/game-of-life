Shader "TEST/sampling" 
{
	SubShader {
	 	
	 	LOD 200
	 	
		Tags 
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True"
		}
		
		Pass
		{
			//Cull Off
			Lighting Off
			//ZWrite Off
			//Offset -1, -1
			Fog { Mode Off }
			//ColorMask RGB
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360 because it uses wrong array syntax (type[size] name)
			#pragma exclude_renderers d3d11 xbox360
			//#pragma target 2.0
			//#pragma glsl
			#pragma vertex vert 
			#pragma fragment frag
			
			static const fixed4 WHITE = fixed4(1, 1, 1, 1);
			static const fixed4 BLACK = fixed4(0, 0, 0, 1);
			
			static const fixed4 RED = fixed4(1, 0, 0, 1);
			static const fixed4 RED_GREEN = fixed4(1, 1, 0, 1);
			static const fixed4 GREEN = fixed4(0, 1, 0, 1);
			static const fixed4 GREEN_BLUE = fixed4(0, 1, 1, 1);
			static const fixed4 BLUE = fixed4(0, 0, 1, 1);
			static const fixed4 BLUE_RED = fixed4(1, 0, 1, 1);
			
			static const int DIVISIONS = 6;
			
			// base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				fixed4 color: COLOR;
				float2 texcoord: TEXCOORD1;
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = float2(v.texcoord.x*DIVISIONS, v.texcoord.y*2);
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
				fixed4 col =  
				RED * (1-min(i.texcoord.x,1))+
				RED_GREEN * (1-abs(clamp(i.texcoord.x-1,-1,1)))+
				GREEN * (1-abs(clamp(i.texcoord.x-2,-1,1)))+
				GREEN_BLUE * (1-abs(clamp(i.texcoord.x-3,-1,1)))+
				BLUE * (1-abs(clamp(i.texcoord.x-4,-1,1)))+
				BLUE_RED * (1-abs(clamp(i.texcoord.x-5,-1,1)))+
				RED * (1-abs(clamp(i.texcoord.x-6,-1,1)));

				col *= i.color;

				return lerp(lerp(BLACK, col, min(i.texcoord.y, 1)), WHITE, max(0, i.texcoord.y-1));
			}
			ENDCG
		}
	} 
	FallBack Off
}
