
using PopravkaBa.Domain.Enums;


namespace PopravkaBa.Domain.Models
{
    public class Administrator : ApplicationUser
    {
        public override Status Aktivan() => Status.Aktivan;
        
    }
}
