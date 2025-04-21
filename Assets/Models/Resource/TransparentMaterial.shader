Shader "Custom/TransparentEmissionMaterial"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 0, 0.5) // ������ ���� � 50% ������������
        _EmissionColor ("Emission Color", Color) = (1, 1, 0, 1) // ������ ���� ��������
        _EmissionStrength ("Emission Strength", Range(0, 10)) = 1.0 // ������������� ��������
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha // ������������
        ZWrite Off                     // ��������� ������ � ����� ������� ��� ������������

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;
            float4 _EmissionColor;
            float _EmissionStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // �������� ���� (� �������������)
                fixed4 baseColor = _Color;

                // ����������� ����
                fixed4 emissionColor = _EmissionColor * _EmissionStrength;

                // ��������� ���� (�������� + ��������)
                return baseColor + emissionColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
