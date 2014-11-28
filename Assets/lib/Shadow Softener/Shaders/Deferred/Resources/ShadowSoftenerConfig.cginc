/*
 *	You can edit the Define below to customize how shadows are filtered in the Deferred Renderer.
 *	Valid filters are PCF2x2, PCF3x3, PCF4x4, up to PCF8x8
 *  You can use the optional "_DIRECTIONAL", "_SPOT", or "_POINT" suffix to override that particular light type.
 */


// Set the Default Shadow Filter
#define SOFTENER_FILTER PCF4x4

// Override the Directional Light Shadow Filter
// #define SOFTENER_FILTER_DIRECTIONAL PCF5x5

// Override the Spot Light Shadow Filter
// #define SOFTENER_FILTER_SPOT PCF3x3

// Override the Point Light Shadow Filter...
// The highest working Point Light filter is PCF7x7 (AKA: PCF48)
 #define SOFTENER_FILTER_POINT PCF7x7