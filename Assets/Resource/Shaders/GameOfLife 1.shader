Shader "Processers/GOL 1-1 finalPlane V2" {
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
			//int vvvvvv = tex2D(_MainTex, i.tex[4].xy).x;
			//return int4(vvvvvv,0,0,0);
			
			
			float mainTex = tex2D(_MainTex, i.tex[4].xy).x;
			//return (mainTex==1)? fixed4(1,1,1,1) : fixed4(0,0,0,0);
			fixed count0 = (int)(mainTex>0);
			mainTex += 0.5;
			
			fixed original = tex2D(_MainTex, i.tex[0].xy).x;
			bool biggerThanZero = original>0;
			int count = (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[0].zw).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[1].xy).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[1].zw).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[2].xy).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[2].zw).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[3].xy).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;
			
			original = tex2D(_MainTex, i.tex[3].zw).x;
			biggerThanZero = original>0;
			count += (int)biggerThanZero;

				float4 aaa = float4(saturate((int)!(count<2 || count>3)*mainTex),0.0,0.0,0);
				float4 bbb = float4((int)(count==3) * 0.09,0.0,0.0,1);

			return count0*aaa + (1-count0)*bbb;
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
