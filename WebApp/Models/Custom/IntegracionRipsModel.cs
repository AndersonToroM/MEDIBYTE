namespace Blazor.WebApp.Models
{
    public class IntegracionRipsModel
    {
        public bool HuboError { get; set; }
        public string Error { get; set; }
        public virtual int? HttpStatus { get; set; }
    }
}
