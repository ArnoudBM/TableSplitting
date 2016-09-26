namespace TableSplitting.Models.OneToZeroOrOne
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PersonnelNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Navigation properties
        public virtual Laptop Laptop { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[{FirstName} {LastName} ({PersonnelNumber})]");

            return sb.ToString();
        }
    }
}
