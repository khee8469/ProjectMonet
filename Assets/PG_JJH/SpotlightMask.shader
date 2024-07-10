Shader "Custom/SpotlightMask"
{
    Properties
    {
        _BaseMap("BaseMap", 2D) = "white" {}
        _SpotlightPosition("Spotlight Position", Vector) = (0, 0, 0, 0)
        _SpotlightDirection("Spotlight Direction", Vector) = (0, 0, -1, 0)
        _SpotlightAngle("Spotlight Angle", Range(0, 180)) = 45
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;
            float4 _SpotlightPosition;
            float4 _SpotlightDirection;
            float _SpotlightAngle;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS).xyz;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                float3 toPixel = IN.worldPos - _SpotlightPosition.xyz;
                float angle = degrees(acos(dot(normalize(toPixel), normalize(_SpotlightDirection.xyz))));
                
                if (angle < _SpotlightAngle / 2)
                {
                    return baseColor;
                }
                else
                {
                    baseColor.a = 0;
                    return baseColor;
                }
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}