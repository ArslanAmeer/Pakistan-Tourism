namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tour_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tours", t => t.Tour_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Tour_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Single(nullable: false),
                        Sale = c.Single(nullable: false),
                        DepartureDate = c.DateTime(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        FacebookPageUrl = c.String(),
                        City_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TourImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false),
                        Caption = c.String(),
                        Tour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tours", t => t.Tour_Id)
                .Index(t => t.Tour_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FullAddress = c.String(nullable: false),
                        ImageUrl = c.String(),
                        BirthDate = c.DateTime(),
                        IsActive = c.Boolean(),
                        Phone = c.Long(nullable: false),
                        Male = c.Boolean(nullable: false),
                        Female = c.Boolean(nullable: false),
                        City_Id = c.Int(),
                        Role_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .Index(t => t.City_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Rank = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        Company_Id = c.Int(),
                        Tour_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .ForeignKey("dbo.Tours", t => t.Tour_Id)
                .Index(t => t.Company_Id)
                .Index(t => t.Tour_Id);
            
            CreateTable(
                "dbo.MainBanners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Caption = c.String(),
                        BannerUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Feedbacks", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Bookings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Users", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Bookings", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.TourImages", "Tour_Id", "dbo.Tours");
            DropForeignKey("dbo.Tours", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Companies", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Cities", "Country_Id", "dbo.Countries");
            DropIndex("dbo.Feedbacks", new[] { "Tour_Id" });
            DropIndex("dbo.Feedbacks", new[] { "Company_Id" });
            DropIndex("dbo.Users", new[] { "Role_Id" });
            DropIndex("dbo.Users", new[] { "City_Id" });
            DropIndex("dbo.TourImages", new[] { "Tour_Id" });
            DropIndex("dbo.Cities", new[] { "Country_Id" });
            DropIndex("dbo.Companies", new[] { "City_Id" });
            DropIndex("dbo.Tours", new[] { "Company_Id" });
            DropIndex("dbo.Bookings", new[] { "User_Id" });
            DropIndex("dbo.Bookings", new[] { "Tour_Id" });
            DropTable("dbo.MainBanners");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.TourImages");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.Companies");
            DropTable("dbo.Tours");
            DropTable("dbo.Bookings");
        }
    }
}
