namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNamePropinFeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "Name");
        }
    }
}
