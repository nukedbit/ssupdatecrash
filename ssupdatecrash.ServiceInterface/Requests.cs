using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ssupdatecrash.ServiceModel;

namespace ssupdatecrash.ServiceInterface {
   
    public class CreatePerson : ICreateDb<Person>, IReturn<CreateResponse> {
        public string Name { get; set; }
    }   

    public class UpdatePerson : IUpdateDb<Person>, IReturn<UpdateResponse> {
        public int Id { get; set; }
        public string Name { get; set; }
    }  

    public class CreateResponse {
        public int Id { get; set; }
        public Person Result { get; set; }
        
        public ResponseStatus ResponseStatus { get; set; }
    }
    
    public class UpdateResponse {
        public int Id { get; set; }
        public Person Result { get; set; }
        
        public ResponseStatus ResponseStatus { get; set; }
    }


    [Route("/create")]
    public class CreatePersonGateway : IReturn<Person>
    {
        public string Name { get; set; }
    }
    
    
    [Route("/update")]
    public class UpdatePersonGateway : IReturn<Person>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateService : Service
    {
        public async Task<object> Post(UpdatePersonGateway request)
        {
            try
            {
                var response = await Gateway.SendAsync(new UpdatePerson()
                {
                    Id = request.Id,
                    Name = request.Name
                });
            
                return response.Result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public async Task<object> Post(CreatePersonGateway request)
        {
            try
            {
                var response = await Gateway.SendAsync(new CreatePerson()
                {
                    Name = request.Name
                });
            
                return response.Result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}