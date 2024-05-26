namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEventRequestStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventRequests", "Status", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventRequests", "Status", c => c.String());
        }
    }
}
