# API Contract

## POST /api/generate

### Description

This endpoint accepts a text prompt and returns a mock moodboard response with images.

The current implementation returns static mock image data. This endpoint is used by the frontend to generate and display a moodboard based on the user's text request.

---

## Request

### Body

```json
{
  "prompt": "minimalism, japanese style, beige green tones"
}
```

### Request fields

| Field  | Type   | Required | Description                                                      |
| ------ | ------ | -------- | ---------------------------------------------------------------- |
| prompt | string | Yes      | Text prompt entered by the user. Minimum length is 3 characters. |

---

## Success Response

### Status code

```txt
200 OK
```

### Body

```json
{
  "prompt": "minimalism, japanese style, beige green tones",
  "images": [
    {
      "url": "https://example.com/image1.jpg",
      "title": "Minimalist interior",
      "sourceUrl": "https://example.com"
    }
  ]
}
```

### Response fields

| Field            | Type   | Description                                |
| ---------------- | ------ | ------------------------------------------ |
| prompt           | string | Original user prompt.                      |
| images           | array  | List of generated moodboard image objects. |
| images.url       | string | Direct image URL.                          |
| images.title     | string | Image title or description.                |
| images.sourceUrl | string | Source URL of the image.                   |

---

## Error Response

### Validation Error

Returned when the request body is invalid or the prompt does not pass validation.

### Status code

```txt
400 Bad Request
```

### Body

```json
{
  "message": "Prompt is required."
}
```

Another possible validation response:

```json
{
  "message": "Prompt must be at least 3 characters long."
}
```

---

## Usage Example

### Request

```http
POST /api/generate
Content-Type: application/json
```

```json
{
  "prompt": "modern office design"
}
```

### Response

```json
{
  "prompt": "modern office design",
  "images": [
    {
      "url": "https://example.com/image1.jpg",
      "title": "Minimalist interior",
      "sourceUrl": "https://example.com"
    }
  ]
}
```

---

## Notes

Future versions may extend this endpoint to return dynamically generated images. The current version provides static mock data to support frontend integration and testing.
