#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aColor;
layout(location = 3) in vec3 aNormal;

out vec2 texCoord;
out vec3 objectColor;
out vec3 normal;
out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    texCoord = aTexCoord;
    objectColor = aColor;
    normal = aNormal * mat3(transpose(inverse(model))); // This inversem model thing should be calculated on the CPU instead
    FragPos = vec3(model * vec4(aPosition, 1.0));
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}
