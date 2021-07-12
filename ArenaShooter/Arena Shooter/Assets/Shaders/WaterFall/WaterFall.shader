Shader "Custom/WaterFall"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _WaterFallTex ("Waterfall (RGB)", 2D) = "white" {}
        _DistortionTex("Distortion (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _DistortionIntensity("Distortion Intensity", Range(0, 1)) = 0.0
        _TimeScale("Time Scale", Range(0, 10)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _WaterFallTex;
        sampler2D _DistortionTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _DistortionIntensity;
        half _Glossiness;
        half _Metallic;
        half _TimeScale;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 dist = tex2D(_DistortionTex, IN.uv_MainTex + float2(0., -_Time.x));
            float2 dudv = (dist.rgb * 2 - 1) * _DistortionIntensity;
            fixed4 falls = tex2D (_WaterFallTex, IN.uv_MainTex+ dudv + float2(0., _Time.x*_TimeScale)) * _Color;
            float2 falls2 = (falls * 2 - 1) * .01;
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex + (falls2.rg));// *_Color;

            float uvPosY = 1 - IN.uv_MainTex.y + falls.rg;
            float3 whiteBottom = float3(uvPosY, uvPosY, uvPosY);
            
            o.Albedo =  c.rgb + whiteBottom * .2 ;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
