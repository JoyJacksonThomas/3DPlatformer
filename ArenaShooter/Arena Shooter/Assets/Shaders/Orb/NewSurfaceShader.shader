Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Emission("Emission", Range(0,10)) = 0.0
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        _GradientColor("Gradient Color", Color) = (1,1,1,1)
        _TimeScale("Time Scale", Range(0, 1)) = 0.0
        _DistortionIntensity("Distortion Intensity", Range(0, .15)) = 0.0
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

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        half _Emission;
        half _TimeScale;
        half _DistortionIntensity;
        float3 _FresnelColor;
        float3 _GradientColor;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float2 time = float2(0, -_Time.w * _TimeScale);
            fixed4 c = tex2D (_MainTex, i.uv_MainTex + time) * _Color;
            
            o.Albedo = clamp((-i.uv_MainTex.y + .7), 0.0, 1.0 )* _GradientColor* 2 ;
            //// Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness; 
            o.Alpha = c.a;
            
            float dudv = (c.rgb*2 - 1)* _DistortionIntensity;
            float fresnel = dot(i.worldNormal + dudv, i.viewDir);
            float upperFresnel = dot(i.worldNormal, float3(0,-1,0));
            fresnel = saturate(1 - (fresnel* (1 - upperFresnel)*1.5));
            //fresnel = saturate(1 - fresnel );
            float3 fresnelColor = fresnel * _FresnelColor; 
            o.Emission =( _Emission + fresnelColor) * 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
