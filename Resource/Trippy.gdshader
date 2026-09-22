shader_type spatial;
render_mode unshaded, fog_disabled;


// ============================================================
// SCREEN
// ============================================================

uniform sampler2D screen_texture
	: source_color, hint_screen_texture, filter_linear;


// ============================================================
// WATERCOLOR TEXTURE
// ============================================================

uniform sampler2D noise_texture
	: source_color, repeat_enable, filter_linear;


// ============================================================
// MASTER
// ============================================================

uniform float watercolor_strength
	: hint_range(0.0, 1.0, 0.01) = 0.85;


// ============================================================
// COLOR / PIGMENT
// ============================================================

uniform float tonal_compression
	: hint_range(0.0, 1.0, 0.01) = 0.35;

uniform float pigment_saturation
	: hint_range(0.0, 2.0, 0.01) = 0.35;

uniform float shadow_lift
	: hint_range(0.0, 1.0, 0.01) = 0.05;


// ============================================================
// WASH
// ============================================================

uniform float wash_strength
	: hint_range(0.0, 1.0, 0.01) = 0.25;

uniform float wash_scale
	: hint_range(0.001, 0.2, 0.001) = 0.015;


// ============================================================
// BLEEDING
// ============================================================

uniform float bleed_strength
	: hint_range(0.0, 1.0, 0.01) = 0.60;

uniform float bleed_radius
	: hint_range(0.1, 8.0, 0.1) = 2.0;


// ============================================================
// PIGMENT POOLING
// ============================================================

uniform float pooling_strength
	: hint_range(0.0, 1.0, 0.01) = 0.40;

uniform float pooling_width
	: hint_range(0.001, 0.5, 0.001) = 0.08;


// ============================================================
// BLOOMS
// ============================================================

uniform float bloom_strength
	: hint_range(0.0, 1.0, 0.01) = 0.25;

uniform float bloom_scale
	: hint_range(0.001, 0.2, 0.001) = 0.025;


// ============================================================
// GRANULATION
// ============================================================

uniform float granulation_strength
	: hint_range(0.0, 1.0, 0.01) = 0.08;

uniform float granulation_scale
	: hint_range(0.1, 20.0, 0.01) = 4.0;


// ============================================================
// PAPER
// ============================================================

uniform float paper_brightness
	: hint_range(0.0, 1.0, 0.01) = 0.12;


// ============================================================
// FULLSCREEN QUAD
// ============================================================

void vertex()
{
	POSITION = vec4(
		VERTEX.xy,
		1.0,
		1.0
	);
}


// ============================================================
// LUMINANCE
// ============================================================

float luminance(vec3 color)
{
	return dot(
		color,
		vec3(
			0.299,
			0.587,
			0.114
		)
	);
}


// ============================================================
// SATURATION
// ============================================================

vec3 adjust_saturation(
	vec3 color,
	float amount
)
{
	float luma = luminance(color);

	return mix(
		vec3(luma),
		color,
		1.0 + amount
	);
}


// ============================================================
// SCREEN BLUR
//
// Pixel size is passed into the function instead of accessing
// VIEWPORT_SIZE from inside the helper.
// ============================================================

