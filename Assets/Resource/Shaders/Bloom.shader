Shader "Processers/Bloom_Custom" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "grey" {}
		_Distance ("Distance", Float) = 1
	}
	//SubShader {
	 CGINCLUDE
		//Tags 
		//{ 
		//	"Queue" = "Geometry" 
		//	"IgnoreProjector" = "True"
		//}
		
		//Pass
		//{
			//ZTest Off Cull Off ZWrite Off Blend Off
	  		//Fog { Mode off }  
		
			//CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360 because it uses wrong array syntax (type[size] name)
			#pragma exclude_renderers d3d11 xbox360
			//#pragma target 2.0
			//#pragma glsl
			//#pragma vertex vert 
			//#pragma fragment frag
			
			// user defined variables
			uniform sampler2D _MainTex;
			uniform float _Distance;
			//uniform half4 _MainTex_TexelSize;
			
			static const float4 FIRST_1 = { 0, 1, 0, -1};
			static const float4 SECOND_1 = { 1, 0, -1, 0};
			
			static const float4 FIRST_2 = { -1, 0, 1, 0};
			static const float4 SECOND_2 = { -2, 0, 2, 0};
			
			static const float weights[3] = { 2, 0.4, 0.6};
			
			// base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				float4 tex2 : TEXCOORD1;
				float4 tex3 : TEXCOORD2;
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				float4 aaa = float4(v.texcoord,v.texcoord);
				o.tex2 = aaa + FIRST_1 * _Distance;
				o.tex3 = aaa + SECOND_1 * _Distance;
				
				return o;
			} 
			vertexOutput vert2 (vertexInput v)
			{
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord;
				float4 aaa = float4(v.texcoord,v.texcoord);
				o.tex2 = aaa + FIRST_2 * _Distance;
				//o.tex3 = aaa + SECOND_2 * _Distance;
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
				float4 sample = tex2D(_MainTex, i.tex);
				
				float4 sample2 = tex2D(_MainTex, i.tex2.xy);
				float4 sample3 = tex2D(_MainTex, i.tex2.zw);
				float4 sample4 = tex2D(_MainTex, i.tex3.xy);
				float4 sample5 = tex2D(_MainTex, i.tex3.zw);
				
				return sample*sample.x*weights[0] + (1-sample.x)*(sample2*weights[1] + sample3*weights[1] + sample4*weights[1] + sample5*weights[1]) ;
			}
			fixed4 frag2(vertexOutput i) : COLOR
			{
				//float4 sample = tex2D(_MainTex, i.tex);
				
				float4 sample2 = tex2D(_MainTex, i.tex2.xy);
				float4 sample3 = tex2D(_MainTex, i.tex2.zw);
				float4 sample4 = tex2D(_MainTex, i.tex3.xy);
				float4 sample5 = tex2D(_MainTex, i.tex3.zw);
				
				return (sample2 + sample3 + sample4 + sample5)*weights[2] ;
			}
			ENDCG
		//}
		
	//} 
	SubShader
	{
		ZTest Off Cull Off ZWrite Off Blend Off
	  	Fog { Mode off }  
	
		Pass 
		{
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag2
			#pragma fragmentoption ARB_precision_hint_fastest 
			
			ENDCG
			 
		}
		//Pass 
		//{
		//	Blend One One
		//	CGPROGRAM
		//	#pragma vertex vert2
		//	#pragma fragment frag2
		//	//#pragma fragmentoption ARB_precision_hint_fastest 
		//	
		//	ENDCG
		//	 
		//}
	}
	FallBack Off
}
