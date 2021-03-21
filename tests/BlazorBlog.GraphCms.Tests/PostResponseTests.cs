using System;
using Newtonsoft.Json;
using Xunit;

namespace BlazorBlog.GraphCms.Tests
{
    public class PostResponseTests
    {
        [Fact]
        public void PostResponseCanBeDeserializedFromGraphCmsJson()
        {
            var graphCmsJson = @"
{
    ""post"": {
      ""title"": ""Union Types and Sortable Relations with GraphCMS"",
      ""slug"": ""union-types-and-sortable-relations"",
      ""content"": {
        ""html"": ""<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Id enim natura desiderat. Falli igitur possumus. Negat enim summo bono afferre incrementum diem. Indicant pueri, in quibus ut in speculis natura cernitur.\r</p><p></p><h1>Lorem Ipsum\r</h1><p></p><p>Ne amores quidem sanctos a sapiente alienos esse arbitrantur. Summus dolor plures dies manere non potest? Expectoque quid ad id, quod quaerebam, respondeas. Non est ista, inquam, Piso, magna dissensio. Respondeat totidem verbis. Non est igitur summum malum dolor.\r</p><p>\r</p><p>Hic ambiguo ludimur. Nam Pyrrho, Aristo, Erillus iam diu abiecti. Si longus, levis dictata sunt. Duo Reges: constructio interrete. Deinde dolorem quem maximum?</p>""
      },
      ""publishedAt"": ""2020-05-19T10:14:15.678847+00:00""
    }
}
";
            
            var expected = new PostResponse
            {
                Post = new PostContent
                {
                    Title = "Union Types and Sortable Relations with GraphCMS",
                    Slug = "union-types-and-sortable-relations",
                    Content = new PostContent.ContentContent
                    {
                        Html = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Id enim natura desiderat. Falli igitur possumus. Negat enim summo bono afferre incrementum diem. Indicant pueri, in quibus ut in speculis natura cernitur.\r</p><p></p><h1>Lorem Ipsum\r</h1><p></p><p>Ne amores quidem sanctos a sapiente alienos esse arbitrantur. Summus dolor plures dies manere non potest? Expectoque quid ad id, quod quaerebam, respondeas. Non est ista, inquam, Piso, magna dissensio. Respondeat totidem verbis. Non est igitur summum malum dolor.\r</p><p>\r</p><p>Hic ambiguo ludimur. Nam Pyrrho, Aristo, Erillus iam diu abiecti. Si longus, levis dictata sunt. Duo Reges: constructio interrete. Deinde dolorem quem maximum?</p>"
                    },
                    PublishedAt = new DateTime(
                        new DateTime(2020, 5, 19, 10, 14, 15, DateTimeKind.Utc).Ticks + 6788470,
                        DateTimeKind.Utc)
                }
            };

            // Act
            var result = JsonConvert.DeserializeObject<PostResponse>(graphCmsJson);

            Assert.Equal(expected, result);
        }
    }
}
