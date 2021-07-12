Shader "Example/Bump" {
    Properties{
      _MainTex("Texture", 2D) = "white" {}
      _BumpMap("Bumpmap", 2D) = "bump" {}
    }
        SubShader{
          Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }
          CGPROGRAM
          #pragma surface surf Standard fullforwardshadows alpha 
          struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
          };
          sampler2D _MainTex;
          sampler2D _BumpMap;
          void surf(Input IN, inout SurfaceOutputStandard o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            //o.Normal.y = -o.Normal.y;
            o.Alpha = 1.0f;
          }
          ENDCG
    }
        Fallback "Diffuse"
}