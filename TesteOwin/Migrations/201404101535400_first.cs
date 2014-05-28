namespace TesteOwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Testes", "EndDate");
            DropColumn("dbo.Testes", "ReasonEnd");
            DropColumn("dbo.Testes", "EventType");
            DropColumn("dbo.Testes", "Value");
            DropColumn("dbo.Testes", "StartDate");
            DropColumn("dbo.Testes", "NameStart");
            DropColumn("dbo.Testes", "ReasonStart");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Testes", "ReasonStart", c => c.Int(nullable: false));
            AddColumn("dbo.Testes", "NameStart", c => c.String());
            AddColumn("dbo.Testes", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Testes", "Value", c => c.Int(nullable: false));
            AddColumn("dbo.Testes", "EventType", c => c.Int(nullable: false));
            AddColumn("dbo.Testes", "ReasonEnd", c => c.Int(nullable: false));
            AddColumn("dbo.Testes", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
