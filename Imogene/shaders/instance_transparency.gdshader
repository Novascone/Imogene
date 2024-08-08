shader_type spatial;
render_mode depth_prepass_alpha; 

instance uniform float opacity:hint_range(0.0, 1.0) = 1.0;
uniform sampler2D _abeldo:source_color;

void fragment() {
	ALBEDO = texture(_abeldo, UV).rbg;
	ALPHA = opacity;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
