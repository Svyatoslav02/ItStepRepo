# API contract documentation

Purpose: Defines the contract for moodboard generation API.
Version: v1.0
Author: ITStep Student Team

This document outlines the API contract for the moodboard generation endpoint, 
detailing the request and response formats, as well as error handling.

## Endpoint

POST /api/generate

## Request Body
{
  "prompt": "string (required)",
  "style": "string (optional)"
}

## Description
prompt: The user’s text input for moodboard generation.
style: Optional parameter defining the visual style (e.g., "modern", "minimalist").

## Successful Response (200 OK)
{
  "prompt": "Generate moodboard for summer vibes",
  "images": [
    {
      "url": "https://example.com/image1.png",
      "title": "Beach photo",
      "sourceUrl": "https://unsplash.com/photos/abc123"
    },
    {
      "url": "https://example.com/image2.png",
      "title": "Sunset photo",
      "sourceUrl": "https://unsplash.com/photos/xyz456"
    }
  ]
}


## Description
id: Unique identifier of the generated moodboard.
images: Array of URLs pointing to generated images.
createdAt: Timestamp of creation.

## Error Responses
### 400 Bad Request
{
  "error": "Invalid prompt",
  "details": "Prompt cannot be empty"
}

### 500 Internal Server Error
{
  "error": "Generation failed",
  "details": "Unexpected error during image generation"
}

## Example
### Request
curl -X POST https://api.moodboard.com/api/generate \
  -H "Content-Type: application/json" \
  -d '{"prompt":"modern house japan"}'

### Response
{
  "prompt": "modern house japan",
  "images": [
    {
      "url": "https://example.com/image1.png",
      "title": "Beach photo",
      "sourceUrl": "https://unsplash.com/photos/abc123"
    },
    {
      "url": "https://example.com/image2.png",
      "title": "Sunset photo",
      "sourceUrl": "https://unsplash.com/photos/xyz456"
    }
  ],
}

