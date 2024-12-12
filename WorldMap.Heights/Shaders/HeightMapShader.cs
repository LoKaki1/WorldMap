using System;
using System.Numerics;
using WorldMap.Common.Factories.Interfaces;
using WorldMap.Common.Models.Enums;
using WorldMap.Common.Models.Shaders;
using WorldMap.Common.Shaders.Interfaces;
using WorldMap.Heights.TempCauseImLazy;

namespace WorldMap.Heights.Shaders;

public class HeightMapShader
{
    private readonly IShaderProgram m_ShaderProgram;

    public IShaderUniform<Matrix4x4> ModelViewProjection { get; }

    public HeightMapShader(IShaderFactory shaderFactory)
    {
        var vertexShaderParams = new ShaderParameters(VertexShader, ShadersTypes.Vertex);
        var vertexShader = shaderFactory.CreateShader(vertexShaderParams);
        var fragmentShaderParams = new ShaderParameters(FragmentShader, ShadersTypes.Fragmenet);
        var fragmentShader = shaderFactory.CreateShader(fragmentShaderParams);
        m_ShaderProgram = shaderFactory.CreateShaderProgram(new([vertexShader, fragmentShader]));
        ModelViewProjection
            = shaderFactory.CreateShaderUnfiorm<Matrix4x4>(new ShaderUniformParameters(m_ShaderProgram, "mvp"));
    }

    public void UseShader()
    {
        m_ShaderProgram.UseProgram();
    }

    public static string FragmentShader = @"
    #version 330
out vec4 gColor;

in vec2 vUV;
in vec2 vBary;
flat in float vBrightness;

uniform bool showWireframe = true;

float barycentric(vec2 vBC, float width)
{
    vec3 bary = vec3(vBC.x, vBC.y, 1.0 - vBC.x - vBC.y);
    vec3 d = fwidth(bary);
    vec3 a3 = smoothstep(d * (width - 0.5), d * (width + 0.5), bary);
    return min(min(a3.x, a3.y), a3.z);
}


void main()
{
    gColor = vec4(vBrightness);

    if (showWireframe)
    {
        gColor.rgb *= 0.25;
        gColor.rgb += vec3(1.0 - barycentric(vBary, 1.0));
    }
}
";

    public static string VertexShader = @$"
#version 330
    layout (location = 0) in float aY;

    out vec2 vUV;
    out vec2 vBary;
    flat out float vBrightness;

    uniform mat4 mvp;

    const float VERTICES_PER_RUN = " + Constants.VERTICES_PER_RUN + @".0;
    const float VERTICES_PER_RUN_NOT_DEGENERATE = " + Constants.VERTICES_PER_RUN_NOT_DEGENERATE + @".0;

    float rand3(vec3 c) { return fract(sin(dot(c.xyz, vec3(12.9898, 78.233, 133.719))) * 43758.5453);  }

    void main()
    {{    
        float runIndex = mod(gl_VertexID, VERTICES_PER_RUN);
        float clampedIndex = clamp(runIndex - 1.0, 0.0, VERTICES_PER_RUN_NOT_DEGENERATE); // First and last are degenerate
        float triangleSize = " + Constants.QUAD_SIZE + @".0;

        // X increments every 2 vertices
        float xPos = floor(clampedIndex / 2.0) * triangleSize;


        // Z increments every N vertices
        float zPos = floor(gl_VertexID / VERTICES_PER_RUN) * triangleSize;


        // Move every 2nd vertex 1 unit on the z axis, to create a triangle
        zPos += mod(clampedIndex, 2.0) * triangleSize;


        // Render to the screen
        vec3 pos = vec3(xPos, aY, zPos);
        gl_Position = mvp * vec4(pos, 1.0);


        // Random triangle brightness
        vBrightness = mix(0.5, 1.0, fract(rand3(pos) * gl_VertexID));


        // Calculate barycentric stuff for the wireframe
        int baryIndex = int(mod(clampedIndex, 3));

        if (baryIndex == 0)
            vBary = vec2(0.0, 0.0);
        else if (baryIndex == 1)
            vBary = vec2(0.0, 1.0);
        else
            vBary = vec2(1.0, 0.0);
    }}
";
}
