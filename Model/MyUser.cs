using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace myappdotnet.Model;

public class MyUser
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }

    public string Surname { get; set; }

    public string First_name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class MyUserConfiguration : IEntityTypeConfiguration<MyUser>
{
    public void Configure(EntityTypeBuilder<MyUser> builder)
    {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email).IsUnique();
    }
}

