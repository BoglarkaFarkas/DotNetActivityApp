using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace myappdotnet.Model;

public class Activities
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public string Time { get; set; }

    public int LocationId { get; set; }

    [ForeignKey("LocationId")]
    public Location Location { get; set; }
}

public class ActivitiesConfiguration : IEntityTypeConfiguration<Activities>
{
    public void Configure(EntityTypeBuilder<Activities> builder)
    {
        builder.HasKey(u => u.Id);
    }
}

