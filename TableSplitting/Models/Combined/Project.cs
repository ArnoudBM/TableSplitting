namespace TableSplitting.Models.Combined
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateAndTimeCreated { get; set; }

        public DateTime DateAndTimeLastModified { get; set; }

        // Navigation properties
        public virtual BusinessProject BusinessProject { get; set; }

        public virtual ConsumerProject ConsumerProject { get; set; }

        // Helper methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[{Name} (Created: {DateAndTimeCreated:G}, Last modified: {DateAndTimeLastModified:G})]");

            if (ConsumerProject != null)
            {
                sb.Append(ConsumerProject);
            }

            if (BusinessProject != null)
            {
                sb.Append(BusinessProject);
            }

            return sb.ToString();
        }
    }
}
