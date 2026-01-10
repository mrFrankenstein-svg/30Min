Shader "Custom/URP/MultiDistortionTransparent"
{
    Properties
    {
        _Tex1("Texture 1", 2D) = "white" {}
        _Tex2("Texture 2", 2D) = "white" {}
        _Tex3("Texture 3", 2D) = "white" {}

        _DistortionStrength1("Distortion Strength 1", Float) = 0.02
        _DistortionStrength2("Distortion Strength 2", Float) = 0.02
        _DistortionStrength3("Distortion Strength 3", Float) = 0.02

        _DistortionDir1("Distortion Direction 1", Vector) = (1, 0, 0, 0)
        _DistortionDir2("Distortion Direction 2", Vector) = (0, 1, 0, 0)
        _DistortionDir3("Distortion Direction 3", Vector) = (1, 1, 0, 0)

        _Alpha1("Alpha 1", Range(0,1)) = 1
        _Alpha2("Alpha 2", Range(0,1)) = 1
        _Alpha3("Alpha 3", Range(0,1)) = 1
    }

    SubShader
    {
        Tags 
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            // Основные директивы
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            // Включаем библиотеки URP
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Входные данные вертекса
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Данные, передаваемые из вершинного в пиксельный шейдер
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Текстуры и параметры
            TEXTURE2D(_Tex1); SAMPLER(sampler_Tex1);
            TEXTURE2D(_Tex2); SAMPLER(sampler_Tex2);
            TEXTURE2D(_Tex3); SAMPLER(sampler_Tex3);

            float _DistortionStrength1;
            float _DistortionStrength2;
            float _DistortionStrength3;

            float2 _DistortionDir1;
            float2 _DistortionDir2;
            float2 _DistortionDir3;

            float _Alpha1;
            float _Alpha2;
            float _Alpha3;

            // Вершинный шейдер
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            // Пиксельный шейдер
            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float time = _Time.y;

                // Вычисляем искажения
                float2 offset1 = sin(dot(uv, _DistortionDir1) * 50.0 + time * 10.0) * _DistortionDir1 * _DistortionStrength1;
                float2 offset2 = sin(dot(uv, _DistortionDir2) * 50.0 + time * 8.0) * _DistortionDir2 * _DistortionStrength2;
                float2 offset3 = sin(dot(uv, _DistortionDir3) * 50.0 + time * 6.0) * _DistortionDir3 * _DistortionStrength3;

                float4 col1 = SAMPLE_TEXTURE2D(_Tex1, sampler_Tex1, uv + offset1);
                float4 col2 = SAMPLE_TEXTURE2D(_Tex2, sampler_Tex2, uv + offset2);
                float4 col3 = SAMPLE_TEXTURE2D(_Tex3, sampler_Tex3, uv + offset3);

                // Модифицируем альфа-каналы
                col1.a *= _Alpha1;
                col2.a *= _Alpha2;
                col3.a *= _Alpha3;

                // Смешиваем
                float4 finalColor = float4(0, 0, 0, 0);
                finalColor.rgb += col1.rgb * col1.a;
                finalColor.rgb += col2.rgb * col2.a;
                finalColor.rgb += col3.rgb * col3.a;

                finalColor.a = saturate(col1.a + col2.a + col3.a); // можно изменить на поочередную перекрывающуюся альфу

                return finalColor;
            }

            ENDHLSL
        }
    }
}
