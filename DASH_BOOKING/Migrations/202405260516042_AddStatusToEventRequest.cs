namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToEventRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRequests", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRequests", "Status");
        }
    }
}
