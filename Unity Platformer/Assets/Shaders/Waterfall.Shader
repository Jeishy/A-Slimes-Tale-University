Shader "Custom/Waterfall" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Waterspeed ("Waterfall Speed", Range(0, 4)) = 1.0
		_AlphaCutoff ("Alpha Cutoff", Range (0,1)) = 1.0
	}
	SubShader {
		Tags { "Queue" = "Opaque" "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _AlphaTex;
		

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic, _Waterspeed;
		fixed4 _Color;
		float _AlphaCutoff;


		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
				
			float2 uv = IN.uv_MainTex;
				
			uv.y += -_Waterspeed * _Time.y; 
			uv.x += sin ((uv.x + uv.y) * 8 + _Time.g * 1.3) * 0.01; 
			
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, uv) * _Color;
			float a = tex2D(_AlphaTex, uv).r;
			
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));

			if (a > _AlphaCutoff)
				o.Alpha = 1;
			else
				o.Alpha = 0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
