namespace instgaram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sender1_id = c.Int(nullable: false),
                        resever1_id = c.Int(nullable: false),
                        resever_Id = c.Int(),
                        sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.resever_Id)
                .ForeignKey("dbo.Users", t => t.sender_Id)
                .Index(t => t.resever_Id)
                .Index(t => t.sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.friends", "sender_Id", "dbo.Users");
            DropForeignKey("dbo.friends", "resever_Id", "dbo.Users");
            DropIndex("dbo.friends", new[] { "sender_Id" });
            DropIndex("dbo.friends", new[] { "resever_Id" });
            DropTable("dbo.friends");
        }
    }
}
