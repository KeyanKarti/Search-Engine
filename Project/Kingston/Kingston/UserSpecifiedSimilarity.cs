using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Search;

namespace Kingston
{
    public class UserSpecifiedSimilarity : DefaultSimilarity
    {
        public override float Tf(float freq)
        {
            return 1.0f;
        }
    }
}
