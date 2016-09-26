namespace TableSplitting.Models.Combined
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class BusinessProject
    {
        [Key, ForeignKey("Project")]
        public int Id { get; set; }

        // Navigation properties
        public virtual Project Project { get; set; }

        [Required]
        public virtual BusinessProjectOptions Options { get; set; }

        public virtual ICollection<ProjectComponent> Components { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[Business project]");

            sb.Append(Options);

            if (Components != null)
            {
                foreach (var component in Components)
                {
                    sb.AppendLine();
                    sb.Append(component);
                }
            }

            return sb.ToString();
        }
    }
}