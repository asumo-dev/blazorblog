using System;
using Newtonsoft.Json;
using Xunit;

namespace BlazorBlog.GraphCms.Tests
{
    public class PagedPostResponseTests
    {
        [Fact]
        public void PagedPostResponseCanBeDeserializedFromGraphCmsJson()
        {
            var graphCmsJson = @"
{
    ""postsConnection"": {
      ""aggregate"": {
        ""count"": 4
      },
      ""edges"": [
        {
          ""node"": {
            ""title"": ""Union Types and Sortable Relations with GraphCMS"",
            ""slug"": ""union-types-and-sortable-relations"",
            ""publishedAt"": ""2020-05-19T10:14:15.678847+00:00"",
            ""content"": {
              ""html"": ""<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Id enim natura desiderat. Falli igitur possumus. Negat enim summo bono afferre incrementum diem. Indicant pueri, in quibus ut in speculis natura cernitur.\r</p><p></p><h1>Lorem Ipsum\r</h1><p></p><p>Ne amores quidem sanctos a sapiente alienos esse arbitrantur. Summus dolor plures dies manere non potest? Expectoque quid ad id, quod quaerebam, respondeas. Non est ista, inquam, Piso, magna dissensio. Respondeat totidem verbis. Non est igitur summum malum dolor.\r</p><p>\r</p><p>Hic ambiguo ludimur. Nam Pyrrho, Aristo, Erillus iam diu abiecti. Si longus, levis dictata sunt. Duo Reges: constructio interrete. Deinde dolorem quem maximum?</p>""
            }
          }
        },
        {
          ""node"": {
            ""title"": ""Connecting Multiple Platforms Together"",
            ""slug"": ""connecting-multiple-platforms"",
            ""publishedAt"": ""2020-05-19T10:15:43.697582+00:00"",
            ""content"": {
              ""html"": ""<h1>Nihil ad rem! Ne sit sane;\r</h1><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Apparet statim, quae sint officia, quae actiones. Illis videtur, qui illud non dubitant bonum dicere -; Deinde dolorem quem maximum?\r</p><p>\r</p><h2>An vero, inquit, quisquam potest probare, quod perceptfum, quod.\r</h2><p>Omnes enim iucundum motum, quo sensus hilaretur. Non quam nostram quidem, inquit Pomponius iocans; Utilitatis causa amicitia est quaesita.\r</p><p>\r</p><h3>Duo Reges: constructio interrete.\r</h3><p>Qui ita affectus, beatum esse numquam probabis; Quae cum dixisset paulumque institisset, Quid est? Efficiens dici potest.</p>""
            }
          }
        }
      ],
      ""pageInfo"": {
        ""pageSize"": 2
      }
    }
}
";

            var expected = new PagedPostsResponse
            {
                PostsConnection = new PagedPostsResponse.PostsConnectionContent
                {
                    Aggregate = new PagedPostsResponse.PostsConnectionContent.AggregateContent
                    {
                        Count = 4
                    },
                    Edges = new PagedPostsResponse.PostsConnectionContent.EdgeContent[]
                    {
                        new()
                        {
                            Node = new PostContent
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
                        },
                        new()
                        {
                            Node = new PostContent
                            {
                               Title = "Connecting Multiple Platforms Together",
                               Slug = "connecting-multiple-platforms",
                               Content = new PostContent.ContentContent
                               {
                                   Html = "<h1>Nihil ad rem! Ne sit sane;\r</h1><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Apparet statim, quae sint officia, quae actiones. Illis videtur, qui illud non dubitant bonum dicere -; Deinde dolorem quem maximum?\r</p><p>\r</p><h2>An vero, inquit, quisquam potest probare, quod perceptfum, quod.\r</h2><p>Omnes enim iucundum motum, quo sensus hilaretur. Non quam nostram quidem, inquit Pomponius iocans; Utilitatis causa amicitia est quaesita.\r</p><p>\r</p><h3>Duo Reges: constructio interrete.\r</h3><p>Qui ita affectus, beatum esse numquam probabis; Quae cum dixisset paulumque institisset, Quid est? Efficiens dici potest.</p>"
                               },
                               PublishedAt = new DateTime(
                                   new DateTime(2020, 5, 19, 10, 15, 43, DateTimeKind.Utc).Ticks + 6975820,
                                   DateTimeKind.Utc)
                            }
                        }
                    }
                }
            };

            // Act
            var result = JsonConvert.DeserializeObject<PagedPostsResponse>(graphCmsJson);

            Assert.Equal(expected.PostsConnection.Aggregate, result.PostsConnection.Aggregate);
            Assert.Equal(expected.PostsConnection.Edges, result.PostsConnection.Edges);
        }
    }
}
