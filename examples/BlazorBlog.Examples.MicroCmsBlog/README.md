This is an example project of using [microCMS](https://microcms.io/) (JP) as a data source for BlazorBlog.

## Settings

Configure the following settings in `wwwroot/appsettings.json`.

```json
{
  "MicroCms": {
    "ApiKey": "{API_KEY}",
    "Endpoint": "https://{SERVICE_NAME}.microcms.io/api/v1/{ENTITY_NAME}"
  }
}
```

image

## microCMS settings

### Content type

Fields:

| Field ID | Type | |
--- | --- | ---
| title | Text field | The title of a blog post |
| slug | Text field | The slug of a blog post |
| body | Rich edit | The body of a blog post |
