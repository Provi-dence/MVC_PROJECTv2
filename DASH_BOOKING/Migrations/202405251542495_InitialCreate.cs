namespace DASH_BOOKING.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        CreatedAt = c.DateTime(nullable: false),
                        Role = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.EventImages",
                c => new
                    {
                        EventImageId = c.Int(nullable: false, identity: true),
                        EventModelId = c.Int(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.EventImageId)
                .ForeignKey("dbo.EventModels", t => t.EventModelId, cascadeDelete: true)
                .Index(t => t.EventModelId);
            
            CreateTable(
                "dbo.EventModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        EventTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        OrganizerEmail = c.String(),
                        OrganizerPhone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventRequests",
                c => new
                    {
                        EventRequestId = c.Int(nullable: false, identity: true),
                        EventName = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        EventDate = c.DateTime(nullable: false),
                        EventTime = c.Time(nullable: false, precision: 7),
                        Location = c.String(nullable: false),
                        OrganizerEmail = c.String(nullable: false),
                        OrganizerPhone = c.String(),
                    })
                .PrimaryKey(t => t.EventRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventImages", "EventModelId", "dbo.EventModels");
            DropIndex("dbo.EventImages", new[] { "EventModelId" });
            DropTable("dbo.EventRequests");
            DropTable("dbo.EventModels");
            DropTable("dbo.EventImages");
            DropTable("dbo.Users");
        }
    }
}
