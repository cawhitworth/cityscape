float4x4 World;
float4x4 View;
float4x4 Projection;
float4 Ambient;
float4 Diffuse;
texture texRoad;
float LightDistance;

sampler2D smpRoad = sampler_state
{
  texture = <texRoad>;
  MinFilter = LINEAR;
  MagFilter = LINEAR;
  MipFilter = LINEAR;
  AddressU = WRAP;
  AddressV = WRAP;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 Tex0 : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Tex0 : TEXCOORD0;
    float  Fogging : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    output.Tex0 = input.Tex0;
    
    float dist = length(viewPosition);
    
    output.Fogging = 1 / (0.1f * dist);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 col = tex2D(smpRoad, input.Tex0);
    
    float4 fogColour = float4(0.0f, 0.0f, 0.1f, 0.0f);
    return lerp(fogColour, Ambient * col, input.Fogging);
}

technique DefaultTechnique
{
    pass Pass1
    {
        // TODO: set renderstates here.
		CullMode = None;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}
