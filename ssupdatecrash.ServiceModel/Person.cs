using ServiceStack.DataAnnotations;

namespace ssupdatecrash.ServiceModel {
    public class Person {
        [AutoIncrement]
        public int Id { get; set; }

        public string Name {get;set;}
    }
}