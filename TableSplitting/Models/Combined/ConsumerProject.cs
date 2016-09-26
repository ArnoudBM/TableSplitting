namespace TableSplitting.Models.Combined
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class ConsumerProject
    {
        [Key, ForeignKey("Project")]
        public int Id { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"[Consumer project]");

            return sb.ToString();
        }
    }
}