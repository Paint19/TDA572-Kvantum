#version 330

out vec4 outputColor;

in vec2 texCoord;

// Lighting
in vec3 color;              //The color of the object.
uniform vec3 lightColor;    //The color of the light.
uniform vec3 lightPos;      //The position of the light.
uniform vec3 viewPos;       //The position of the view and/or of the player.

uniform sampler2D texture0;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 result = ambient * color;
    outputColor = texture(texture0, texCoord) * vec4(result, 1.0f);
}