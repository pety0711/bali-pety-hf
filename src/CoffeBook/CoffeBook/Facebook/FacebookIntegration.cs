using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;

namespace CoffeBook.Facebook
{
    class FacebookIntegration
    {
        public FacebookClient GetClient()
        {
            return new FacebookClient
            {
                AppId = "574708729398919",
                AppSecret = "5e3da227006e0d338a1427355615ebf8"
            };
        }
    }
}
