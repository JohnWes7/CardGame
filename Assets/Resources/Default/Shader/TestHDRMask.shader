Shader "Test/TestHDRMask"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            // Use custom blend mode
            Blend One OneMinusSrcAlpha
            ZWrite Off
            ColorMask RGBA
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;

                // Clamp the color and alpha to be within [0, 1]
                col = clamp(col, 0.0, 1.0);

                // Apply custom blending
                fixed4 src = col; // This is the source color (already clamped)
                fixed4 dst = fixed4(0, 0, 0, 0); // Destination color is initialized to zero

                // Manually compute the blend
                fixed4 result;
                result.rgb = src.rgb * src.a + dst.rgb * (1.0 - src.a);
                result.a = src.a + dst.a * (1.0 - src.a);

                return result;
            }
            ENDCG
        }
    }
}