vec3 sample_blur(
	vec2 uv,
	vec2 pixel_size,
	float radius
)
{
	vec3 result = vec3(0.0);

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(-2.0, -2.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(0.0, -2.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(2.0, -2.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(-2.0, 0.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(2.0, 0.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(-2.0, 2.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(0.0, 2.0) * radius
	).rgb;

	result += texture(
		screen_texture,
		uv + pixel_size * vec2(2.0, 2.0) * radius
	).rgb;

	return result / 9.0;
}


// ============================================================
// EDGE DETECTION
// ============================================================

float get_edge_strength(
	vec2 uv,
	vec2 pixel_size
)
{
	float center = luminance(
		texture(
			screen_texture,
			uv
		).rgb
	);

	float left = luminance(
		texture(
			screen_texture,
			uv + vec2(-pixel_size.x, 0.0)
		).rgb
	);

	float right = luminance(
		texture(
			screen_texture,
			uv + vec2(pixel_size.x, 0.0)
		).rgb
	);

	float up = luminance(
		texture(
			screen_texture,
			uv + vec2(0.0, -pixel_size.y)
		).rgb
	);

	float down = luminance(
		texture(
			screen_texture,
			uv + vec2(0.0, pixel_size.y)
		).rgb
	);

	float horizontal =
		abs(left - right);

	float vertical =
		abs(up - down);

	float edge =
		horizontal +
		vertical;

	return smoothstep(
		0.01,
		0.15,
		edge
	);
}


// ============================================================
// NOISE
//
// Screen-space noise is intentionally used here only as a
// broad wash/pigment pattern. It is NOT the main effect.
// ============================================================

float sample_noise(
	vec2 uv,
	float scale
)
{
	return texture(
		noise_texture,
		uv * scale
	).r;
}


// ============================================================
// FRAGMENT
// ============================================================

void fragment()
{
	// --------------------------------------------------------
	// Pixel size
	// --------------------------------------------------------

	vec2 pixel_size =
		1.0 / VIEWPORT_SIZE;


	// --------------------------------------------------------
	// Original image
	// --------------------------------------------------------

	vec2 uv =
		SCREEN_UV;

	vec3 original =
		texture(
			screen_texture,
			uv
		).rgb;

	vec3 color =
		original;


	// ========================================================
	// 1. TONAL COMPRESSION
	//
	// Reduce the harshness of the original rendered image.
	// ========================================================

	float original_luma =
		luminance(color);

	float compressed_luma =
		mix(
			original_luma,
			smoothstep(
				0.0,
				1.0,
				original_luma
			),
			tonal_compression
		);

	float ratio =
		compressed_luma /
		max(
			original_luma,
			0.001
		);

	color *= ratio;


	// ========================================================
	// 2. SHADOW LIFT
	// ========================================================

	float dark_mask =
		1.0 -
		smoothstep(
			0.15,
			0.65,
			original_luma
		);

	color +=
		vec3(
			0.035
		) *
		dark_mask *
		shadow_lift;


	// ========================================================
	// 3. PIGMENT SATURATION
	// ========================================================

	color =
		adjust_saturation(
			color,
			pigment_saturation
		);


	// ========================================================
	// 4. LARGE WATERCOLOUR WASH
	//
	// Large irregular pigment concentration.
	// ========================================================

	float wash_1 =
		sample_noise(
			uv,
			1.0 / wash_scale
		);

	float wash_2 =
		sample_noise(
			uv * 0.43 +
			vec2(
				17.3,
				31.7
			),
			1.0 / wash_scale
		);

	float wash =
		mix(
			wash_1,
			wash_2,
			0.5
		);

	wash =
		wash * 2.0 -
		1.0;

	// Reduce the visibility of the noise itself.
	float wash_amount =
		wash *
		wash_strength *
		0.30;

	color +=
		color *
		wash_amount;


	// ========================================================
	// 5. COLOR BLEED
	//
	// This is intentionally strong.
	// ========================================================

	vec3 blur_1 =
		sample_blur(
			uv,
			pixel_size,
			bleed_radius
		);

	vec3 blur_2 =
		sample_blur(
			uv,
			pixel_size,
			bleed_radius * 2.5
		);

	vec3 bleed =
		mix(
			blur_1,
			blur_2,
			0.35
		);

	float edge =
		get_edge_strength(
			uv,
			pixel_size
		);

	// Broad wash everywhere, slightly less directly on
	// hard edges.
	float bleed_mask =
		bleed_strength *
		(
			0.30 +
			(1.0 - edge) *
			0.70
		);

	color =
		mix(
			color,
			bleed,
			bleed_mask
		);


	// ========================================================
	// 6. PIGMENT POOLING
	//
	// Accentuate pigment around boundaries.
	// ========================================================

	float pooling =
		smoothstep(
			0.0,
			pooling_width,
			edge
		);

	float current_luma =
		luminance(color);

	float pigment_mask =
		1.0 -
		smoothstep(
			0.20,
			0.75,
			current_luma
		);

	float pool =
		pooling *
		pigment_mask *
		pooling_strength;

	// Dark pigment deposition.
	color *=
		1.0 -
		pool * 0.28;

	// Slight saturation increase where pigment pools.
	color =
		adjust_saturation(
			color,
			pool * 0.55
		);


	// ========================================================
	// 7. BLOOMS
	//
	// Medium-scale variation in pigment density.
	// ========================================================

	float bloom_1 =
		sample_noise(
			uv,
			1.0 / bloom_scale
		);

	float bloom_2 =
		sample_noise(
			uv * 0.47 +
			vec2(
				47.2,
				11.3
			),
			1.0 / bloom_scale
		);

	float bloom =
		mix(
			bloom_1,
			bloom_2,
			0.5
		);

	bloom =
		bloom * 2.0 -
		1.0;

	current_luma =
		luminance(color);

	float bloom_mask =
		1.0 -
		smoothstep(
			0.20,
			0.80,
			current_luma
		);

	color +=
		color *
		bloom *
		bloom_strength *
		bloom_mask *
		0.35;


	// ========================================================
	// 8. GRANULATION
	//
	// Fine pigment deposits.
	//
	// This is intentionally VERY subtle.
	// ========================================================

	float grain_1 =
		sample_noise(
			uv,
			granulation_scale
		);

	float grain_2 =
		sample_noise(
			uv * 2.7 +
			vec2(
				13.1,
				71.4
			),
			granulation_scale
		);

	float grain =
		mix(
			grain_1,
			grain_2,
			0.35
		);

	grain -= 0.5;

	current_luma =
		luminance(color);

	float grain_mask =
		(
			1.0 -
			smoothstep(
				0.15,
				0.80,
				current_luma
			)
		) *
		granulation_strength;

	color +=
		grain *
		grain_mask *
		0.10;


	// ========================================================
	// 9. PAPER LUMINOSITY
	// ========================================================

	current_luma =
		luminance(color);

	float paper_mask =
		smoothstep(
			0.50,
			0.95,
			current_luma
		);

	vec3 paper_color =
		vec3(
			1.0,
			0.985,
			0.94
		);

	color =
		mix(
			color,
			mix(
				color,
				paper_color,
				0.14
			),
			paper_mask *
			paper_brightness
		);


	// ========================================================
	// 10. FINAL MIX
	// ========================================================

	color =
		mix(
			original,
			color,
			watercolor_strength
		);


	// ========================================================
	// OUTPUT
	// ========================================================

	ALBEDO =
		clamp(
			color,
			0.0,
			1.0
		);
}