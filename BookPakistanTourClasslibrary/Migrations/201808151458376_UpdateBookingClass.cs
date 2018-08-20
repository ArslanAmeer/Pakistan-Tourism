namespace BookPakistanTourClasslibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingClass : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Bookings", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Bookings", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Bookings", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Bookings", name: "UserId", newName: "User_Id");
        }
    }
}
