Shader "Custom/GradientShader"
{
    Properties
    {
        _ColorTop ("Color Top", Color) = (1,1,1,1)
        _ColorBottom ("Color Bottom", Color) = (0,0,0,1)
        _Alpha("Alpha", Float) = 1.0 // Alpha 속성 추가
    	
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f_t {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _ColorTop;
            fixed4 _ColorBottom;
			float _Alpha; // Alpha 변수 추가

			v2f_t vert(appdata_t IN)
			{
				v2f_t OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv.x;

				return OUT;
			}

			fixed4 frag(v2f_t IN) : SV_Target 
			{ 
				fixed4 final_color = lerp(_ColorBottom,_ColorTop ,IN.uv.x);
				final_color.a *= _Alpha; // 최종 알파값 설정.
				
				return final_color; 
			} 

           ENDCG

       }
   } 
}
