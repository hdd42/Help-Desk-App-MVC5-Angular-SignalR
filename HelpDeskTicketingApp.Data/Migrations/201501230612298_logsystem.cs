namespace HelpDeskTicketingApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logsystem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeLineLogs",
                c => new
                    {
                        EntryId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        TimeHappened = c.DateTime(),
                        EntryType = c.String(),
                    })
                .PrimaryKey(t => t.EntryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TimeLineLogs");
        }
    }
}
