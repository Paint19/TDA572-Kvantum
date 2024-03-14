#version 330

out vec4 outputColor;

in vec2 texCoord;

// Lighting
in vec3 objectColor;              //The color of the object.
in vec3 normal;             
in vec3 FragPos;              
uniform vec3 lightColor;    //The color of the light.
uniform vec3 lightPos;      //The position of the light.
uniform vec3 viewPos;       //The position of the view and/or of the player.

uniform sampler2D texture0;

void main()
{
    float ambientStrength = 0.2;

    // Lighting calculations:
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPos - FragPos);  
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    vec3 ambient = ambientStrength * lightColor;
    
    vec3 result = (ambient + diffuse) * objectColor;
    outputColor = texture(texture0, texCoord) * vec4(result, 1.0f);
}