using System;
using System.Collections.Generic;

namespace ComunityDetectorServices.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string PlublishedIn { get; set; }
        public DateTime PublishDate { get; set; }
        public List<Author> Authors { get; set; }
        public List<ArticleReference> References { get; set; }
    }
}
