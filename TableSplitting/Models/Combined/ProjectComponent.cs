namespace TableSplitting.Models.Combined
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class ProjectComponent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("BusinessProject")]
        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string ManufacturerCode { get; set; }

        // Navigation properties
        public virtual BusinessProject BusinessProject { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[Name: {Name}, Manufacturer code: {ManufacturerCode}]");

            return sb.ToString();
        }
    }
}
