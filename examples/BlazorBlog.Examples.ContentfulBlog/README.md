This is an example project of using [Contentful](https://www.contentful.com/) as a data source for BlazorBlog.

## Settings

Configure the following settings in `wwwroot/appsettings.json`.

```json
{
  "Contentful": {
    "DeliveryApiKey": "{DELIVERY_API_KEY}",
    "SpaceId": "{SPACE_ID}"
  }
}
```

## Contentful settings

### Contet type

Api Identifier: blogPost

Fields:

| Field ID | Type | |
--- | --- | ---
| title | Short text | The title of a blog post |
| slug | Short text | The slug of a blog post |
| body | Rich text | The body of a blog post |
