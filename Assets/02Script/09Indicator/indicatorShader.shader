Shader "Custom/Indicator"
{
    Properties
    {
        _FillColor("Fill Color", Color) = (1, 0, 0, 0.3)
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineThickness("Outline Thickness", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _FillColor;
            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 posXZ : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posXZ = v.vertex.xz; // XZ Æò¸é
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.posXZ);
                if (dist > 1)
                    discard;

                if (dist > 1 - _OutlineThickness)
                    return _OutlineColor;

                return _FillColor;
            }
            ENDCG
        }
    }
}
