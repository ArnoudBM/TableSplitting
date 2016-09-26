namespace TableSplitting.Models.Combined
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    [Table("BusinessProject")]
    public class BusinessProjectOptions
    {
        [Key, ForeignKey("BusinessProject")]
        public int Id { get; set; }

        public bool HasDiscount { get; set; }

        public string Contact { get; set; }

        // Navigation properties
        public virtual BusinessProject BusinessProject { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[Contact: {Contact}, Discount: {HasDiscount}]");

            return sb.ToString();
        }
    }
}
