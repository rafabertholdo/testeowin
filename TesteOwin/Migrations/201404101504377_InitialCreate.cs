namespace TesteOwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Testes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Duration = c.Int(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ReasonEnd = c.Int(nullable: false),
                        NameEnd = c.String(),
                        EventType = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        NameStart = c.String(),
                        ReasonStart = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Testes");
        }
    }
}
