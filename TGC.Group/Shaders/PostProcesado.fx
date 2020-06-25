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

float screenWidth, screenHeight;
float timer;

static const int kernelRadius = 5;
static const int kernelSize = 25;
static const float kernel[kernelSize] =
{
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.023792, 0.094907, 0.150342, 0.094907, 0.023792,
    0.015019, 0.059912, 0.094907, 0.059912, 0.015019,
    0.003765, 0.015019, 0.023792, 0.015019, 0.003765,
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

//Input del Vertex Shader
struct VS_INPUT_DEFAULT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT_DEFAULT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
    float4 PosView : COLOR0;
};

//Vertex Shader
VS_OUTPUT_DEFAULT VSDefault(VS_INPUT_DEFAULT input)
{
    VS_OUTPUT_DEFAULT output;

	// Enviamos la posicion transformada
    output.Position = mul(input.Position, matWorldViewProj);
    
    // Propagar las normales por la matriz normal
    output.Normal = mul(input.Normal, matInverseTransposeWorld);
    
	// Propagar coordenadas de textura
    output.TextureCoordinates = input.TextureCoordinates;
    
    output.PosView = mul(input.Position, matWorldView);

    return output;
}

//Pixel Shader
float4 PSFogEffect(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float startFogDistance = 3500;
    float endFogDistance = 4000;
    float4 ColorFog = float4(0.5, 0.5, 0.5, 1);
    
    float4 fvBaseColor = tex2D(diffuseMap, input.TextureCoordinates);
    
    if (fvBaseColor.a < 0.5)
    {
        discard;
    }
    
    if (input.PosView.z < startFogDistance)
        // Si estoy adentro del startFogDistance
        // muestro el color original de la textura
        return fvBaseColor;
    else if (input.PosView.z > endFogDistance)
    {
        // Si estoy fuera del endFogDistance
        // muestro el color gris
        fvBaseColor = ColorFog;
        return fvBaseColor;
    }
    else
    {
		// combino fog y textura
        float1 total = endFogDistance - startFogDistance;
        float1 resto = input.PosView.z - startFogDistance;
        float1 proporcion = resto / total;
        fvBaseColor = lerp(fvBaseColor, ColorFog, proporcion);
        
        return fvBaseColor;
    }
    
    
    /*
    // combino fog y textura
    float1 total = endFogDistance - startFogDistance;
    float1 resto = input.PosView.z - startFogDistance;
    float1 proporcion = resto / total;
    float4 fvBaseColor2 = lerp(fvBaseColor, ColorFog, proporcion);
    
    return (input.PosView.z <= startFogDistance) ? fvBaseColor : fvBaseColor2;*/

    

}

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
float4 PSPostProcessMonster(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    float borde = distance(input.TextureCoordinates, float2(0.5, 0.5));
    
    int pixelX = round(input.TextureCoordinates.x * screenWidth);

    int porcentajeX = 5 * tan(timer * 3);
    int lineasX = int(pixelX) % porcentajeX;
    
    
    int porcentajeX1 = 5 * tan(timer * 5);
    int lineasX1 = int(pixelX) % porcentajeX;
        
    if (lineasX <= 1.0)
    {
        tex = (lineasX <= 1.0) * tex;
        tex = lerp(tex, float4(0, 0, 0, 1), 0.6);
        return lerp(tex, float4(0, 0, 0, 1), borde * 1.25);
    }
    else
    {
        tex = (lineasX1 <= 1.0) * tex;
        tex = lerp(tex, float4(1, 1, 1, 1), 0.6);
        return lerp(tex, float4(0, 0, 0, 1), borde * 1.25);
    }
}

//Pixel Shader
float4 PSPostProcessNightVision(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    float bordeDerecho = distance(input.TextureCoordinates, float2(0.25, 0.5));
    float bordeIzquierdo = distance(input.TextureCoordinates, float2(0.75, 0.5));
    
    int pixelY = round(input.TextureCoordinates.y * screenHeight);
    
    int lineasY = int(pixelY) % 5;
    
    tex = (lineasY <= 1.0) * tex;
    tex = lerp(tex, float4(0, 0.5, 0, 1), 0.6);
    
    return (input.TextureCoordinates.x < 0.5) ? lerp(tex, float4(0, 0, 0, 1), bordeDerecho * 2) :
    lerp(tex, float4(0, 0, 0, 1), bordeIzquierdo * 2);
}

float4 PSPostProcessDefault(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float4 tex =  tex2D(renderTargetSampler, input.TextureCoordinates);
    return lerp(tex, float4(0, 0, 0, 1), 0.7);
}

float4 PSPostProcessLinterna(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float borde = distance(float2(input.TextureCoordinates.x, input.TextureCoordinates.y / 2), float2(0.5, 0.2));
    
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    return lerp(tex, float4(0, 0, 0, 1), borde * 2.5);
}


float4 PSPostProcessVela(VS_OUTPUT_DEFAULT input) : COLOR0
{
    float borde = distance(float2(input.TextureCoordinates.x, input.TextureCoordinates.y / 2), float2(0.5, 0.2));
    
    float4 tex = tex2D(renderTargetSampler, input.TextureCoordinates);
    
    tex = lerp(tex, float4(0.6,0.2,0,1), 0.3);
    
    return lerp(tex, float4(0, 0, 0, 1), borde * 2.5);
}


technique FogEffect
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSDefault();
        PixelShader = compile ps_3_0 PSFogEffect();
    }
}

technique PostProcessDefault
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessDefault();
    }
}

technique PostProcessVela
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessVela();
    }
}

technique PostProcessLinterna
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessLinterna();
    }
}

technique PostProcessMonster
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessMonster();
    }
}

technique PostProcessNightVision
{
    pass Pass_0
    {
        VertexShader = compile vs_3_0 VSPostProcess();
        PixelShader = compile ps_3_0 PSPostProcessNightVision();
    }
}