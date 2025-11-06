Shader "Custom/MusicVisualizer"
{
    Properties
    {
        _Bars("Bar Count", Integer) = 64
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _Bars;
            float _Frequency[512];
            CBUFFER_END

            struct Attributes
            {
                float4 positionLS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionLS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float Bars(float2 uv, float height)
            {
                uv.y = abs((uv.y - .5) * 2);
                return step(uv.y,height) * step(0.3, frac(uv.x * _Bars )) * 4;
            }
             
            float4 frag(Varyings IN) : SV_Target
            {
                float h = (_Frequency[IN.uv.x * _Bars] * 2.5) + .01;
                float3 color = Bars(IN.uv, h);
                color *= float3(sin(IN.uv.x + 0.074536), cos(IN.uv.y + 1.8), 0.5);
                return float4(color, 1);
            }
            ENDHLSL
        }
    }
}
