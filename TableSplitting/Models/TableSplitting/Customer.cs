namespace TableSplitting.Models.TableSplitting
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("Customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public double MaxCredit { get; set; }

        // Navigation properties
        [Required]
        public virtual CustomerAddress Address { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[{Name}, maximum credit allowed: {MaxCredit}]");

            if (Address != null)
            {
                sb.Append(Address);
            }

            return sb.ToString();
        }
    }
}
