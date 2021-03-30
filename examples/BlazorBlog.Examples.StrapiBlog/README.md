This is an example project of using [Strapi](https://strapi.io/) as a data source for BlazorBlog.

## Settings

Configure the following settings in `wwwroot/appsettings.json`.

```json
{
  "Strapi": {
    "BaseUrl": "{BASE_URL}",
    "ContentName": "{CONTENT_NAME}"
  }
}
```

## Strapi settings

### Content type

Fields:

| Name | Type | |
--- | --- | ---
| title | Text | The title of a blog post |
| slug | Text | The slug of a blog post |
| cody | Rich text | The body of a blog post |
