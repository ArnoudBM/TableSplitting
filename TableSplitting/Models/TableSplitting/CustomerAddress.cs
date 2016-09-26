namespace TableSplitting.Models.TableSplitting
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("Customer")]
    public class CustomerAddress
    {
        [Key, ForeignKey("Customer")]
        public int Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[{Address}, {City}]");

            return sb.ToString();
        }
    }
}
