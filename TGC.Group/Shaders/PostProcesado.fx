/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

float3 eyePosition;

struct Light
{
	float3 position;
	float3 color;
};

Light lights[2];

float screenWidth, screenHeight, timer = 0.0;

static const int kernelRadius = 5;
static const int kernelSize = 25;
static const float kernel[kernelSize] =
{
	0.003765,   0.015019,	0.023792,	0.015019,	0.003765,
	0.015019,	0.059912,	0.094907,	0.059912,	0.015019,
	0.023792,	0.094907,	0.150342,	0.094907,	0.023792,
	0.015019,	0.059912,	0.094907,	0.059912,	0.015019,
	0.003765,	0.015019,	0.023792,	0.015019,	0.003765,
};


//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

//Textura para full screen quad
texture renderTarget;
sampler2D renderTargetSampler = sampler_state
{
	Texture = (renderTarget);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};
//Textura
texture textureExample;
sampler2D textureSampler = sampler_state
{
    Texture = (textureExample);
    ADDRESSU = MIRROR;
    ADDRESSV = MIRROR;
	MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};


float distanceToShape(int sides, float2 position)
{
    float angle = atan2(position.y, position.x) + 3.14;
    float radius = 3.14 * 2.0 / float(sides);

    return cos(floor(.5 + angle / radius) * radius - angle) * length(position);
}

float2 random2(float2 input)
{
    return frac(sin(float2(dot(input, float2(127.1, 311.7)), dot(input, float2(269.5, 183.3)))) * 43758.5453);
}

float2 rotate2D(float2 position, float angle)
{
    position -= 0.5;
    position = mul(float2x2(cos(angle), -sin(angle),
						    sin(angle), cos(angle)), position);
    position += 0.5;
    return position;
}


//Input del Vertex Shader
struct VS_INPUT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
	float3 MeshPosition : TEXCOORD1;
};

//Input del Vertex Shader
struct VS_INPUT_POSTPROCESS
{
	float4 Position : POSITION0;
	float2 TextureCoordinates : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_POSTPROCESS
{
	float4 Position : POSITION0;
	float2 TextureCoordinates : TEXCOORD0;
};

//Vertex Shader
VS_OUTPUT_POSTPROCESS VSPostProcess(VS_INPUT_POSTPROCESS input)
{
	VS_OUTPUT_POSTPROCESS output;
	// Propagamos la posicion, ya que esta en espacio de pantalla
	output.Position = input.Position;
	// Propagar coordenadas de textura
	output.TextureCoordinates = input.TextureCoordinates;
	return output;
}

//Pixel Shader
float4 PSPostProcess(VS_INPUT_POSTPROCESS input) : COLOR0
{
	float4 color = tex2D(renderTargetSampler, input.TextureCoordinates);
	float4 rojo = (1,1,1,1);
	float borde = distance(input.TextureCoordinates, (0.5, 0.5));
	return lerp(color, rojo, borde);
}

technique PostProcess
{
	pass Pass_0
	{
		VertexShader = compile vs_3_0 VSPostProcess();
		PixelShader = compile ps_3_0 PSPostProcess();
	}
}
