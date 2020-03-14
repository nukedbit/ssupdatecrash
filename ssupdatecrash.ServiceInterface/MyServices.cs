using System;
using ServiceStack;
using ssupdatecrash.ServiceModel;

namespace ssupdatecrash.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
