Shader "Custom/Fire" {
	Properties {
		[HDR] _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ScrollVor ("Voronoi Scroll", Float) = 1
		_ScrollGrad ("Gradient Scroll", Float) = 1
		_DistortionAmount("Distortion Amount", Float) = 1
		_DistortionScale ("Distortion Scale", Float) = 5
		_DissolveScale("Dissolve Scale", Float) = 1
		_DissolveAmount("Dissolve Amount", Float) = 1.2
	}
	SubShader {
		Tags { "Queue" = "Transparent""RenderType"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _ScrollVor;
		float _ScrollGrad;
		float _DistortionAmount;
		float _DissolveScale;
		float _DistortionScale;
		float _DissolveAmount;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		float2 unity_gradientNoise_dir(float2 p)
		{
			p = p % 289;
			float x = (34 * p.x + 1) * p.x % 289 + p.y;
			x = (34 * x + 1) * x % 289;
			x = frac(x / 41) * 2 - 1;
			return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
		}

		float unity_gradientNoise(float2 p)
		{
			float2 ip = floor(p);
			float2 fp = frac(p);
			float d00 = dot(unity_gradientNoise_dir(ip), fp);
			float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
			float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
			float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
			fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
			return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
		}

		void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
		{
			Out = unity_gradientNoise(UV * Scale) + 0.5;
		}

		inline float2 unity_voronoi_noise_randomVector (float2 UV, float offset)
		{
			float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
			UV = frac(sin(mul(UV, m)) * 46839.32);
			return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
		}

		void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out)
		{
			float2 g = floor(UV * CellDensity);
			float2 f = frac(UV * CellDensity);
			float t = 8.0;
			float3 res = float3(8.0, 0.0, 0.0);

			for(int y=-1; y<=1; y++)
			{
				for(int x=-1; x<=1; x++)
				{
					float2 lattice = float2(x,y);
					float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
					float d = distance(lattice + offset, f);
					if(d < res.x)
					{
						res = float3(d, offset.x, offset.y);
						Out = res.x;
					}
				}
			}
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Scroll gradient
			float2 sgrad = (0.1, _ScrollGrad);
			float2 tg = _Time * sgrad;
			tg += (1,1) * IN.uv_MainTex;

			// Scroll voronoi
			float2 svor = (0, _ScrollVor);
			float2 tv = _Time * svor;
			tv += (1,1) * IN.uv_MainTex;

			float tuvg;
			float tuvv;

			// Add gradient noise
			Unity_GradientNoise_float(tg, _DistortionScale, tuvg);

			// Add voronoi
			Unity_Voronoi_float(tv, 2, _DissolveScale, tuvv);
			tuvv = pow(tuvv, _DissolveAmount);
			float uvgradvor = tuvg * tuvv;
			IN.uv_MainTex = lerp(IN.uv_MainTex, tuvg, _DistortionAmount);

			// Albedo comes from a texture
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			float4 main = c * uvgradvor;
			main *= _Color;
			o.Albedo = main.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = main.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
