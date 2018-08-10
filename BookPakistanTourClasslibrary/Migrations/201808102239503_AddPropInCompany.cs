namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropInCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "ImageUrl");
        }
    }
}
