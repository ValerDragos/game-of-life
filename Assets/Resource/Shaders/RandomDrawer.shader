Shader "Processers/RandomDrawer" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
		_Size ("Width", Vector) = (1,1,0,0)
	}
	SubShader {
		Tags 
		{ 
			"Queue" = "Geometry" 
			"IgnoreProjector" = "True"
		}
		
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
			//#pragma target 2.0
			//#pragma glsl
			#pragma vertex vert 
			#pragma fragment frag
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float2 _Size;
			
			// base input structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD;
				fixed4 color: COLOR;
			};
			
			// vertex function
			vertexOutput vert (vertexInput v)
			{
				vertexOutput o; 
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = v.texcoord * _Size;
				o.color = v.color;
				
				return o;
			} 
			
			// fragment function
			fixed4 frag(vertexOutput i) : COLOR
			{
				float pixelPosition = i.tex.x + i.tex.y * _Size.x;
				
				return i.color * tex2D(_MainTex, float2((pixelPosition % _MainTex_TexelSize.z) * _MainTex_TexelSize.x, 0.5));
			}
			ENDCG
		}
	} 
	FallBack Off
}
