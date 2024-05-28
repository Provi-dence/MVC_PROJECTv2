namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPasswordHashedToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsPasswordHashed", c => c.Boolean(nullable: false));

            Sql("UPDATE dbo.Users SET IsPasswordHashed = 1");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsPasswordHashed");
        }
    }
}
