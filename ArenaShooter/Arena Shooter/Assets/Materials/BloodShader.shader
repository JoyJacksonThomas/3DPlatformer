Shader "Particles/BloodShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalTex("Normal (RGB)", 2D) = "white" {}
        _DuDvTex("dudv (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Range(0,1)) = 0.0
        _Power("Power", Range(0,5)) = 0.0
        _DistortionIntensity("Distortion Intensity", Range(0, 1)) = 0.0
        _TimeScale("Time Scale", Range(0, 10)) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }
        //Blend SrcAlpha OneMinusSrcAlpha
        ZWrite on
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows  vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalTex;
        sampler2D _DuDvTex;


        struct appdata_particles {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 color : COLOR;
            float4 texcoords : TEXCOORD0;
            float4 customData : TEXCOORD1;
            float4 customData2 : TEXCOORD2;
            float4 tangent : TANGENT;
        };

        struct Input
        {
            float2 texcoord;
            float2 texcoord1;
            float4 color;
        };
        
        void vert(inout appdata_particles v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.texcoord = v.texcoords;
            o.texcoord1 = v.customData.xy;
            o.color = v.customData2;
        }

        half _Glossiness;
        half _Specular;
        half _Power;
        half _NormalPower;
        half _DistortionIntensity;

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 dist = tex2D(_DuDvTex, IN.texcoord*.06);
            float2 dudv = (dist.rgb * 2 - 1) * _DistortionIntensity;
            fixed4 c = tex2D (_MainTex, IN.texcoord + dudv);
            
            //fixed4 n = tex2D(_NormalTex, IN.texcoord);
            float ba = 1 - c.r;

            c *= IN.color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Specular = _Specular;
            o.Smoothness = _Glossiness;
            o.Occlusion = 1;
            o.Normal = UnpackNormal(tex2D(_NormalTex, IN.texcoord + dudv));
            o.Normal.y = -o.Normal.y;
            //fixed4 colB = tex2D(_MainTex, IN.texcoord1);

            o.Alpha = 1.0f;
            o.Alpha = (1 - IN.texcoord1.r) * .9f;
            o.Alpha = ba;

            ba = pow(ba, _Power);
            clip((1 - IN.texcoord1.r)*.6f - ba);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
