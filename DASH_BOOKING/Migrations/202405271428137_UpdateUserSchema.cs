﻿namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserSchema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventModels", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventModels", "Status");
        }
    }
}
