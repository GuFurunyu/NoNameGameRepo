Shader "Custom/ScreenColorReplace"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            // 支持最多32组颜色
            #define MAX_COLOR_COUNT 32
            int _ColorCount;
            fixed4 _SourceColors[MAX_COLOR_COUNT];
            fixed4 _TargetColors[MAX_COLOR_COUNT];

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                for (int idx = 0; idx < _ColorCount; ++idx)
                {
                    // 颜色比较时允许微小误差
                    if (abs(col.r - _SourceColors[idx].r) < 0.01 &&
                    abs(col.g - _SourceColors[idx].g) < 0.01 &&
                    abs(col.b - _SourceColors[idx].b) < 0.01)
                    {
                        return fixed4(_TargetColors[idx].rgb, col.a);
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
