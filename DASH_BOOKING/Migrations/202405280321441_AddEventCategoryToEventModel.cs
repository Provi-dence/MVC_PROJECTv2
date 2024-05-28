namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventCategoryToEventModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventModels", "EventCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventModels", "EventCategory");
        }
    }
}
