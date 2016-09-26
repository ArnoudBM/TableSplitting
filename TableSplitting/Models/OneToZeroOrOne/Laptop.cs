namespace TableSplitting.Models.OneToZeroOrOne
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Laptop
    {
        [Key, ForeignKey("Employee")]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[{Type} ({Code})]");

            return sb.ToString();
        }
    }
}
