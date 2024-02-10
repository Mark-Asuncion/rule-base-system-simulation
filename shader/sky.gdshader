shader_type sky;
uniform sampler2D gradient;
uniform float ratio: hint_range(0.0, 1.0, 0.01) = 0;
void sky() {
	vec4 color = texture(gradient, vec2(ratio, 0.5));
	vec4 color2 = texture(gradient, vec2(ratio + 0.1, 0.5));
	vec3 fcolor = mix(color2.rgb,color.rgb, EYEDIR.y);
	COLOR = fcolor;
}
