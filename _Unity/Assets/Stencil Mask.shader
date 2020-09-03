Shader "Custom/StencilRead"{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent+3"}
			Stencil{
				Ref 5
				Comp Always
				Pass Replace
			}
			ZWrite Off
			LOD 100

   CGPROGRAM
			#pragma surface surf Standard fullforwardshadows exclude_path:deferred addshadow
			#pragma target 3.0
			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
}