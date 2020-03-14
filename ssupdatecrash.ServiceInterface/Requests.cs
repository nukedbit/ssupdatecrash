using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ssupdatecrash.ServiceModel;

namespace ssupdatecrash.ServiceInterface {
    public interface IAudit 
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        string CreatedInfo { get; set; }
        DateTime ModifiedDate { get; set; }
        string ModifiedBy { get; set; }
        string ModifiedInfo { get; set; }
        DateTime? SoftDeletedDate { get; set; }
        string SoftDeletedBy { get; set; }
        string SoftDeletedInfo { get; set; }
    }
    
    public abstract class AuditBase : IAudit
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CreatedInfo { get; set; }

        public DateTime ModifiedDate { get; set; }
        [Required]
        public string ModifiedBy { get; set; }
        [Required]
        public string ModifiedInfo { get; set; }

        [Index] //Check if Deleted
        public DateTime? SoftDeletedDate { get; set; }
        public string SoftDeletedBy { get; set; }
        public string SoftDeletedInfo { get; set; }
    }
    
 
    [AutoPopulate(nameof(IAudit.CreatedDate),  Eval = "utcNow")]
    [AutoPopulate(nameof(IAudit.CreatedBy),    Eval = "userAuthName")] //or userAuthId
    [AutoPopulate(nameof(IAudit.CreatedInfo),  Eval = "`${userSession.DisplayName} (${userSession.City})`")]
    [AutoPopulate(nameof(IAudit.ModifiedDate), Eval = "utcNow")]
    [AutoPopulate(nameof(IAudit.ModifiedBy),   Eval = "userAuthName")] //or userAuthId
    [AutoPopulate(nameof(IAudit.ModifiedInfo), Eval = "`${userSession.DisplayName} (${userSession.City})`")]
    public abstract class CreateAuditBase<Table,TResponse> : ICreateDb<Table>, IReturn<TResponse> {}
    
 
    [AutoPopulate(nameof(IAudit.ModifiedDate), Eval = "utcNow")]
    [AutoPopulate(nameof(IAudit.ModifiedBy),   Eval = "userAuthName")] //or userAuthId
    [AutoPopulate(nameof(IAudit.ModifiedInfo), Eval = "`${userSession.DisplayName} (${userSession.City})`")]
    public abstract class UpdateAuditBase<Table,TResponse> : IUpdateDb<Table>, IReturn<TResponse> {}


    public class CreatePerson : CreateAuditBase<Person,CreateResponse> {
        public string Name { get; set; }
    }   

    public class UpdatePerson : UpdateAuditBase<Person,UpdateResponse> {
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
                var response = await Gateway.SendAsync(new UpdatePerson
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
    }
}