namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserDOBProp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "BirthDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BirthDate", c => c.DateTime());
        }
    }
}
