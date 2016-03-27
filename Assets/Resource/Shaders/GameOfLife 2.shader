Shader "Processers/GOL 1-1 finalPlane V3" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "grey" {}
	}
	SubShader {
	 
		Tags { "Queue" = "Background" "IgnoreProjector" = "True" }
		
		//int pack(float3 aaa)
		//{
		//  	unsigned int3 parts;
		//  	parts.x = round(15.0 * aaa.x);
		//  	parts.y = round(15.0 * aaa.y);
		//  	parts.z = round(15.0 * aaa.z);
		//  	int bbb = ((parts.z << 8) | (parts.y << 4) | parts.x);
		//  	return bbb;
		//}
		//	
		//fixed3 unpack(unsigned int aaa)
		//{
		//	fixed3 result;
		//	result.x = (aaa & 0xF) / 15.0;
		//	result.y = ((aaa >> 4) & 0xF) / 15.0;
		//	result.z = ((aaa >> 8) & 0xF) / 15.0;
		//	return result;
		//}
		
		Pass
		{
			//Cull Off
			Lighting Off
			ZWrite Off
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
			
			const float STEP = 0.1;
			
			// user defined variables
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			// base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex[5] : TEXCOORD0;
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				float texOffsetX = _ScreenParams.z - 1.0;
				float texOffsetY = _ScreenParams.w - 1.0;
			
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex[4].xy = v.texcoord.xy;
				o.tex[4].zw = float2(0,0);
				o.tex[0] = float4 (v.texcoord.xy + float2(-texOffsetX,texOffsetY), v.texcoord.xy + float2(0.0,texOffsetY));
				o.tex[1] = float4 (v.texcoord.xy + float2(texOffsetX,texOffsetY), v.texcoord.xy + float2(texOffsetX,0.0));
				o.tex[2] = float4 (v.texcoord.xy + float2(texOffsetX,-texOffsetY), v.texcoord.xy + float2(0.0,-texOffsetY));
				o.tex[3] = float4 (v.texcoord.xy + float2(-texOffsetX,-texOffsetY), v.texcoord.xy + float2(-texOffsetX,0.0));
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
			fixed2 mainTex = tex2D(_MainTex, i.tex[4].xy).xy;
			mainTex.y += 0.0666666666666667;
			
			half count = tex2D(_MainTex, i.tex[0].xy).x;
			count += tex2D(_MainTex, i.tex[0].zw).x;
			count += tex2D(_MainTex, i.tex[1].xy).x;
			count += tex2D(_MainTex, i.tex[1].zw).x;
			count += tex2D(_MainTex, i.tex[2].xy).x;
			count += tex2D(_MainTex, i.tex[2].zw).x;
			count += tex2D(_MainTex, i.tex[3].xy).x;
			count += tex2D(_MainTex, i.tex[3].zw).x;
			
			count = ceil(count);
					
				int condition = (int)!(count<2 || count>3);
				float4 aaa = float4(condition,mainTex.y,0,1);
				condition = (int)(count==3);
				float4 bbb = float4(condition,0.0666666666666667,0,0);

			return mainTex.x*aaa + (1-mainTex.x)*bbb;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
