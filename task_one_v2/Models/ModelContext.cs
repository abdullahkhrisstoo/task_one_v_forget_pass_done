using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace task_one_v2.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chatroom> Chatrooms { get; set; }

    //public virtual DbSet<ChefRecipe> ChefRecipes { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    //public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Privacypolicy> Privacypolicies { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeApprovalStatus> RecipeApprovalStatuses { get; set; }

    public virtual DbSet<RecipeOrder> RecipeOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TermsCondition> TermsConditions { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<TestimonialApprovalStatus> TestimonialApprovalStatuses { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<UserVerificationStatus> UserVerificationStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##RECIPE_PROJECT_ONE;PASSWORD=0000;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##RECIPE_PROJECT_ONE")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Aboutid).HasName("SYS_C008610");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Aboutid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ABOUTID");
            entity.Property(e => e.Description)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image1)
                .HasMaxLength(255)
                .HasColumnName("IMAGE1");
            entity.Property(e => e.Image2)
                .HasMaxLength(255)
                .HasColumnName("IMAGE2");
            entity.Property(e => e.Image3)
                .HasMaxLength(255)
                .HasColumnName("IMAGE3");
            entity.Property(e => e.Image4)
                .HasMaxLength(255)
                .HasColumnName("IMAGE4");
            entity.Property(e => e.Numberofchef)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("NUMBEROFCHEF");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("TITLE");
            entity.Property(e => e.Yearsofexperience)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("YEARSOFEXPERIENCE");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("SYS_C008611");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("CATEGORY_NAME");
            entity.Property(e => e.ImageCategory)
                .HasMaxLength(255)
                .HasColumnName("IMAGE_CATEGORY");
        });

        modelBuilder.Entity<Chatroom>(entity =>
        {
            entity.HasKey(e => e.Chatroomid).HasName("SYS_C008612");

            entity.ToTable("CHATROOM");

            entity.Property(e => e.Chatroomid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHATROOMID");
            entity.Property(e => e.Chefid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHEFID");
            entity.Property(e => e.Messagetext)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGETEXT");
            entity.Property(e => e.Times)
                .HasPrecision(6)
                .HasColumnName("TIMES");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Chef).WithMany(p => p.ChatroomChefs)
                .HasForeignKey(d => d.Chefid)
                .HasConstraintName("SYS_C008631");

            entity.HasOne(d => d.User).WithMany(p => p.ChatroomUsers)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008630");
        });

        //modelBuilder.Entity<ChefRecipe>(entity =>
        //{
        //    entity.HasKey(e => e.ChefRecipeId).HasName("SYS_C008613");

        //    entity.ToTable("CHEF_RECIPE");

        //    entity.Property(e => e.ChefRecipeId)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnType("NUMBER(38)")
        //        .HasColumnName("CHEF_RECIPE_ID");
        //    entity.Property(e => e.CreationDate)
        //        .HasColumnType("DATE")
        //        .HasColumnName("CREATION_DATE");
        //    entity.Property(e => e.RecipeId)
        //        .HasColumnType("NUMBER(38)")
        //        .HasColumnName("RECIPE_ID");
        //    entity.Property(e => e.UserId)
        //        .HasColumnType("NUMBER(38)")
        //        .HasColumnName("USER_ID");

        //    entity.HasOne(d => d.Recipe).WithMany(p => p.ChefRecipes)
        //        .HasForeignKey(d => d.RecipeId)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("SYS_C008633");

        //    entity.HasOne(d => d.User).WithMany(p => p.ChefRecipes)
        //        .HasForeignKey(d => d.UserId)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("SYS_C008632");
        //});



        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Contactid).HasName("SYS_C008614");

            entity.ToTable("CONTACT_US");

            entity.Property(e => e.Contactid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CONTACTID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Homepageid).HasName("SYS_C008615");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Homepageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOMEPAGEID");
            entity.Property(e => e.Avatarimage)
                .HasMaxLength(255)
                .HasColumnName("AVATARIMAGE");
            entity.Property(e => e.Backgroundimage)
                .HasMaxLength(255)
                .HasColumnName("BACKGROUNDIMAGE");
            entity.Property(e => e.Description)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("TITLE");
        });

        //modelBuilder.Entity<Ingredient>(entity =>
        //{
        //    entity.HasKey(e => e.Ingredientid).HasName("SYS_C008616");

        //    entity.ToTable("INGREDIENTS");

        //    entity.Property(e => e.Ingredientid)
        //        .ValueGeneratedOnAdd()
        //        .HasColumnType("NUMBER(38)")
        //        .HasColumnName("INGREDIENTID");
        //    entity.Property(e => e.Ingredientname)
        //        .HasMaxLength(255)
        //        .HasColumnName("INGREDIENTNAME");
        //});

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C008617");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cvc)
                .HasMaxLength(3)
                .HasColumnName("CVC");
            entity.Property(e => e.Expiredate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIREDATE");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("FULLNAME");
            entity.Property(e => e.Numberid)
                .HasMaxLength(16)
                .HasColumnName("NUMBERID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008634");
        });

        modelBuilder.Entity<Privacypolicy>(entity =>
        {
            entity.HasKey(e => e.Policyid).HasName("SYS_C008618");

            entity.ToTable("PRIVACYPOLICY");

            entity.Property(e => e.Policyid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("POLICYID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("SYS_C008619");

            entity.ToTable("RECIPE");

            entity.Property(e => e.RecipeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.ApprovalStatusId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("APPROVAL_STATUS_ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.ChefId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHEF_ID");
            entity.Property(e => e.CreationDate)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.Description)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.ImageRecipe)
                .HasMaxLength(255)
                .HasColumnName("IMAGE_RECIPE");
            entity.Property(e => e.Ingredients)
                .HasColumnType("CLOB")
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Procedure)
                .HasColumnType("CLOB")
                .HasColumnName("PROCEDURE");
            entity.Property(e => e.Recipename)
                .HasMaxLength(255)
                .HasColumnName("RECIPENAME");

            entity.HasOne(d => d.ApprovalStatus).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ApprovalStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008637");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008636");

            entity.HasOne(d => d.Chef).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ChefId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008635");

            //entity.HasMany(d => d.IngredientsNavigation).WithMany(p => p.Recipes)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "RecipeIngredient",
            //        r => r.HasOne<Ingredient>().WithMany()
            //            .HasForeignKey("IngredientId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("SYS_C008639"),
            //        l => l.HasOne<Recipe>().WithMany()
            //            .HasForeignKey("RecipeId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("SYS_C008638"),
            //        j =>
            //        {
            //            j.HasKey("RecipeId", "IngredientId").HasName("SYS_C008620");
            //            j.ToTable("RECIPE_INGREDIENTS");
            //            j.IndexerProperty<decimal>("RecipeId")
            //                .HasColumnType("NUMBER(38)")
            //                .HasColumnName("RECIPE_ID");
            //            j.IndexerProperty<decimal>("IngredientId")
            //                .HasColumnType("NUMBER(38)")
            //                .HasColumnName("INGREDIENT_ID");
            //        });
        });

        modelBuilder.Entity<RecipeApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.ApprovalStatusId).HasName("SYS_C008622");

            entity.ToTable("RECIPE_APPROVAL_STATUS");

            entity.Property(e => e.ApprovalStatusId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("APPROVAL_STATUS_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("STATUS_NAME");
        });

        modelBuilder.Entity<RecipeOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("SYS_C008621");

            entity.ToTable("RECIPE_ORDER");

            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.OrderDate)
                .HasColumnType("DATE")
                .HasColumnName("ORDER_DATE");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTAL_PRICE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");

            //entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeOrders)
            //    .HasForeignKey(d => d.RecipeId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("SYS_C008641");

            entity.HasOne(d => d.User).WithMany(p => p.RecipeOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008640");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C008623");

            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<TermsCondition>(entity =>
        {
            entity.HasKey(e => e.TermsId).HasName("SYS_C008624");

            entity.ToTable("TERMS_CONDITIONS");

            entity.Property(e => e.TermsId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TERMS_ID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("SYS_C008625");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.TestimonialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIAL_ID");
            entity.Property(e => e.ApprovalStatusId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("APPROVAL_STATUS_ID");
            entity.Property(e => e.DateTime)
                .HasColumnType("DATE")
                .HasColumnName("DATE_TIME");
            entity.Property(e => e.TestimonialText)
                .HasColumnType("CLOB")
                .HasColumnName("TESTIMONIAL_TEXT");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Userimg)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USERIMG");

            entity.HasOne(d => d.ApprovalStatus).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.ApprovalStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008643");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008642");
        });

        modelBuilder.Entity<TestimonialApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.ApprovalStatusId).HasName("SYS_C008626");

            entity.ToTable("TESTIMONIAL_APPROVAL_STATUS");

            entity.Property(e => e.ApprovalStatusId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("APPROVAL_STATUS_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("STATUS_NAME");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008627");

            entity.ToTable("USER_CREDENTIALS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.UserCredentials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008644");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008628");

            entity.ToTable("USER_INFO");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.CertificateImage)
                .HasMaxLength(255)
                .HasColumnName("CERTIFICATE_IMAGE");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("IMAGE");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Nationality)
                .HasMaxLength(255)
                .HasColumnName("NATIONALITY");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.VerificationStatusId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("VERIFICATION_STATUS_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008645");

            entity.HasOne(d => d.VerificationStatus).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.VerificationStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008646");
        });

        modelBuilder.Entity<UserVerificationStatus>(entity =>
        {
            entity.HasKey(e => e.VerificationStatusId).HasName("SYS_C008629");

            entity.ToTable("USER_VERIFICATION_STATUS");

            entity.Property(e => e.VerificationStatusId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("VERIFICATION_STATUS_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("STATUS_NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
