namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Status");
        }
    }
}
