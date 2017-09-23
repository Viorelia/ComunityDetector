using System.Collections.Generic;

namespace ComunityDetectorServices.Models
{
    public class ArticleReference
    {
        public string ArticleTitle { get; set; }
        public List<Author> Authors { get; set; }
    }
}
