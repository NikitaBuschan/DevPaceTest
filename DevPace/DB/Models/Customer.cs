namespace DevPace.DB.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Company: {CompanyName}, Phone: {Phone}, Email: {Email}";
        }
    }
}
