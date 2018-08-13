namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTourDepDateProp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tours", "DepartureDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tours", "DepartureDate", c => c.DateTime());
        }
    }
}
