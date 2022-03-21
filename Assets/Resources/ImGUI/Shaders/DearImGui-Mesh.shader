Shader "DearImGui/Mesh"
{
    // shader for builtin rendering
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        LOD 100

        Lighting Off
        Cull Off ZWrite On ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "DEARIMGUI BUILTIN"

            CGPROGRAM
            #pragma vertex ImGuiPassVertex
            #pragma fragment ImGuiPassFrag
            #include "./PassesBuiltin.hlsl"
            ENDCG
        }
    }
}
