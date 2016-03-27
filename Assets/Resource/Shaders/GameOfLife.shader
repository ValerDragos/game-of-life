Shader "Processers/GOL 1-1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "grey" {}
	}
	SubShader {
	 
		Tags { "Queue" = "Geometry" }
		ZWrite Off
		
		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360 because it uses wrong array syntax (type[size] name)
			#pragma exclude_renderers d3d9 d3d11 xbox360
			//#pragma target 2.0
			//#pragma glsl
			#pragma vertex vert 
			#pragma fragment frag
			
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
				//half2 texCenter : TEXCOORD1; 
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				float texOffsetX = _ScreenParams.z - 1.0;
				float texOffsetY = _ScreenParams.w - 1.0;
				
				//float texOffsetX = _TexelSize;
				//float texOffsetY = _TexelSize;
			
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex[4].xy = v.texcoord.xy;
				o.tex[0] = float4 (v.texcoord.xy + float2(-texOffsetX,texOffsetY), v.texcoord.xy + float2(0.0,texOffsetY));
				o.tex[1] = float4 (v.texcoord.xy + float2(texOffsetX,texOffsetY), v.texcoord.xy + float2(texOffsetX,0.0));
				o.tex[2] = float4 (v.texcoord.xy + float2(texOffsetX,-texOffsetY), v.texcoord.xy + float2(0.0,-texOffsetY));
				o.tex[3] = float4 (v.texcoord.xy + float2(-texOffsetX,-texOffsetY), v.texcoord.xy + float2(-texOffsetX,0.0));
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
			//	float texOffsetX = _ScreenParams.z - 1.0;
			//	float texOffsetY = _ScreenParams.w - 1.0;
				
			//float2 texCoord = i.tex.xy;
			//int count0 = 0;
			//// Center
			//half4 sampleA = tex2D(_MainTex, texCoord);
			//count0 = (int)sampleA.x;
			//// TopLeft
			//texCoord += float2(-texOffsetX,texOffsetY); 
			//sampleA += tex2D(_MainTex, texCoord);
			//// Top
			//texCoord += float2(texOffsetX, 0.0);
			//sampleA += tex2D(_MainTex, texCoord);
			//// TopRight
			//texCoord += float2(texOffsetX, 0.0);
			//sampleA += tex2D(_MainTex, texCoord);
			//// Right
			//texCoord += float2(0.0, -texOffsetY);
			//sampleA += tex2D(_MainTex, texCoord);
			//// Left
			//texCoord += float2(-texOffsetX*2.0, 0.0);
			//sampleA += tex2D(_MainTex, texCoord);
			//// BottomLeft
			//texCoord += float2(0.0, -texOffsetY);
			//sampleA += tex2D(_MainTex, texCoord);
			//// Bottom
			//texCoord += float2(texOffsetX, 0.0);
			//sampleA += tex2D(_MainTex, texCoord);
			//// BottomRight
			//texCoord += float2(texOffsetX, 0.0);
			//sampleA += tex2D(_MainTex, texCoord);
			//return (((tex2D(_MainTex, i.tex[4].xy).x) == 1)? fixed4(1,0,0,0.2) : fixed4(1,1,1,1));
			//float2 texCoord = i.tex.xy;
			fixed count0 = 0;
			// Center
			//half4 sampleA = tex2D(_MainTex, i.texCenter);
			float sampleA = tex2D(_MainTex, i.tex[4].xy).x;
			
//return sampleA;
			count0 = ceil(sampleA.x);
			//return ((count0 == 1)? fixed4(1,1,1,1) : fixed4(0,0,0,0));
			sampleA = tex2D(_MainTex, i.tex[0].xy).x;
			sampleA += tex2D(_MainTex, i.tex[0].zw).x;
			sampleA += tex2D(_MainTex, i.tex[1].xy).x;
			sampleA += tex2D(_MainTex, i.tex[1].zw).x;
			sampleA += tex2D(_MainTex, i.tex[2].xy).x;
			sampleA += tex2D(_MainTex, i.tex[2].zw).x;
			sampleA += tex2D(_MainTex, i.tex[3].xy).x; 
			sampleA += tex2D(_MainTex, i.tex[3].zw).x;
			int count = ceil(sampleA);
			//return sampleA;
			// TopLeft
			//sampleA = sampleA + tex2D(_MainTex, texCoord + float2(-texOffsetX,texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex, texCoord + float2(0.0,texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex, texCoord + float2(texOffsetX,texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex2, texCoord + float2(texOffsetX,0.0)); 
			//sampleA = sampleA + tex2D(_MainTex, texCoord + float2(texOffsetX,-texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex3, texCoord + float2(0.0,-texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex, texCoord + float2(-texOffsetX,-texOffsetY));
			//sampleA = sampleA + tex2D(_MainTex3, texCoord + float2(-texOffsetX,0.0));
			//	 return (count == _AAA)? fixed4(0,1,0,1) : fixed4(1,0,0,1);
	//int count = ceil(sampleA.x);
				//return center + fixed4(0.1,0.1,0.1,0.1);
				//int count = (int)(topLeft.x+top.x+topRight.x+right.x+bottomRight.x+bottom.x+bottomLeft.x+left.x);
				bool rez1 = count<2 || count>3;
				fixed4 aaa = (rez1)? fixed4(0,0.0,0.0,0) : fixed4(1,0.0,0.0,1);
				fixed4 bbb = (count==3)? fixed4(1,0.0,0.0,1) : fixed4(0.0,0.0,0.0,0);
				//fixed4 aaa = (rez1)? fixed4(0,0.0,0.0,0) : fixed4(0,0.0,0.0,1);
				//fixed4 bbb = (count==3)? fixed4(0,0.0,0.0,1) : fixed4(0.0,0.0,0.0,0);
	return ((count0>0)? aaa : bbb);
			//return fixed4(1,1,1,1);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
