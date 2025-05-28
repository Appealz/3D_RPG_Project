Shader "Custom/OnlyCircle"
{
    Properties
    {
        _Color("Color", Color) = (1, 0, 0, 1)
        _Radius("Radius", Float) = 0.4
        _Softness("Softness", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            AlphaTest Greater 0 // Alpha 0 이하는 그리지 않음

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Radius;
            float _Softness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                float alpha = smoothstep(_Radius, _Radius - _Softness, dist);

                if (alpha >= 1)
                    discard;

                return fixed4(_Color.rgb, (1 - alpha) * _Color.a);
            }
            ENDCG
        }
    }
    FallBack Off
}
