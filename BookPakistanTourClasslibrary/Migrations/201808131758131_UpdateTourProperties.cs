namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTourProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedbacks", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Feedbacks", new[] { "Company_Id" });
            AddColumn("dbo.Feedbacks", "DateEntered", c => c.String());
            DropColumn("dbo.Feedbacks", "Company_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedbacks", "Company_Id", c => c.Int());
            DropColumn("dbo.Feedbacks", "DateEntered");
            CreateIndex("dbo.Feedbacks", "Company_Id");
            AddForeignKey("dbo.Feedbacks", "Company_Id", "dbo.Companies", "Id");
        }
    }
}
