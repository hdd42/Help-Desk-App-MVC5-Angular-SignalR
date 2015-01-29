namespace HelpDeskTicketingApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDateTimeIssue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Issues", "DateReported", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Issues", "DateResolved", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Issues", "DateResolved", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Issues", "DateReported", c => c.DateTime(nullable: false));
        }
    }
}
