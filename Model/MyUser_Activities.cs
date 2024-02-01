using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace myappdotnet.Model;

public class MyUser_Activities
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    public int ActivityId { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public MyUser User { get; set; }

    [ForeignKey("ActivityId")]
    public Activities Activity { get; set; }
}


public class MyUser_ActivitiesConfiguration : IEntityTypeConfiguration<MyUser_Activities>
{
    public void Configure(EntityTypeBuilder<MyUser_Activities> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();


        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);

        builder.HasOne(a => a.Activity)
            .WithMany()
            .HasForeignKey(a => a.ActivityId);
    }
}