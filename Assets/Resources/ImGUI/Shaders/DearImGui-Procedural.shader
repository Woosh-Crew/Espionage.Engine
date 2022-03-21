Shader "DearImGui/Procedural"
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
            Name "DEARIMGUI PROCEDURAL BUILTIN"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment ImGuiPassFrag
            #include "./PassesBuiltin.hlsl"

            StructuredBuffer<ImVert> _Vertices;
            int _BaseVertex;

            Varyings vert(uint id : SV_VertexID)
            {
#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE)
                // BaseVertexLocation is not automatically added to SV_VertexID
                id += _BaseVertex;
#endif
                ImVert v = _Vertices[id];
                return ImGuiPassVertex(v);
            }
            ENDCG
        }
    }
}
