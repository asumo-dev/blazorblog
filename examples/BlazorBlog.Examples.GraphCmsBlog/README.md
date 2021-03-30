This is an example project of using [GraphCMS](https://graphcms.com/) as a data source for BlazorBlog.

## Settings

Configure the following settings in `wwwroot/appsettings.json`.

```json
{
  "GraphCMS": {
    "Endpoint": "{ENDPOINT}",
    "ApiToken": "{API_TOKEN}"
  }
}
```

## GraphCMS settings

### Model

API ID: Post

Fields:

| API ID | Type | |
--- | --- | ---
| title | String | The title of a blog post |
| slug | String | The slug of a blog post |
| content | RichText | The body of a blog post |
